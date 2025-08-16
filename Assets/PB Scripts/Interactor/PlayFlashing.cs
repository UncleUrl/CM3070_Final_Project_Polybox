// Version 0.8.5
// This script provides a flashing effect for the Instruction Advance button.

using System.Collections;
using UnityEngine;
using TMPro;

public class PlayFlashing : MonoBehaviour
{
    [Header("Visual Feedback")]
    public TextMeshProUGUI playText; 
    private Coroutine playFlashCoroutine;
    private bool hasBeenUsed = false;

    void Start()
    {
        // Set up the play symbol
        if (playText != null)
            playText.text = "∆"; 

        StartFlashing();
    }

    void StartFlashing()
    {
        if (playFlashCoroutine == null)
            playFlashCoroutine = StartCoroutine(StrobePlay());
    }

    void StopFlashing()
    {
        if (playFlashCoroutine != null)
        {
            StopCoroutine(playFlashCoroutine);
            playFlashCoroutine = null;
        }
    }

    IEnumerator StrobePlay()
    {
        Color customRed = new Color32(183, 37, 27, 255); // #B7251B
        Color transparent = new Color32(183, 37, 27, 0);

        while (!hasBeenUsed)
        {
            // Fade in
            for (float t = 0; t <= 1; t += Time.deltaTime * 3.5f)
            {
                playText.color = Color.Lerp(transparent, customRed, t);
                yield return null;
            }

            // Fade out
            for (float t = 0; t <= 1; t += Time.deltaTime * 3.5f)
            {
                playText.color = Color.Lerp(customRed, transparent, t);
                yield return null;
            }
        }
    }

    public void OnInteract()
    {
        hasBeenUsed = true;
        StopFlashing();

        if (playText != null)
            playText.gameObject.SetActive(false);
    }
    
    public void ResetAndStartFlashing()
    {
    hasBeenUsed = false;
    
    // Show the play text again
    if (playText != null)
    {
        playText.gameObject.SetActive(true);
        // Reset color to ensure it's visible when restarting
        Color customRed = new Color32(183, 37, 27, 255);
        playText.color = customRed;
    }
    
    StartFlashing();
    }
}