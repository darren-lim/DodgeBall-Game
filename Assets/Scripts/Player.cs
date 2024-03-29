using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Move player in 2D space
    public float playerSpeed = 3.4f;
    public float jumpHeight = 6.5f;
    public float gravityValue = -9.81f;
    //public bool isGrounded;
    public Rigidbody2D rb;
    public bool facingRight = true;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    Transform t;

    private Vector2 worldPosition;
    private Vector2 direction;
    private float angle;
    private SpriteRenderer sRenderer;

    void Start()
    {
        //Get the rigidbody2d of the gameObject this script is assigned to.
        rb = GetComponent<Rigidbody2D>();
        t = transform;
        sRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovement();
        HandleFacingDirection();

        //// facing direction based on movement
        //if (rb.velocity.x > 0 && !facingRight)
        //{
        //    facingRight = true;
        //    t.localScale = new Vector2(Mathf.Abs(t.localScale.x), t.localScale.y);
        //}
        //else if (rb.velocity.x < 0 && facingRight)
        //{
        //    facingRight = false;
        //    t.localScale = new Vector2(-Mathf.Abs(t.localScale.x), t.localScale.y);
        //}
    }

    void HandleMovement()
    {
        //Determine the direction of the movement based on user input.
        float moveDir = Input.GetAxis("Horizontal");

        //Calculate the velocity of the gameObject.
        rb.velocity = new Vector2(moveDir * playerSpeed, rb.velocity.y);

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }

    void HandleFacingDirection()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosition - (Vector2)t.position).normalized;

        // flip the arm when reaches 90 degrees
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector3 localScale = new Vector3(1f, 1f, 1f);
        if (angle > 90 || angle < -90)
        {
            facingRight = false;
            sRenderer.flipX = true;
        }
        else
        {
            facingRight = true;
            sRenderer.flipX = false;
        }
    }

    public bool IsGrounded()
    {
        if(Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
