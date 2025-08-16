//  Version 0.8.5
//  This script animates text in the Information display.
//  This script took an oddly long time to work properly.
//  Note that neither Unity nor C# seems to like the idea of
//  a reverse scroll. Do not attempt Star Wars credit roll...

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VerticalScroller : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public float scrollSpeed = 3f;
    public float fadeDuration = 0.4f;

    [TextArea(2, 20)]
    public string inputLines;

    private RectTransform textRect;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (textDisplay == null) return;

        textDisplay.text = inputLines;

        textRect = textDisplay.GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (textRect == null) return;

        textRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (textRect.anchoredPosition.y >= 4.5f)
        {
            ResetScroll();
        }
    }

    public void SetDialogue(string dialogue)
    {
        inputLines = dialogue;
        textDisplay.text = inputLines;
        textRect.anchoredPosition = Vector2.zero;
        canvasGroup.alpha = 1;
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    void ResetScroll()
    {
        StartCoroutine(FadeOutThenReset());
    }

    IEnumerator FadeOutThenReset()
    {
        yield return FadeOut();
        textRect.anchoredPosition = Vector2.zero;
        yield return FadeIn();
    }
}
