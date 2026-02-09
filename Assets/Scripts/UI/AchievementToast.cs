using UnityEngine;
using TMPro;
using System.Collections;

public class AchievementToast : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descText;

    [Header("Timing")]
    [SerializeField] private float fadeIn = 0.15f;
    [SerializeField] private float hold = 1.6f;
    [SerializeField] private float fadeOut = 0.25f;

    private Coroutine routine;

    void Awake()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void Show(string title, string desc)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShowRoutine(title, desc));
    }

    private IEnumerator ShowRoutine(string title, string desc)
    {
        titleText.text = $"🏆 {title}";
        descText.text = desc;

        // Fade in
        yield return FadeTo(1f, fadeIn);

        // Hold
        yield return new WaitForSeconds(hold);

        // Fade out
        yield return FadeTo(0f, fadeOut);

        routine = null;
    }

    private IEnumerator FadeTo(float target, float duration)
    {
        if (canvasGroup == null) yield break;

        float start = canvasGroup.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }

        canvasGroup.alpha = target;
    }
}