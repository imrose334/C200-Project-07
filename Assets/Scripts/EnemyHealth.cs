using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 25;  
    private int currentHealth;
    private bool dead;

    [Header("UI")]
    public Image healthBar;             // Assign the foreground image here
    public float smoothSpeed = 5f;      // How fast the bar updates
    private float targetFill;

    void Awake()
    {
        currentHealth = maxHealth;
        targetFill = 1f;
        if (healthBar != null)
            healthBar.fillAmount = 1f;
    }

    void Update()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFill, Time.deltaTime * smoothSpeed);
        }
    }

    public void TakeDamage(int damage)
    {
        if (dead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetFill = (float)currentHealth / maxHealth;

        Debug.Log($"{name} HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        dead = true;

        // Disable AI + physics
        VampireAI ai = GetComponent<VampireAI>();
        if (ai) ai.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = false;

        Destroy(gameObject, 0.2f);
    }
}
