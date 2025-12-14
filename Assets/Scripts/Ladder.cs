using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LadderController : MonoBehaviour
{
    [Header("Climbing Settings")]
    public float climbSpeed = 3f;
    public string ladderTag = "Ladder"; // tag for ladders
    public bool snapToLadderX = true;   // should player align with ladder X position?

    private Rigidbody2D rb;
    private bool isTouchingLadder = false;
    private float defaultGravity;
    private float ladderXPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        if (isTouchingLadder)
        {
            HandleClimbing();
        }
        else
        {
            // Restore normal gravity when not on ladder
            rb.gravityScale = defaultGravity;
        }
    }

    void HandleClimbing()
    {
        float verticalVelocity = 0f;

        // Climb up/down
        if (Input.GetKey(KeyCode.R))
            verticalVelocity = climbSpeed;
        else if (Input.GetKey(KeyCode.F))
            verticalVelocity = -climbSpeed;

        // Optional: snap player X to ladder
        float xPos = rb.velocity.x;
        if (snapToLadderX)
            xPos = ladderXPosition;

        // Set new velocity
        rb.velocity = new Vector2(xPos, verticalVelocity);

        // Disable gravity while climbing
        rb.gravityScale = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ladderTag))
        {
            isTouchingLadder = true;

            // Store ladder X position for snapping
            if (snapToLadderX)
                ladderXPosition = other.transform.position.x;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(ladderTag))
        {
            isTouchingLadder = false;

            // Restore gravity
            rb.gravityScale = defaultGravity;
        }
    }
}
