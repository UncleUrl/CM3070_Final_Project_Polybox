// Version 0.8.5
// This script controls the information button's interactions.

using System.Collections;
using UnityEngine;

public class InformationButton : MonoBehaviour
{
    [Header("UI")]
    public GameObject instructionsUI; // Reference to Instructions GameObject
    
    [Header("Interactor References")]
    public FlashingBorder flashingBorder; // Reference to flashing border script
    public PlayFlashing playFlashing;     // Reference to play flashing script
    
    public void OnInteract()
    {
        // Show Instructions UI
        if (instructionsUI != null)
            instructionsUI.SetActive(true);
        
        // Start coroutine to re-enable flashing after 5 seconds
        StartCoroutine(RestartFlashingAfterDelay());
    }
    
    IEnumerator RestartFlashingAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        
        // Reset and restart flashing
        if (flashingBorder != null)
        {
            flashingBorder.ResetAndStartFlashing();
        }
        
        if (playFlashing != null)
        {
            playFlashing.ResetAndStartFlashing();
        }
    }
}