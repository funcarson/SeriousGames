using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Tooltip("Tag of the object to follow (your Rover should be tagged 'Player').")]
    public string targetTag = "Player";

    [Tooltip("How quickly the camera catches up. Smaller = tighter follow.")]
    public float smoothTime = 0.15f;

    [Tooltip("Offset from the target's position (usually (0,0,-10) for 2D).")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private Transform target;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        var go = GameObject.FindGameObjectWithTag(targetTag);
        if (go != null)
            target = go.transform;
        else
            Debug.LogError($"CameraFollow: No GameObject found with tag '{targetTag}'.");
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Desired camera position based on target + offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );
    }
}


