// GameManager.cs
using UnityEngine;
using System.Collections.Generic;

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
        // TODO: Implement persistent save (PlayerPrefs or JSON)
    }

    private void LoadProgress()
    {
        // TODO: Implement load; if none, keep defaults
    }
}
