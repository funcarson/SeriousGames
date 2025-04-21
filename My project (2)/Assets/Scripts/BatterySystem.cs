// BatterySystem.cs
using UnityEngine;
using UnityEngine.UI;

public class BatterySystem : MonoBehaviour
{
    public Image batteryBar;
    private float maxBattery;
    private float currentBattery;

    void Start()
    {
        maxBattery = GameManager.Instance.currentConfig.battery.capacityModifier;
        currentBattery = maxBattery;
    }

    void Update()
    {
        float drain = Time.deltaTime;
        // Extra drain when moving
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.velocity.magnitude > 0.1f) drain *= 1.5f;

        currentBattery = Mathf.Max(0, currentBattery - drain);
        batteryBar.fillAmount = currentBattery / maxBattery;

        if (currentBattery <= 0)
        {
            // End mission
            GameManager.Instance.EndMission();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MissionSummary");
        }
    }
}