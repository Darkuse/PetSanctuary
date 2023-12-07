using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed = 3f;
    private Joystick joystick;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joystick = FindObjectOfType<Joystick>();
    }

    private void Update()
    {
        // Get input for movement from the joystick
        float horizontal = joystick.GetDirection().x;
        float vertical = joystick.GetDirection().y;

        // Calculate movement direction
        Vector2 movement = new Vector2(horizontal, vertical);

        // Normalize the movement vector to prevent diagonal speed boost
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Move the character using Rigidbody
        rb.velocity = movement * moveSpeed;
    }
}
