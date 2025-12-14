using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    // -------------------
    // Movement / jump
    // -------------------
    public float moveSpeed = 6f;
    public float maxJumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = 0.4f;
    public float accelerationTimeAirborne = 0.2f;
    public float accelerationTimeGrounded = 0.1f;

    // Wall jump params
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float wallSlideSpeedMax = 3f;
    public float wallStickTime = 0.25f;

    // Physics / velocity
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    // Controller / input
    Controller2D controller;
    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    // -------------------
    // Abilities
    // -------------------
    [Header("Abilities")]
    public bool hasMantisClaw = false;

    // NEW: INPUT LOCK FLAG
    public bool inputLocked = false;

    private bool wallJumping = false;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter = 0f;

    // -------------------
    // Ladder settings
    // -------------------
    public float climbSpeed = 3f;
    public KeyCode climbUpKey = KeyCode.W;
    public KeyCode climbDownKey = KeyCode.S;
    private bool touchingLadder = false;
    private bool onLadder = false;
    private float defaultGravity;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        defaultGravity = gravity;
    }

    void Update()
    {
        // ============================================
        // INPUT LOCK FREEZE (hard freeze player)
        // ============================================
        if (inputLocked)
        {
            velocity = Vector3.zero;   // no sliding or gravity fall
            controller.Move(Vector3.zero, Vector2.zero); // no motion applied
            return; // stop the entire update cycle
        }
        // ============================================

        // Ladder gets first priority
        if (touchingLadder)
        {
            HandleLadderWithSeparateKeys();
        }
        else
        {
            onLadder = false;
            gravity = defaultGravity;
        }

        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        // Slope / landing adjustments
        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            else
                velocity.y = 0;
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (inputLocked) return; // No jumping while frozen
        if (onLadder) return;

        if (wallSliding && hasMantisClaw)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }

            wallJumping = true;
            wallJumpCounter = wallJumpTime;
        }
        else if (controller.collisions.below)
        {
            // Normal jump
            velocity.y = maxJumpVelocity;
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
            velocity.y = minJumpVelocity;
    }

    void HandleWallSliding()
    {
        if (onLadder) return;

        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;

        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
                velocity.y = -wallSlideSpeedMax;

            float timeToWallUnstick = wallStickTime;

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                    timeToWallUnstick -= Time.deltaTime;
            }
        }
    }

    void CalculateVelocity()
    {
        if (onLadder) return;

        float targetVelocityX = directionalInput.x * moveSpeed;

        if (wallJumping)
        {
            wallJumpCounter -= Time.deltaTime;
            if (wallJumpCounter <= 0) wallJumping = false;
        }
        else
        {
            velocity.x = Mathf.SmoothDamp(
                velocity.x,
                targetVelocityX,
                ref velocityXSmoothing,
                (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne
            );
        }

        velocity.y += gravity * Time.deltaTime;
    }

    void HandleLadderWithSeparateKeys()
    {
        bool upPressed = Input.GetKey(climbUpKey) || Input.GetKey(KeyCode.UpArrow);
        bool downPressed = Input.GetKey(climbDownKey) || Input.GetKey(KeyCode.DownArrow);

        if (upPressed || downPressed)
        {
            onLadder = true;
            gravity = 0f;
            velocity.y = upPressed ? climbSpeed : -climbSpeed;
            velocity.x = directionalInput.x * (moveSpeed * 0.5f);
        }
        else if (onLadder)
        {
            velocity.y = 0f;
            velocity.x = directionalInput.x * (moveSpeed * 0.5f);
        }
        else
        {
            gravity = defaultGravity;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
            touchingLadder = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            touchingLadder = false;
            onLadder = false;
            gravity = defaultGravity;
        }
    }

    // NEW: input lock controller method
    public void LockInput(bool state)
    {
        inputLocked = state;

        if (state)
            SetDirectionalInput(Vector2.zero);
    }
    public Vector2 GetDirectionalInput()
{
    return directionalInput;   // or whatever variable stores your movement input
}

}
