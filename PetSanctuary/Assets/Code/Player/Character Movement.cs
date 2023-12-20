using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed = 3f;
    private Joystick joystick;
    private Quaternion targetRotation;
    public float rotationSpeed = 5f;
    private Animator anim;

    private bool right = false;
    private bool left = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joystick = FindObjectOfType<Joystick>();
        targetRotation = transform.rotation;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Get input for movement from the joystick
        float horizontal = joystick.GetDirection().x;
        float vertical = joystick.GetDirection().y;


        // Calculate movement direction
        Vector2 movement = new Vector2(horizontal, vertical);

        if (movement == new Vector2(0, 0))
            anim.SetBool("Walking", false);
        else
            anim.SetBool("Walking", true);

        // Normalize the movement vector to prevent diagonal speed boost
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }


        if (horizontal < 0 && !left)
        {
            anim.SetTrigger("RotationLeft");
            anim.ResetTrigger("RotationRight");
            left = true;
            right = false;
        }
        else if (horizontal > 0 && !right)
        {
            anim.SetTrigger("RotationRight");
            anim.ResetTrigger("RotationLeft");
            left = false;
            right = true;
        }

        // Move the character using Rigidbody
        rb.velocity = movement * moveSpeed;

    }
}
