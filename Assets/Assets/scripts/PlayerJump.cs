using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float groundCheckRadius = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump when the player presses the jump button (e.g., Spacebar)
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void Jump()
    {
        // Apply an upward force to make the player jump
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
