// GameManager.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Progress & Unlocks")]
    public int currentBudget = 100;
    public List<string> unlockedCodes = new List<string>();
    public HashSet<string> discoveredResourceIds = new HashSet<string>();

    [HideInInspector] public RoverConfiguration currentConfig;
    [HideInInspector] public int missionScore;
    [HideInInspector] public int missionSamples;
    [HideInInspector] public int missionPhotos;

    private const string KEY_BUDGET = "Budget";
    private const string KEY_UNLOCKED = "UnlockedCodes";
    private const string KEY_DISCOVERED = "DiscoveredResources";

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

    public void EndMission()
    {
        // Convert missionScore to next budget
        currentBudget += missionScore;
        SaveProgress();
    }

    private void SaveProgress()
    {
        // Save simple values
        PlayerPrefs.SetInt(KEY_BUDGET, currentBudget);

        // Serialize list and set as CSV
        if (unlockedCodes != null && unlockedCodes.Count > 0)
            PlayerPrefs.SetString(KEY_UNLOCKED, string.Join(",", unlockedCodes));
        else
            PlayerPrefs.DeleteKey(KEY_UNLOCKED);

        if (discoveredResourceIds != null && discoveredResourceIds.Count > 0)
            PlayerPrefs.SetString(KEY_DISCOVERED, string.Join(",", discoveredResourceIds));
        else
            PlayerPrefs.DeleteKey(KEY_DISCOVERED);

        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        // Load budget
        if (PlayerPrefs.HasKey(KEY_BUDGET))
            currentBudget = PlayerPrefs.GetInt(KEY_BUDGET);

        // Load unlocked codes
        if (PlayerPrefs.HasKey(KEY_UNLOCKED))
        {
            var csv = PlayerPrefs.GetString(KEY_UNLOCKED);
            unlockedCodes = csv
                .Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        // Load discovered resources
        if (PlayerPrefs.HasKey(KEY_DISCOVERED))
        {
            var csv = PlayerPrefs.GetString(KEY_DISCOVERED);
            discoveredResourceIds = new HashSet<string>(
                csv.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
            );
        }
    }
}