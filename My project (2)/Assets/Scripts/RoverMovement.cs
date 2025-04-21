using UnityEngine;

public class RoverMovement : MonoBehaviour
{
    public float baseSpeed = 5f;
    private Rigidbody2D rb;
    private TouchJoystick joystick;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joystick = FindObjectOfType<TouchJoystick>();
    }

    void Update()
    {
        Vector2 kb = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 js = joystick != null ? joystick.Direction : Vector2.zero;
        Vector2 move = (kb + js).normalized;

        float speedMod = GameManager.Instance.currentConfig.tracks.speedModifier;
        rb.velocity = move * baseSpeed * speedMod;
    }
}
