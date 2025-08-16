// Version 0.8.5
// This script provides a flashing border effect for visual feedback.
// This code is done in this manner to avoid conflicts in the other displays code.

using System.Collections;
using UnityEngine;
using TMPro;

public class FlashingBorder : MonoBehaviour
{
    [Header("Visual Feedback")]
    public GameObject visualPrompt;
    public TextMeshProUGUI borderText; 

    private Coroutine flashCoroutine;
    private bool hasBeenUsed = false;

    void Start()
    {
        // Behold - the crudest bit of code in this project.
        // The dotted border pattern - straight out of Fortran 
        // It is almost embarrassing that the vagracies of Unity and VR hardware reduces code to this...
        borderText.text = "• • • • • • • • • • • • • •\n•                    •\n•                    •\n•                    •\n•                    •\n•                    •\n•                    •\n•                    •\n•                    •\n•                    •\n•                    •\n•                    •\n• • • • • • • • • • • • • •";
        StartFlashing();
    }

    void StartFlashing()
    {
        if (flashCoroutine == null)
            flashCoroutine = StartCoroutine(StrobeBorder());
    }

    void StopFlashing()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }
    }

    IEnumerator StrobeBorder()
    {
        Color customRed = new Color32(183, 37, 27, 255); // #B7251B
        Color transparent = new Color32(183, 37, 27, 0);  // Same color but transparent

        while (!hasBeenUsed)
        {
            // Fade in
            for (float t = 0; t <= 1; t += Time.deltaTime * 3.5f) // 3.5f == fade speed
            {
                borderText.color = Color.Lerp(transparent, customRed, t);
                yield return null;
            }

            // Fade out
            for (float t = 0; t <= 1; t += Time.deltaTime * 3.5f)
            {
                borderText.color = Color.Lerp(customRed, transparent, t);
                yield return null;
            }
        }
    }

    public void OnInteract()
    {
        hasBeenUsed = true;
        StopFlashing();

        if (visualPrompt != null)
            visualPrompt.SetActive(false);
        if (borderText != null)
            borderText.gameObject.SetActive(false);
    }

    public void ResetAndStartFlashing()
    {
        hasBeenUsed = false;

        // Show the border text again
        if (borderText != null)
        {
            borderText.gameObject.SetActive(true);
            // Reset color to ensure it's visible when restarting
            Color customRed = new Color32(183, 37, 27, 255);
            borderText.color = customRed;
        }

        // Show the visual prompt again
        if (visualPrompt != null)
            visualPrompt.SetActive(true);

        StartFlashing();
    }
}