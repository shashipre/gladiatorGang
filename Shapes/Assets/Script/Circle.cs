using UnityEngine;
using UnityEngine.InputSystem;

public class Circle : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float jumpForce = 5f; // Jump force
    Rigidbody2D rb;
    Vector2 moveInput;
    bool isGrounded = false; // Flag to track if the circle is grounded

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movement
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);

        // Rotation
        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }

        // Jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
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

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
