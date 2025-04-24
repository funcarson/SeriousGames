// RoverBuilderUI.cs
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RoverBuilderUI : MonoBehaviour
{
    [Header("Options")]
    public PartDefinition[] tracksOptions;
    public PartDefinition[] batteryOptions;
    public PartDefinition[] cameraOptions;
    public PartDefinition[] scannerOptions;
    public PartDefinition[] specialOptions;

    [Header("Dropdowns")]
    public Dropdown tracksDropdown;
    public Dropdown batteryDropdown;
    public Dropdown cameraDropdown;
    public Dropdown scannerDropdown;
    public Dropdown specialDropdown;

    [Header("Stat Texts")]
    public Text budgetText;
    public Text speedText;
    public Text batteryLifeText;
    public Text warningText;
    public Button launchButton;

    [Header("Tracks Row UI")]
    public Text tracksCostText;
    public Image tracksIcon;

    [Header("Battery Row UI")]
    public Text batteryCostText;
    public Image batteryIcon;

    [Header("Camera Row UI")]
    public Text cameraCostText;
    public Image cameraIcon;

    [Header("Scanner Row UI")]
    public Text scannerCostText;
    public Image scannerIcon;

    [Header("Special Row UI")]
    public Text specialCostText;
    public Image specialIcon;

    private RoverConfiguration config;

    void Start()
    {
        config = new RoverConfiguration();

        Populate(tracksDropdown, tracksOptions);
        Populate(batteryDropdown, batteryOptions);
        Populate(cameraDropdown, cameraOptions);
        Populate(scannerDropdown, scannerOptions);
        Populate(specialDropdown, specialOptions);

        tracksDropdown.onValueChanged.AddListener(_ => OnSelectionChanged());
        batteryDropdown.onValueChanged.AddListener(_ => OnSelectionChanged());
        cameraDropdown.onValueChanged.AddListener(_ => OnSelectionChanged());
        scannerDropdown.onValueChanged.AddListener(_ => OnSelectionChanged());
        specialDropdown.onValueChanged.AddListener(_ => OnSelectionChanged());

        launchButton.onClick.AddListener(OnLaunch);

        OnSelectionChanged();
    }

    void Populate(Dropdown dd, PartDefinition[] options)
    {
        dd.ClearOptions();
        dd.AddOptions(options.Select(o => o.partName).ToList());
    }

    void OnSelectionChanged()
    {
        config.tracks = tracksOptions[tracksDropdown.value];
        config.battery = batteryOptions[batteryDropdown.value];
        config.camera = cameraOptions[cameraDropdown.value];
        config.scanner = scannerOptions[scannerDropdown.value];
        config.special = specialOptions[specialDropdown.value];

        UpdateUI();
    }

    void UpdateUI()
    {
        // 1) update totals & stats
        int totalCost = config.tracks.cost
                      + config.battery.cost
                      + config.camera.cost
                      + config.scanner.cost
                      + (config.special?.cost ?? 0);

        budgetText.text = $"Budget: {totalCost}/{GameManager.Instance.currentBudget}";
        speedText.text = $"Speed: {config.tracks.speedModifier:F1}";
        batteryLifeText.text = $"Battery Life: {config.battery.capacityModifier:F0}";
        bool valid = totalCost <= GameManager.Instance.currentBudget;
        warningText.text = valid ? "" : "Over budget!";
        launchButton.interactable = valid;

        // 2) update each row explicitly
        tracksCostText.text = $"${config.tracks.cost}";
        tracksIcon.sprite = config.tracks.icon;

        batteryCostText.text = $"${config.battery.cost}";
        batteryIcon.sprite = config.battery.icon;

        cameraCostText.text = $"${config.camera.cost}";
        cameraIcon.sprite = config.camera.icon;

        scannerCostText.text = $"${config.scanner.cost}";
        scannerIcon.sprite = config.scanner.icon;

        specialCostText.text = $"${config.special.cost}";
        specialIcon.sprite = config.special.icon;
    }

    void OnLaunch()
    {
        GameManager.Instance.currentConfig = config;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MarsTerrain");
    }
}
