using UnityEngine;

public class CameraViewLock : MonoBehaviour
{
    void Start()
    {
        // Replace 5f with your camera size from the Inspector:
        Camera.main.orthographicSize = 5f;
    }
}
