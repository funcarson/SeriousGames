// CodeEntryUI.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CodeEntryUI : MonoBehaviour
{
    public InputField codeInput;
    public Button submitButton;
    public Text feedbackText;
    public Button exit;
    public Button codeButton;
    public Button quitButton;
    public Button startButton;
    public GameObject codeEntryPanel;

    // Map of valid codes to unlock keys
    private Dictionary<string, string> codeMap = new Dictionary<string, string> {
        {"SOLAR123", "SolarPanel"},
        // add other codes
    };

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);

        exit.onClick.AddListener(() =>
        {
            codeEntryPanel.SetActive(false);
            startButton.gameObject.SetActive(true);
            codeButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
        });
    }
    void OnSubmit()
    {
        if (codeInput == null || submitButton == null || feedbackText == null)
        {
            Debug.LogError("Assign all CodeEntryUI fields in the Inspector");
            return;
        }
        string code = codeInput.text.Trim().ToUpper();
        if (codeMap.ContainsKey(code) && !GameManager.Instance.unlockedCodes.Contains(code))
        {
            GameManager.Instance.unlockedCodes.Add(code);
            feedbackText.text = "Unlocked: " + codeMap[code];

            // Also reveal the special row if it's in this scene:
            var builderUI = FindObjectOfType<RoverBuilderUI>();
            if (builderUI != null) builderUI.ShowSpecialRow();
        }
        else
        {
            feedbackText.text = "Invalid or already used code.";
        }
    }
}
