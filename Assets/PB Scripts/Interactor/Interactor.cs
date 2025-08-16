// Version 0.8.5
// This script handles user interactions with the visual elements.
//  Again, this is some crude code to deal with Unity's interaction system.

using UnityEngine;
using TMPro;

public class Interactor : MonoBehaviour
{
    [Header("Visual Feedback")]
    public GameObject visualPrompt;
    public FlashingBorder flashingBorder;
    public PlayFlashing playFlashing;

    [Header("UI")]
    public TextMeshProUGUI debugText;
    public GameObject instructionsUI; 

    [Header("Lesson Management")]
    public LessonManager lessonManager; 

    private bool hasBeenUsed = false;
    
    public void OnInteract()
    {
        hasBeenUsed = true;
        
        // Check if instructions are currently visible BEFORE hiding them
        bool instructionsWereVisible = (instructionsUI != null && instructionsUI.activeInHierarchy);
        
        if (visualPrompt != null)
            visualPrompt.SetActive(false);
        
        // Stop the flashing scripts
        if (flashingBorder != null)
            flashingBorder.OnInteract();
        
        if (playFlashing != null)
            playFlashing.OnInteract();
        
        // Hide Instructions UI
        if (instructionsUI != null)
            instructionsUI.SetActive(false);
        
        ProcessNextState(instructionsWereVisible);
    }

    // The following allows for the user to see the flashing instructions advance
    // after raycasting to bring up the instruction buttons.
    void ProcessNextState(bool instructionsWereVisible)
    {
        if (instructionsWereVisible)
        {
            // Instructions were visible - just hide them, don't advance lesson
            UpdateDebugText("Instructions hidden - same lesson");
        }
        else
        {
            // Instructions were not visible - advance to next lesson
            if (lessonManager != null)
            {
                lessonManager.LoadNextLesson();
            }
            else
            {
                UpdateDebugText("Moving to next state");
            }
        }
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}