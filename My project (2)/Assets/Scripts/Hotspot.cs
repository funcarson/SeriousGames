// Hotspot.cs
using UnityEngine;

public class Hotspot : MonoBehaviour
{
    public enum HotspotType { Sample, Photo }

    [Header("Hotspot Settings")]
    public HotspotType type;
    public string resourceId;        // unique ID for samples
    public int scienceValue;

    [Header("Photo Settings")]
    public Sprite photoSprite;
    [TextArea] public string photoCaption;
    public GameObject photoPopupPrefab;

    [Header("Visual Indicator")]
    public GameObject indicator;     // your radar indicator object

    [Header("Audio Clips")]
    public AudioClip detectionClip;  // radar ping
    public AudioClip pictureClip;    // camera shutter

    private Transform roverTransform;
    private AudioSource audioSource;
    private float scannerRange => GameManager.Instance.currentConfig.scanner.scannerRange;

    // Tracks whether the indicator was active last frame
    private bool indicatorWasActive = false;

    private bool inRange = false;

    void Start()
    {
        // Find the Rover
        var rover = GameObject.FindGameObjectWithTag("Player");
        if (rover != null) roverTransform = rover.transform;

        // Setup AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Hide the indicator initially
        if (indicator != null)
            indicator.SetActive(false);
    }

    void Update()
    {
        if (roverTransform != null && indicator != null)
        {
            float dist = Vector2.Distance(roverTransform.position, transform.position);
            bool nowActive = dist <= scannerRange;

            // When the indicator *becomes* active, play radar ping
            if (nowActive && !indicatorWasActive && detectionClip != null)
                audioSource.PlayOneShot(detectionClip);

            indicator.SetActive(nowActive);
            indicatorWasActive = nowActive;
        }

        // Collection input
        if (inRange && Input.GetKeyDown(KeyCode.E))
            Collect();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            inRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            inRange = false;
    }

    void Collect()
    {
        // 1) Register this hotspot's ID so it's counted toward the win condition
        if (!GameManager.Instance.discoveredResourceIds.Contains(resourceId))
            GameManager.Instance.discoveredResourceIds.Add(resourceId);

        // 2) Award science points and increment the appropriate counter
        GameManager.Instance.missionScore += scienceValue;
        if (type == HotspotType.Sample)
        {
            GameManager.Instance.missionSamples++;
        }
        else if (type == HotspotType.Photo)
        {
            // 1) Register the resource
            if (!GameManager.Instance.discoveredResourceIds.Contains(resourceId))
                GameManager.Instance.discoveredResourceIds.Add(resourceId);

            // 2) Determine science to award from the current camera config
            var camDef = GameManager.Instance.currentConfig.camera;
            int scienceGain = camDef.sciencePerPhoto;
            GameManager.Instance.missionScore += scienceGain;
            GameManager.Instance.missionPhotos += 1;

            // 3) Play the shutter sound at the camera
            if (pictureClip != null && Camera.main != null)
                AudioSource.PlayClipAtPoint(pictureClip, Camera.main.transform.position);

            // 4) Show the photo popup
            if (photoPopupPrefab != null)
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    var popupGO = Instantiate(photoPopupPrefab, canvas.transform);
                    var popup = popupGO.GetComponent<PhotoPopup>();
                    if (popup != null)
                        popup.Show(photoSprite, photoCaption, scienceGain);
                }
            }

            // 5) Immediately check for a full‐clear win
            GameManager.Instance.CheckForWin();

            // 6) Destroy yourself
            Destroy(gameObject);
        }
    }
}


