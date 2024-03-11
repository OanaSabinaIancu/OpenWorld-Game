using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; set; }

    public CharacterController controller;

    public float speed = 12f;
    public float sprintSpeedMultiplier = 2f; // Multiplier for sprinting speed
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If the player is grounded and falling, reset the y-velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get input for movement in the horizontal and vertical axes
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate the movement direction based on input
        Vector3 move = transform.right * x + transform.forward * z;

        // Check if the player wants to jump and is grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Calculate the jump velocity based on the jump height and gravity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Calculate the speed multiplier based on sprinting
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetMouseButton(1) ? sprintSpeedMultiplier : 1f;

        // Move the player using the controller
        controller.Move(move * speed * speedMultiplier * Time.deltaTime);

        // Apply gravity to the player's velocity
        velocity.y += gravity * Time.deltaTime;

        // Move the player based on the velocity
        controller.Move(velocity * Time.deltaTime);
    }
}
