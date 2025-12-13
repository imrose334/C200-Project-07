using UnityEngine;

// Ensure Rigidbody2D is present for physics
[RequireComponent(typeof(Rigidbody2D))] 
public class TopDownCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The base speed of the character.")]
    public float baseMoveSpeed = 5f;
    [Tooltip("Speed multiplier applied when entering 'Grass' zones.")]
    public float grassSpeedMultiplier = 0.5f;

    // --- Core Variables ---
    private float currentMoveSpeed;
    private Rigidbody2D rb;
    private Vector2 rawInputDirection; 
    
    // State variables for interactions
    private bool isOverGrass = false;
    private bool isNearHouse = false;  // <--- THIS LINE IS NOW CORRECT
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        // 1. INPUT READING (Read the keys every frame)
        
        float moveX = 0f;
        float moveY = 0f;
        
        if (Input.GetKey(KeyCode.A)) { moveX = -1f; }
        else if (Input.GetKey(KeyCode.D)) { moveX = 1f; }

        if (Input.GetKey(KeyCode.S)) { moveY = -1f; } 
        else if (Input.GetKey(KeyCode.W)) { moveY = 1f; }
        
        rawInputDirection = new Vector2(moveX, moveY).normalized;
        
        // --- Interaction Input (E key) ---
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interaction key pressed (E).");
        }
    }

    // FixedUpdate is for physics calculations
    void FixedUpdate()
    {
        // 2. MOVEMENT EXECUTION
        rb.velocity = rawInputDirection * currentMoveSpeed;
    }
    
    // --- Interaction / Trigger Logic ---
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grass"))
        {
            isOverGrass = true;
            currentMoveSpeed = baseMoveSpeed * grassSpeedMultiplier; 
            Debug.Log("Entered Grass. Speed reduced.");
        }
        
        if (other.CompareTag("House"))
        {
            isNearHouse = true;
            Debug.Log("Near House. Press E to enter/interact.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Grass"))
        {
            isOverGrass = false;
            currentMoveSpeed = baseMoveSpeed;
            Debug.Log("Exited Grass. Speed restored.");
        }
        
        if (other.CompareTag("House"))
        {
            isNearHouse = false;
            Debug.Log("Away from House.");
        }
    }
}