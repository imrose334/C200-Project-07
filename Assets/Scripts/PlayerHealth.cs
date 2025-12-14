using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int CurrentHealth
{
    get { return currentHealth; }
}

    
    [Header("Health")]
    
    public int maxHealth = 100;
    public float hitInvincibilityTime = 0.5f;

    [Header("UI")]
    public Image healthBar;           // Assign the foreground health image here
    public float smoothSpeed = 5f;    // How fast the bar updates
    private float targetFill;

    [Header("Respawn")]
    public float respawnDelay = 1.5f;
    private Vector3 respawnPosition = new Vector3(3.08999991f, 4.90999985f, 0.874539495f);

    private int currentHealth;
    private float lastHitTime;

    private Rigidbody2D rb;
    private Collider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        ResetPlayer();
    }

    void Update()
    {
        // Smoothly update health bar
        if (healthBar != null)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFill, Time.deltaTime * smoothSpeed);
        }
    }

    public void TakeHit(int damage)
    {
        if (Time.time < lastHitTime + hitInvincibilityTime)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetFill = (float)currentHealth / maxHealth;

        lastHitTime = Time.time;

        if (currentHealth <= 0)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    IEnumerator RespawnRoutine()
    {
        // Disable physics + collisions (NOT the GameObject)
        rb.velocity = Vector2.zero;
        rb.simulated = false;
        col.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        ResetPlayer();

        rb.simulated = true;
        col.enabled = true;
    }

    void ResetPlayer()
    {
        currentHealth = maxHealth;
        targetFill = 1f;

        if (healthBar != null)
            healthBar.fillAmount = 1f;

        transform.position = respawnPosition;
    }
}
