using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VampireAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float detectionRange = 6f;
    public float attackRange = 1.2f;

    [Header("Combat")]
    public int damage = 10;
    public float attackCooldown = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    private float lastAttackTime;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("guy");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null)
        {
            SetIdle();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            SetAttack();
        }
        else if (distance <= detectionRange)
        {
            SetRun();
            movement = (player.position - transform.position).normalized;
        }
        else
        {
            SetIdle();
            movement = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }

    void SetRun()
    {
        animator.SetBool("run", true);
        animator.SetBool("attack", false);
        animator.SetBool("idle", false);
    }

    void SetAttack()
    {
        rb.velocity = Vector2.zero;

        animator.SetBool("run", false);
        animator.SetBool("attack", true);
        animator.SetBool("idle", false);
    }

    void SetIdle()
    {
        rb.velocity = Vector2.zero;

        animator.SetBool("run", false);
        animator.SetBool("attack", false);
        animator.SetBool("idle", true);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("guy"))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeHit(damage);
                    lastAttackTime = Time.time;
                }
            }
        }

        if (collision.gameObject.CompareTag("wall"))
        {
            movement = Vector2.zero;
        }
    }
}
