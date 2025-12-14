using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target & Speed")]
    public Transform target;
    public float followSpeed = 10f;

    [Header("Offset")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow target is not set! Please assign the Player's Transform.");
            return;
        }

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
    }
}
