// RoverBuilderUI.cs
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RoverBuilderUI : MonoBehaviour
{
    public PartDefinition[] tracksOptions;
    public PartDefinition[] batteryOptions;
    public PartDefinition[] cameraOptions;
    public PartDefinition[] scannerOptions;
    public PartDefinition[] specialOptions;

    public Dropdown tracksDropdown;
    public Dropdown batteryDropdown;
    public Dropdown cameraDropdown;
    public Dropdown scannerDropdown;
    public Dropdown specialDropdown;

    public Text budgetText;
    public Text speedText;
    public Text batteryLifeText;

    public Button launchButton;
    public Text warningText;

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

        if (budgetText == null) Debug.LogError("RoverBuilderUI.budgetText is missing!");
        if (speedText == null) Debug.LogError("RoverBuilderUI.speedText is missing!");
        if (batteryLifeText == null) Debug.LogError("RoverBuilderUI.batteryLifeText is missing!");
        if (warningText == null) Debug.LogError("RoverBuilderUI.warningText is missing!");
        if (launchButton == null) Debug.LogError("RoverBuilderUI.launchButton is missing!");

        int totalCost = config.tracks.cost + config.battery.cost + config.camera.cost + config.scanner.cost + (config.special != null ? config.special.cost : 0);
        budgetText.text = $"Budget: {totalCost}/{GameManager.Instance.currentBudget}";
        float speed = config.tracks.speedModifier;
        float capacity = config.battery.capacityModifier;
        speedText.text = $"Speed: {speed:F1}";
        batteryLifeText.text = $"Battery Life: {capacity:F0}";

        var trackRow = tracksDropdown.GetComponentInParent<Transform>();
        trackRow.Find("CostText").GetComponent<Text>().text = $"${config.tracks.cost}";
        trackRow.Find("IconPreview").GetComponent<Image>().sprite = config.tracks.icon;

        var batteryRow = tracksDropdown.GetComponentInParent<Transform>();
        batteryRow.Find("CostText").GetComponent<Text>().text = $"${config.battery.cost}";
        batteryRow.Find("IconPreview").GetComponent<Image>().sprite = config.battery.icon;

        var cameraRow = tracksDropdown.GetComponentInParent<Transform>();
        cameraRow.Find("CostText").GetComponent<Text>().text = $"${config.camera.cost}";
        cameraRow.Find("IconPreview").GetComponent<Image>().sprite = config.camera.icon;

        var specialRow = tracksDropdown.GetComponentInParent<Transform>();
        specialRow.Find("CostText").GetComponent<Text>().text = $"${config.special.cost}";
        specialRow.Find("IconPreview").GetComponent<Image>().sprite = config.special.icon;

        var scannerRow = tracksDropdown.GetComponentInParent<Transform>();
        scannerRow.Find("CostText").GetComponent<Text>().text = $"${config.scanner.cost}";
        scannerRow.Find("IconPreview").GetComponent<Image>().sprite = config.scanner.icon;


        bool valid = totalCost <= GameManager.Instance.currentBudget;
        launchButton.interactable = valid;
        warningText.text = valid ? "" : "Over budget!";
    }

    void OnLaunch()
    {
        GameManager.Instance.currentConfig = config;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MarsTerrain");
    }
}