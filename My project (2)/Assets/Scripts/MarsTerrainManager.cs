// MarsTerrainManager.cs
using System.Collections.Generic;
using UnityEngine;

public class MarsTerrainManager : MonoBehaviour
{
    void Start()
    {
        // Reset run data just in case
        GameManager.Instance.ClearRunData();
        var allHotspots = FindObjectsOfType<Hotspot>();
        var ids = new HashSet<string>();
        foreach (var h in allHotspots)
            ids.Add(h.resourceId);
        GameManager.Instance.totalResourceTypes = ids.Count;
    }
}

