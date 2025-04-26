using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MissionSummaryUI : MonoBehaviour
{
    public TMP_Text photosText;
    public TMP_Text scoreText;
    public TMP_Text newBudgetText;
    public Button nextButton;

    void Start()
    {
        //Populate from GameManager
        photosText.text = $"Photos Taken: {GameManager.Instance.missionPhotos}";
        scoreText.text = $"Science Points: {GameManager.Instance.missionScore}";
        newBudgetText.text = $"New Budget: {GameManager.Instance.currentBudget}";

        nextButton.onClick.AddListener(() =>
            SceneManager.LoadScene("RoverBuilder")
        );
    }
}

