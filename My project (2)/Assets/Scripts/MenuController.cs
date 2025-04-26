// MenuController.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Button startButton;
    public Button codeEntryButton;
    public GameObject codeEntryPanel;
    public Button quitButton;

    private void Start()
    {
        startButton.onClick.AddListener(() => {
            SceneManager.LoadScene("RoverBuilder");
        });
        codeEntryButton.onClick.AddListener(() => {
            codeEntryPanel.SetActive(true);
            startButton.gameObject.SetActive(false);
            codeEntryButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);
        });

        // Quit the application
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}

