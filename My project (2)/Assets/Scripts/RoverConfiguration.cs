// GameManager.cs
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RoverConfiguration
{
    public PartDefinition tracks;
    public PartDefinition battery;
    public PartDefinition camera;
    public PartDefinition scanner;
    public PartDefinition special;   // e.g., solar panel
}

