using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageOverlay : MonoBehaviour
{
    public Image overlayImage;
    public float maxAlpha = 0.6f;
    public float fadeSpeed = 3f;

    Coroutine currentRoutine;

    public void ShowDamage()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(DamageFlash());
    }

    IEnumerator DamageFlash()
    {
        overlayImage.color = new Color(1, 0, 0, maxAlpha);

        while (overlayImage.color.a > 0)
        {
            float a = overlayImage.color.a - Time.deltaTime * fadeSpeed;
            overlayImage.color = new Color(1, 0, 0, a);
            yield return null;
        }
    }
}
