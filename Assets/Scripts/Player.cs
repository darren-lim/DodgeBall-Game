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
    public bool isGrounded;
    public Rigidbody2D rb;
    public bool facingRight = true;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    [SerializeField]
    private float thrustBall = 10f;
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
        HandleThrowBall();
    }

    // Move player and jump
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

    // Face the sprite towards the mouse horizontal direction
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

    // On mouse click, throw ball
    void HandleThrowBall()
    {
        if (Input.GetMouseButton(0))
        {
            thrustBall += 1f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            // turn off sprite ball on hand
            print(thrustBall);
            // turn on player ball object
            print("Throw");
            // add thrust and throw the ball
        }
    }

    public bool IsGrounded()
    {
        if(Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            isGrounded = true;
            return true;
        } else
        {
            isGrounded = false;
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
