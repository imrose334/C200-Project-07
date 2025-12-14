using UnityEngine;

public class ToggleCanvasOnTrigger : MonoBehaviour
{
    [Tooltip("The entire Canvas to enable/disable")]
    public GameObject popupCanvas;

    private void Start()
    {
        // Safety: ensure canvas starts off
        if (popupCanvas != null)
            popupCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TeleportZone"))
        {
            popupCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TeleportZone"))
        {
            popupCanvas.SetActive(false);
        }
    }
}
