using UnityEngine;
using UnityEngine.UI;

public class SignInteraction : MonoBehaviour
{
    public GameObject promptUI;   // small "Press E to interact"
    public GameObject messageUI;  // big message panel
    public string messageText = "This is the sign message.";

    private bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            promptUI.SetActive(false);   // hide the prompt
            messageUI.SetActive(true);   // show the big message
            messageUI.GetComponent<Text>().text = messageText;
        }
        else if (playerNearby && Input.GetKeyDown(KeyCode.Escape))
        {
            messageUI.SetActive(false);  // hide message if pressing Escape
            promptUI.SetActive(true);    // show prompt again
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            promptUI.SetActive(false);
            messageUI.SetActive(false);
        }
    }
}
