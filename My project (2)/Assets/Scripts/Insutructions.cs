using UnityEngine;
using UnityEngine.UI;

public class InstructionsPanel : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [SerializeField] private GameObject panel;      // The UI Panel GameObject
    [SerializeField] private Button openButton;     // Button that opens the panel
    [SerializeField] private Button closeButton;    // Button inside panel to close it

    private void Awake()
    {
        // Start with the instructions panel hidden
        panel.SetActive(false);

        // Wire up the click callbacks
        openButton.onClick.AddListener(OpenPanel);
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void OpenPanel()
    {
        panel.SetActive(true);
    }

    private void ClosePanel()
    {
        panel.SetActive(false);
    }
}

