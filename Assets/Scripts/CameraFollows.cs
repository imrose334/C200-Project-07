using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target & Speed")]
    [Tooltip("The Transform of the player or object the camera should follow.")]
    public Transform target;
    
    [Tooltip("How quickly the camera catches up to the target. Higher value is faster.")]
    // NOTE: This value is now a speed, not a smoothing factor (due to MoveTowards).
    public float followSpeed = 10f; 

    [Header("Offset")]
    [Tooltip("The desired offset of the camera from the target.")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    
    // LateUpdate runs after all Update and FixedUpdate functions.
    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow target is not set! Please assign the Player's Transform.");
            return;
        }

        // 1. Calculate the desired position
        // We use the target's X and Y, and maintain the camera's current Z depth.
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x, 
            target.position.y + offset.y, 
            transform.position.z // Keep the existing Z depth
        ); 

        // 2. Move the camera directly towards the desired position
        // Using MoveTowards ensures a guaranteed movement, which can feel snappier/smoother.
        transform.position = Vector3.MoveTowards(
            transform.position, 
            desiredPosition, 
            followSpeed * Time.deltaTime
        );
    }
}