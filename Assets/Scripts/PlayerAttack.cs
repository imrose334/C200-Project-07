using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    public int attackDamage = 40;
    public float attackRange = 1.2f;
    public float attackCooldown = 0.6f;
    public LayerMask enemyLayer;

    private Animator animator;
    private float lastAttackTime;
    private bool isAttacking;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        if (isAttacking)
            return;

        isAttacking = true;
        lastAttackTime = Time.time;

        animator.ResetTrigger("attack"); // safety
        animator.SetTrigger("attack");

        // 🔥 FAILSAFE — guarantees unlock even if animation event fails
        Invoke(nameof(ForceEndAttack), attackCooldown);
    }

    // 🔥 HIT FRAME (ANIMATION EVENT)
    public void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("vamp"))
                continue;

            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(attackDamage);
        }
    }

    // 🔥 END FRAME (ANIMATION EVENT)
    public void EndAttack()
    {
        isAttacking = false;
        CancelInvoke(nameof(ForceEndAttack));
    }

    // 🔥 ABSOLUTE FAILSAFE
    void ForceEndAttack()
    {
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
