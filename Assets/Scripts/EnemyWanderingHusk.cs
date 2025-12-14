using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyWanderingHusk : MonoBehaviour
{
    [Header("Speeds")]
    public float walkSpeed = 1.2f;
    public float runSpeed = 4f;

    [Header("Burst Timing")]
    public float runDuration = 0.45f;
    public float pauseAfterRun = 0.35f;

    [Header("Player Detection")]
    public bool useTagToFindPlayer = true;
    public string playerTag = "Player";
    public LayerMask playerLayer;
    public float detectionRange = 6f;

    [Header("Environment Checks")]
    public LayerMask groundLayer;
    public float wallCheckDistance = 0.45f;
    public float ledgeCheckDistance = 0.9f;

    [Header("Animator")]
    public Animator animator; // assign in inspector

    // Runtime
    private Rigidbody2D rb;
    private Collider2D col;
    private Transform target;

    private bool chasing = false;
    private bool inRun = false;
    private bool pausing = false;
    private int moveDir = 1;

    private Coroutine burstCoroutine = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (useTagToFindPlayer)
        {
            GameObject p = GameObject.FindGameObjectWithTag(playerTag);
            if (p != null) target = p.transform;
        }
    }

    void Update()
    {
        // Dynamic player detection if using layer
        if (!useTagToFindPlayer)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
            target = hit != null ? hit.transform : null;
        }

        if (target == null)
        {
            chasing = false;
            return;
        }

        float dist = Vector2.Distance(transform.position, target.position);

        if (!chasing && dist <= detectionRange)
        {
            chasing = true;
        }
        else if (chasing && dist > detectionRange + 1.5f)
        {
            chasing = false;
            if (burstCoroutine != null)
            {
                StopCoroutine(burstCoroutine);
                burstCoroutine = null;
                inRun = false;
                pausing = false;
            }
        }

        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (!chasing || target == null)
        {
            // Idle: stop moving
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        CheckEnvironment();

        // Face player
        moveDir = (target.position.x > transform.position.x) ? 1 : -1;
        transform.localScale = new Vector3(moveDir, 1f, 1f);

        if (inRun) return; // run handled in coroutine
        if (pausing)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        // Walk slowly toward player
        rb.velocity = new Vector2(moveDir * walkSpeed, rb.velocity.y);

        // Start burst coroutine if not active
        if (burstCoroutine == null)
        {
            burstCoroutine = StartCoroutine(BurstCycle());
        }
    }

    IEnumerator BurstCycle()
    {
        // Walk a short random interval before lunging
        float walkTime = Random.Range(0.6f, 1.2f);
        float timer = 0f;
        while (timer < walkTime)
        {
            if (!chasing || target == null)
            {
                burstCoroutine = null;
                yield break;
            }
            if (!inRun && !pausing)
                rb.velocity = new Vector2(moveDir * walkSpeed, rb.velocity.y);

            timer += Time.deltaTime;
            yield return null;
        }

        // Begin RUN burst
        inRun = true;
        pausing = false;

        moveDir = (target.position.x > transform.position.x) ? 1 : -1;
        transform.localScale = new Vector3(moveDir, 1f, 1f);

        rb.velocity = new Vector2(moveDir * runSpeed, rb.velocity.y);

        float t = 0f;
        while (t < runDuration)
        {
            if (!chasing || target == null) break;
            t += Time.deltaTime;
            yield return null;
        }

        // End Run
        inRun = false;
        rb.velocity = new Vector2(0f, rb.velocity.y);

        // Pause
        pausing = true;
        yield return new WaitForSeconds(pauseAfterRun);
        pausing = false;

        burstCoroutine = null;
    }

    void CheckEnvironment()
    {
        if (col == null) return;

        Vector2 origin = col.bounds.center;
        Vector2 wallOrigin = origin;
        Vector2 ledgeOrigin = new Vector2(origin.x + moveDir * (col.bounds.extents.x + 0.05f), col.bounds.min.y + 0.05f);

        RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, Vector2.right * moveDir, wallCheckDistance, groundLayer);
        if (wallHit.collider != null) { Flip(); return; }

        RaycastHit2D ledgeHit = Physics2D.Raycast(ledgeOrigin, Vector2.down, ledgeCheckDistance, groundLayer);
        if (ledgeHit.collider == null) { Flip(); return; }
    }

    void Flip()
    {
        moveDir *= -1;
        transform.localScale = new Vector3(moveDir, 1f, 1f);
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        bool isWalking = !inRun && !pausing && Mathf.Abs(rb.velocity.x) > 0.01f;
        bool isLunging = inRun;

        animator.SetBool("robotwalk", isWalking);
        animator.SetBool("robotlunge", isLunging);
        // idle will automatically play when both booleans false
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (col != null)
        {
            Gizmos.color = Color.cyan;
            Vector2 origin = col.bounds.center;
            Vector2 ledgeOrigin = new Vector2(origin.x + moveDir * (col.bounds.extents.x + 0.05f), col.bounds.min.y + 0.05f);
            Gizmos.DrawLine(origin, origin + Vector2.right * moveDir * wallCheckDistance);
            Gizmos.DrawLine(ledgeOrigin, ledgeOrigin + Vector2.down * ledgeCheckDistance);
        }
    }
}
