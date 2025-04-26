using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class CameraResetOnLoad : MonoBehaviour
{
    // Tweak these in the Inspector
    public string targetSceneName = "MarsTerrain";
    public float orthoSize = 8f;
    public Vector3 cameraPosition = new Vector3(0f, 0f, -10f);

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        // Listen for scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetSceneName)
        {
            cam.orthographic = true;
            cam.orthographicSize = orthoSize;
            transform.position = cameraPosition;
        }
    }
}