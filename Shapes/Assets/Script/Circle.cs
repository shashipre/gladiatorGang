using UnityEngine;
using UnityEngine.InputSystem;

public class Circle : MonoBehaviour
{
    [SerializeField] float rollSpeed = 100f; // Roll speed
    [SerializeField] float airMovementSpeed = 5f; // Horizontal movement speed when in air
    [SerializeField] float jumpForce = 5f; // Jump force
    Rigidbody2D rb;
    bool isGrounded = false; // Flag to track if the circle is grounded

    void Start()
    {
        rollSpeed = rollSpeed * 10000f;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // Roll only when grounded
        if (isGrounded)
        {
            // Set angular velocity directly to achieve constant rotation speed
            rb.angularVelocity = -moveInput * rollSpeed * Time.deltaTime;
        }
        else
        {
            // Set horizontal velocity directly to achieve constant movement speed
            rb.velocity = new Vector2(moveInput * airMovementSpeed * Time.deltaTime, rb.velocity.y);
        }

        // Jumping
        if (isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !IsTouchingHorizontalGround())
        {
            Jump();
        }
    }

    void OnMove(InputValue inputValue)
    {
        // Unused for pure rolling behavior
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a platform
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true; // Set grounded flag to true
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the collision is with a platform
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false; // Set grounded flag to false
        }
    }

    bool IsTouchingHorizontalGround()
    {
        Collider2D[] colliders = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        int count = rb.GetContacts(filter, colliders);

        for (int i = 0; i < count; i++)
        {
            Vector2 normal = colliders[i].ClosestPoint(rb.position) - rb.position;
            if (Mathf.Abs(normal.y) > 0.9f) // Adjust this threshold as needed
            {
                return true;
            }
        }
        return false;
    }

    void Jump()
    {
        // Apply jump force
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        // Set grounded flag to false to prevent double jumping
        isGrounded = false;
    }
}
