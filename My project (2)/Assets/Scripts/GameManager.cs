// GameManager.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Progress & Unlocks")]
    public int currentBudget = 100;
    public List<string> unlockedCodes = new List<string>();

    [HideInInspector] public RoverConfiguration currentConfig;
    [HideInInspector] public int missionScore;
    [HideInInspector] public int missionSamples;
    [HideInInspector] public int missionPhotos;

    // Tracks what you've collected *this mission*
    [HideInInspector] public HashSet<string> discoveredResourceIds = new HashSet<string>();

    // Set at scene start to the number of hotspots in this mission
    [HideInInspector] public int totalResourceTypes;

    // Code to display when you win
    public string winCode = "ROVERWIN";

    private const string KEY_BUDGET = "Budget";
    private const string KEY_UNLOCKED = "UnlockedCodes";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        // Clear persistent save when the game exits
        ClearProgress();
    }

    /// <summary>
    /// Clears only the in-mission data so you can start a new run.
    /// </summary>
    public void ClearRunData()
    {
        discoveredResourceIds.Clear();
        missionScore = 0;
        missionSamples = 0;
        missionPhotos = 0;
    }

    /// <summary>
    /// Clears all saved progress (budget & unlocked codes).
    /// </summary>
    public void ClearProgress()
    {
        PlayerPrefs.DeleteKey(KEY_BUDGET);
        PlayerPrefs.DeleteKey(KEY_UNLOCKED);
        PlayerPrefs.Save();

        currentBudget = 100;
        unlockedCodes.Clear();
    }

    /// <summary>
    /// Call when a mission ends to update budget and transition.
    /// </summary>
    public void EndMission()
    {
        // Apply this run's score to the budget
        currentBudget += missionScore;
        SaveProgress();

        // Decide which scene to go to
        if (discoveredResourceIds.Count >= totalResourceTypes)
        {
            // Full clear => win
            ClearProgress();
            ClearRunData();
            SceneManager.LoadScene("WinScene");
        }
        else
        {
            SceneManager.LoadScene("MissionSummary");
        }
    }

    /// <summary>
    /// Immediately checks if you've collected all hotspots this run, and if so, sends you to the Win screen.
    /// </summary>
    public void CheckForWin()
    {
        if (discoveredResourceIds.Count >= totalResourceTypes)
        {
            ClearProgress();
            ClearRunData();
            SceneManager.LoadScene("WinScene");
        }
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(KEY_BUDGET, currentBudget);

        if (unlockedCodes != null && unlockedCodes.Count > 0)
            PlayerPrefs.SetString(KEY_UNLOCKED, string.Join(",", unlockedCodes));
        else
            PlayerPrefs.DeleteKey(KEY_UNLOCKED);

        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        if (PlayerPrefs.HasKey(KEY_BUDGET))
            currentBudget = PlayerPrefs.GetInt(KEY_BUDGET);

        if (PlayerPrefs.HasKey(KEY_UNLOCKED))
        {
            var csv = PlayerPrefs.GetString(KEY_UNLOCKED);
            unlockedCodes = csv
                .Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }
    }
}
