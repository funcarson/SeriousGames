// MenuController.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Button startButton;
    public Button codeEntryButton;
    public GameObject codeEntryPanel;

    private void Start()
    {
        startButton.onClick.AddListener(() => {
            SceneManager.LoadScene("RoverBuilder");
        });
        codeEntryButton.onClick.AddListener(() => {
            codeEntryPanel.SetActive(true);
        });
    }
}

