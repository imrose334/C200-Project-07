using UnityEngine;

public class StartupDisplayFix : MonoBehaviour
{
    void Awake()
    {
        Screen.fullScreen = false;
        Screen.SetResolution(1920, 1200, false);
    }
}
