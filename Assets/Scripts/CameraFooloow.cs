using UnityEngine;

public class CameraFoolloow : MonoBehaviour
{
    public Controller2D target;
    public float verticalOffset = 1f;
    public float lookAheadDstX = 2f;
    public float lookSmoothTimeX = 0.2f;
    public float verticalSmoothTime = 0.2f;

    [Header("Vertical Look Settings")]
    public float lookUpDistance = 2f;
    public float lookDownDistance = 3f;
    public float lookVerticalSmoothTime = 0.2f;

    private FocusArea focusArea;
    private float currentLookAheadX;
    private float targetLookAheadX;
    private float lookAheadDirX;
    private float smoothLookVelocityX;
    private float smoothVelocityY;

    private float targetVerticalLookOffset = 0f;
    private float currentVerticalLookOffset = 0f;
    private float verticalLookVelocity;
    private bool lookAheadStopped;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFoolloow target is not assigned!");
            enabled = false;
            return;
        }

        focusArea = new FocusArea(target.collider.bounds, new Vector2(1, 1)); // adjust size as needed
    }

    void LateUpdate()
    {
        focusArea.Update(target.collider.bounds);

        // Vertical input for looking up/down
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) targetVerticalLookOffset = -lookDownDistance;
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) targetVerticalLookOffset = lookUpDistance;
        else targetVerticalLookOffset = 0f;

        currentVerticalLookOffset = Mathf.SmoothDamp(currentVerticalLookOffset, targetVerticalLookOffset, ref verticalLookVelocity, lookVerticalSmoothTime);

        // Horizontal look ahead
        if (focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            Vector2 playerInput = target.playerInput;

            if (Mathf.Sign(playerInput.x) == Mathf.Sign(focusArea.velocity.x) && playerInput.x != 0)
            {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * lookAheadDstX;
            }
            else if (!lookAheadStopped)
            {
                lookAheadStopped = true;
                targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4f;
            }
        }

        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

        // Calculate target position using focus center
        Vector2 focusPosition = focusArea.centre + Vector2.up * (verticalOffset + currentVerticalLookOffset);
        focusPosition += Vector2.right * currentLookAheadX;
        Vector3 targetPosition = new Vector3(focusPosition.x, focusPosition.y, -10f);

        // SmoothDamp ensures zero jitter even with floating-point rounding
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.05f);
    }

    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        private float left, right, top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;
            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0f;
            if (targetBounds.min.x < left) shiftX = targetBounds.min.x - left;
            else if (targetBounds.max.x > right) shiftX = targetBounds.max.x - right;
            left += shiftX; right += shiftX;

            float shiftY = 0f;
            if (targetBounds.min.y < bottom) shiftY = targetBounds.min.y - bottom;
            else if (targetBounds.max.y > top) shiftY = targetBounds.max.y - top;
            top += shiftY; bottom += shiftY;

            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
