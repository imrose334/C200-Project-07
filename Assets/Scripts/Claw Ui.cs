using UnityEngine;
using TMPro;
using System.Collections;

public class ClawUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject promptUI;
    public TextMeshProUGUI abilityPopup;

    [Header("Timing / Fade")]
    public float popupDuration = 2f; // how long before pressing E is allowed
    public float fadeSpeed = 2f;

    // internal state
    private bool playerNearby = false;
    private Player player;
    private bool popupVisible = false;
    private bool canClose = false;
    private Coroutine runningFade = null;

    void Update()
    {
        // First-time pickup press
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !popupVisible)
        {
            PickupClaw();
        }

        // If popup is visible and skipping is allowed, allow closing with E
        if (popupVisible && canClose && Input.GetKeyDown(KeyCode.E))
        {
            // Start fade out if not already running
            if (runningFade == null)
                runningFade = StartCoroutine(FadeOutAndClose());
        }
    }

    private void PickupClaw()
    {
        if (player != null)
        {
            player.hasMantisClaw = true;
            player.LockInput(true); // freeze player input while popup shown
        }

        if (promptUI != null) promptUI.SetActive(false);

        // Show and fade in popup
        if (abilityPopup != null)
        {
            // Ensure popup is enabled
            abilityPopup.gameObject.SetActive(true);

            // Stop any previous coroutine
            if (runningFade != null) StopCoroutine(runningFade);

            runningFade = StartCoroutine(FadeInAndEnableClose());
        }
        else
        {
            // If no popup assigned, immediately restore control and destroy pickup
            if (player != null) player.LockInput(false);
            Destroy(gameObject);
        }

        popupVisible = true;
        canClose = false;
    }

    private IEnumerator FadeInAndEnableClose()
    {
        // Set alpha to 0 to start
        Color c = abilityPopup.color;
        c.a = 0f;
        abilityPopup.color = c;

        // Fade IN to alpha 1
        while (abilityPopup.color.a < 1f)
        {
            c = abilityPopup.color;
            c.a += Time.deltaTime * fadeSpeed;
            c.a = Mathf.Clamp01(c.a);
            abilityPopup.color = c;
            yield return null;
        }

        // Ensure fully opaque
        c = abilityPopup.color;
        c.a = 1f;
        abilityPopup.color = c;

        // Wait the required display delay, then allow skipping
        yield return new WaitForSeconds(popupDuration);
        canClose = true;
        runningFade = null;
    }

    private IEnumerator FadeOutAndClose()
    {
        canClose = false;

        // Fade OUT to alpha 0
        while (abilityPopup != null && abilityPopup.color.a > 0f)
        {
            Color c = abilityPopup.color;
            c.a -= Time.deltaTime * fadeSpeed;
            c.a = Mathf.Clamp01(c.a);
            abilityPopup.color = c;
            yield return null;
        }

        // Ensure hidden
        if (abilityPopup != null)
        {
            Color c = abilityPopup.color;
            c.a = 0f;
            abilityPopup.color = c;
            abilityPopup.gameObject.SetActive(false);
        }

        // Restore player control
        if (player != null)
            player.LockInput(false);

        popupVisible = false;
        runningFade = null;

        // Finally destroy the pickup object (now that UI and lock are done)
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            player = collision.GetComponent<Player>();
            if (promptUI != null && !popupVisible) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            if (promptUI != null && !popupVisible) promptUI.SetActive(false);
        }
    }
}
