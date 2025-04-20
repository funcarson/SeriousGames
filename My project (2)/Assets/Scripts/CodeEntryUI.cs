// CodeEntryUI.cs
using UnityEngine;
using UnityEngine.UI;

public class CodeEntryUI : MonoBehaviour
{
    public InputField codeInput;
    public Button submitButton;
    public Text feedbackText;

    // Map of valid codes to unlock keys
    private Dictionary<string, string> codeMap = new Dictionary<string, string> {
        {"SOLAR123", "SolarPanel"},
        // add other codes
    };

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        string code = codeInput.text.Trim().ToUpper();
        if (codeMap.ContainsKey(code) && !GameManager.Instance.unlockedCodes.Contains(code))
        {
            GameManager.Instance.unlockedCodes.Add(code);
            feedbackText.text = "Unlocked: " + codeMap[code];
        }
        else
        {
            feedbackText.text = "Invalid or already used code.";
        }
    }
}
