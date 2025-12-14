using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Kinematic; // IMPORTANT for trigger-based movement
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
    }

    void FixedUpdate()
    {
        Vector2 moveDelta = input * moveSpeed * Time.fixedDeltaTime;
        Vector2 targetPos = rb.position + moveDelta;

        // Check if moving would overlap a Collision trigger
        Collider2D hit = Physics2D.OverlapBox(
            targetPos,
            col.bounds.size,
            0f,
            LayerMask.GetMask("Default")
        );

        if (hit != null && hit.CompareTag("Collision"))
        {
            // BLOCK movement
            return;
        }

        rb.MovePosition(targetPos);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collision"))
        {
            Debug.Log("Entered Collision trigger");
        }
    }
}
