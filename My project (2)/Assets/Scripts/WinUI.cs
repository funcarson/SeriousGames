// WinUI.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text codeText;
    public Button restartButton;  // renamed from menuButton
    public Button quitButton;

    void Start()
    {
        // Display your win code
        codeText.text = $"Your Code: {GameManager.Instance.winCode}";

        // Restart: full reset and back to Main Menu
        restartButton.onClick.AddListener(() =>
        {
            // Completely clear all saved and run data
            GameManager.Instance.ClearProgress();
            GameManager.Instance.ClearRunData();

            // Destroy all Canvases except the one you really want
            foreach (var cv in FindObjectsOfType<Canvas>())
            {
                if (cv.gameObject.name != "MainCanvas") // or tag it specifically
                    Destroy(cv.gameObject);
            }

            // Load the Main Menu scene fresh
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        });

        // Quit the application
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}

