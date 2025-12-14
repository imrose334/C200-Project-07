using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPTextEffects : MonoBehaviour
{
    [Header("Effect Settings")]
    public float shakeAmount = 5f;
    public float scaleAmount = 0.2f;
    public float rotationAmount = 15f;
    public float effectSpeed = 0.05f;
    public bool randomColors = true;
    public bool shake = true;
    public bool scale = true;
    public bool rotate = true;

    [Header("Fade Settings")]
    public float fadeOutDuration = 10f;
    public float fadeInDuration = 10f;
    public float fadeDelay = 60f; // delay before fading back in

    private TextMeshProUGUI tmp;
    private Vector3 initialPos;
    private Vector3 initialScale;
    private Quaternion initialRot;
    private Color initialColor;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        initialPos = tmp.rectTransform.localPosition;
        initialScale = tmp.rectTransform.localScale;
        initialRot = tmp.rectTransform.localRotation;
        initialColor = tmp.color;
    }

    void OnEnable()
    {
        StartCoroutine(AnimateText());
        StartCoroutine(FadeCycle());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        tmp.rectTransform.localPosition = initialPos;
        tmp.rectTransform.localScale = initialScale;
        tmp.rectTransform.localRotation = initialRot;
        tmp.color = initialColor;
    }

    IEnumerator AnimateText()
    {
        while (true)
        {
            Vector3 newPos = initialPos;
            Vector3 newScale = initialScale;
            Quaternion newRot = initialRot;

            if (shake)
            {
                newPos += new Vector3(
                    Random.Range(-shakeAmount, shakeAmount),
                    Random.Range(-shakeAmount, shakeAmount),
                    0f);
            }

            if (scale)
            {
                float scaleFactor = 1 + Random.Range(-scaleAmount, scaleAmount);
                newScale = initialScale * scaleFactor;
            }

            if (rotate)
            {
                newRot = Quaternion.Euler(0, 0, Random.Range(-rotationAmount, rotationAmount));
            }

            if (randomColors)
            {
                tmp.color = new Color(Random.value, Random.value, Random.value, tmp.color.a);
            }

            tmp.rectTransform.localPosition = newPos;
            tmp.rectTransform.localScale = newScale;
            tmp.rectTransform.localRotation = newRot;

            yield return new WaitForSeconds(effectSpeed);
        }
    }

    IEnumerator FadeCycle()
    {
        while (true)
        {
            // Fade out
            yield return StartCoroutine(FadeText(1f, 0f, fadeOutDuration));

            // Wait before fading back in
            yield return new WaitForSeconds(fadeDelay);

            // Fade in
            yield return StartCoroutine(FadeText(0f, 1f, fadeInDuration));
        }
    }

    IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color c = tmp.color;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            tmp.color = new Color(c.r, c.g, c.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        tmp.color = new Color(c.r, c.g, c.b, endAlpha);
    }
}
