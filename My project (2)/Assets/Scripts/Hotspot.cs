// Hotspot.cs
using UnityEngine;

public class Hotspot : MonoBehaviour
{
    public enum HotspotType { Sample, Photo }
    public HotspotType type;
    public string resourceId;        // unique id
    public int scienceValue;
    public Sprite photoSprite;
    public string photoCaption;

    private bool inRange = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) inRange = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) inRange = false;
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    void Collect()
    {
        if (type == HotspotType.Sample)
        {
            if (!GameManager.Instance.discoveredResourceIds.Contains(resourceId))
            {
                GameManager.Instance.discoveredResourceIds.Add(resourceId);
            }
            GameManager.Instance.missionScore += scienceValue;
            GameManager.Instance.missionSamples++;
            Destroy(gameObject);
        }
        else if (type == HotspotType.Photo)
        {
            GameObject popup = Instantiate(Resources.Load<GameObject>("PhotoPopup"));
            popup.GetComponent<PhotoPopup>().Show(photoSprite, photoCaption, scienceValue);
            GameManager.Instance.missionScore += scienceValue;
            GameManager.Instance.missionPhotos++;
            Destroy(gameObject);
        }
    }
}
