// MissionSummaryUI.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionSummaryUI : MonoBehaviour
{
    public Text samplesText;
    public Text photosText;
    public Text scoreText;
    public Text newBudgetText;
    public Button nextButton;

    void Start()
    {
        samplesText.text = $"Samples Collected: {GameManager.Instance.missionSamples}";
        photosText.text = $"Photos Taken: {GameManager.Instance.missionPhotos}";
        scoreText.text = $"Science Points: {GameManager.Instance.missionScore}";
        newBudgetText.text = $"New Budget: {GameManager.Instance.currentBudget}";
        nextButton.onClick.AddListener(() => {
            SceneManager.LoadScene("RoverBuilder");
        });
    }
}
