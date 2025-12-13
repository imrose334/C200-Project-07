using UnityEngine;

// Add this script to any sprite that needs proper depth sorting (e.g., Player, NPC, movable objects)
public class SpriteDepthSorter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    // The frequency at which the sorting is updated (e.g., 0.1f = 10 times per second)
    public float updateInterval = 0.1f; 
    private float timer;
    
    // The sorting base is used to amplify the sorting effect, adjust as needed.
    private const int SORTING_ORDER_BASE = 5000; 

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteDepthSorter requires a SpriteRenderer component.");
            enabled = false;
        }
        
        // Initial sort immediately
        UpdateSortingOrder();
    }

    void Update()
    {
        // Use a timer to prevent updating the sort order every frame, which saves performance.
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            UpdateSortingOrder();
            timer = updateInterval;
        }
    }

    private void UpdateSortingOrder()
    {
        // The lower the Y position, the higher the sorting order (appears in front).
        // We invert the Y position and multiply it by a large number (100) to get distinct integers.
        int newOrder = (int)(SORTING_ORDER_BASE - transform.position.y * 100);
        spriteRenderer.sortingOrder = newOrder;
    }
}