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
        if (isAttacking)
            return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        isAttacking = true;
        lastAttackTime = Time.time;

        animator.SetBool("attack", true);
    }

    // 🔥 CALLED FROM ANIMATION EVENT (THIS IS CRITICAL)
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
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    // 🔥 CALLED FROM ANIMATION EVENT (END OF ANIM)
    public void EndAttack()
    {
        animator.SetBool("attack", false);
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
