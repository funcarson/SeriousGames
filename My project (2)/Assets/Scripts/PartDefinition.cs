// PartDefinition.cs
using UnityEngine;

[CreateAssetMenu(menuName = "Rover/PartDefinition")]
public class PartDefinition : ScriptableObject
{
    public string partName;
    public Sprite icon;
    public int cost;
    public float weight;
    public float powerUsage;

    // Specialized stats:
    public float speedModifier;      // for tracks
    public float capacityModifier;   // for battery (max battery units)
    public int sciencePerPhoto;      // for camera
    public float scannerRange;       // for scanner
}
