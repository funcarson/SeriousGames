// RoverBuilderUI.cs
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

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
    public GameObject specialRow;     // container for the entire Special row
    public Text specialCostText;
    public Image specialIcon;

    private RoverConfiguration config;

    void Start()
    {
        config = new RoverConfiguration();

        // Initially show/hide special row
        bool isUnlocked = GameManager.Instance != null
                          && GameManager.Instance.unlockedCodes.Contains("SOLAR123");
        specialRow.SetActive(isUnlocked);

        Populate(tracksDropdown, tracksOptions);
        Populate(batteryDropdown, batteryOptions);
        Populate(cameraDropdown, cameraOptions);
        Populate(scannerDropdown, scannerOptions);

        if (isUnlocked)
            Populate(specialDropdown, specialOptions);
        else
        {
            specialDropdown.ClearOptions();
            specialDropdown.interactable = false;
        }

        tracksDropdown.onValueChanged.AddListener(_ => OnSelectionChanged());
        batteryDropdown.onValueChanged.AddListener(_ => OnSelectionChanged());
        cameraDropdown.onValueChanged.AddListener(_ => OnSelectionChanged());
        scannerDropdown.onValueChanged.AddListener(_ => OnSelectionChanged());
        if (isUnlocked)
            specialDropdown.onValueChanged.AddListener(_ => OnSelectionChanged());

        launchButton.onClick.AddListener(OnLaunch);

        OnSelectionChanged();
    }

    // Called by CodeEntryUI to reveal the special row at runtime
    public void ShowSpecialRow()
    {
        specialRow.SetActive(true);
        Populate(specialDropdown, specialOptions);
        specialDropdown.interactable = true;
        specialDropdown.onValueChanged.AddListener(_ => OnSelectionChanged());
        OnSelectionChanged();
    }

    void Populate(Dropdown dd, PartDefinition[] options)
    {
        dd.ClearOptions();
        dd.AddOptions(options.Select(o => o.partName).ToList());
        dd.value = 0;
    }

    void OnSelectionChanged()
    {
        config.tracks = tracksOptions[tracksDropdown.value];
        config.battery = batteryOptions[batteryDropdown.value];
        config.camera = cameraOptions[cameraDropdown.value];
        config.scanner = scannerOptions[scannerDropdown.value];

        if (specialRow.activeSelf && specialOptions.Length > 0)
            config.special = specialOptions[specialDropdown.value];
        else
            config.special = null;

        UpdateUI();
    }

    void UpdateUI()
    {
        // 1) Calculate total cost and update budget display
        int totalCost = config.tracks.cost
                      + config.battery.cost
                      + config.camera.cost
                      + config.scanner.cost
                      + (config.special?.cost ?? 0);
        budgetText.text = $"Budget: {totalCost}/{GameManager.Instance.currentBudget}";

        // 2) Sum up weight and power usage from all parts
        float totalWeight = config.tracks.weight
                          + config.battery.weight
                          + config.camera.weight
                          + config.scanner.weight
                          + (config.special?.weight ?? 0f);

        float totalPowerUsage = config.tracks.powerUsage
                              + config.battery.powerUsage
                              + config.camera.powerUsage
                              + config.scanner.powerUsage
                              + (config.special?.powerUsage ?? 0f);

        // 3) Compute and display effective speed
        //    Assumes RoverMovement.BaseSpeed is your unladen speed constant
        float effectiveSpeed = RoverMovement.baseSpeed
                             * config.tracks.speedModifier
                             / totalWeight;
        speedText.text = $"Speed: {effectiveSpeed:F1}";

        // 4) Compute and display effective battery life (in seconds)
        float effectiveBatteryLife = config.battery.capacityModifier
                                   / totalPowerUsage;
        batteryLifeText.text = $"Battery Life: {effectiveBatteryLife:F0}s";

        // 5) Budget validity and warning
        bool valid = totalCost <= GameManager.Instance.currentBudget;
        warningText.text = valid ? "" : "Over budget!";
        launchButton.interactable = valid;

        // 6) Update each component row’s cost and icon

        // Tracks
        tracksCostText.text = $"${config.tracks.cost}";
        tracksIcon.sprite = config.tracks.icon;

        // Battery
        batteryCostText.text = $"${config.battery.cost}";
        batteryIcon.sprite = config.battery.icon;

        // Camera
        cameraCostText.text = $"${config.camera.cost}";
        cameraIcon.sprite = config.camera.icon;

        // Scanner
        scannerCostText.text = $"${config.scanner.cost}";
        scannerIcon.sprite = config.scanner.icon;

        // Special (only if unlocked and selected)
        if (specialRow.activeSelf && config.special != null)
        {
            specialCostText.text = $"${config.special.cost}";
            specialIcon.sprite = config.special.icon;
        }
    }

    void OnLaunch()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.currentConfig = config;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MarsTerrain");

        GameManager.Instance.ClearRunData();    // <-- reset run‐local data
        GameManager.Instance.currentConfig = config;
        SceneManager.LoadScene("MarsTerrain");
    }
}


