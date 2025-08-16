// Version 0.8.5
// This script manages the lesson flow and timing.

using System.Collections;
using UnityEngine;
using TMPro;

public class LessonManager : MonoBehaviour
{
    [Header("Lessons")]
    public LessonData[] lessons; 

    [Header("References")]
    public UnifiedSequencerPlaybackEngine sequencerEngine;
    public FlashingBorder flashingBorder;
    public PlayFlashing playFlashing;
    public TextMeshProUGUI debugText;


    [Header("Timing")]
    public float autoPromptDelay = 30f; //default 30 seconds

    private int currentLessonIndex = 0;
    private Coroutine autoPromptCoroutine;

    void Start()
    {
        LoadCurrentLesson();
        StartAutoPromptTimer(); 
    }

    public void LoadNextLesson()
    {
        // Stop current auto-prompt
        if (autoPromptCoroutine != null)
        {
            StopCoroutine(autoPromptCoroutine);
            autoPromptCoroutine = null;
        }

        // Move to next lesson
        currentLessonIndex++;

        if (currentLessonIndex < lessons.Length)
        {
            LoadCurrentLesson();
            StartAutoPromptTimer();
        }
        else
        {
            // All lessons completed
            sequencerEngine.UpdateDebugText("All lessons completed!");
        }
    }
    
    void LoadCurrentLesson()
    {
        if (currentLessonIndex < lessons.Length && lessons[currentLessonIndex] != null)
        {
            // Load the new lesson
            sequencerEngine.SetLesson(lessons[currentLessonIndex]);
            
            // Update dialogue using VerticalScroller's SetDialogue method
            if (sequencerEngine.verticalScroller != null)
            {
                sequencerEngine.verticalScroller.SetDialogue(lessons[currentLessonIndex].dialogueText);
            }

             // Update lesson text field
            if (sequencerEngine.lessonText != null)
            {
                sequencerEngine.lessonText.text = lessons[currentLessonIndex].lessonText;
            }
            
            // Update debug info
            sequencerEngine.UpdateDebugText($"Loaded Lesson {currentLessonIndex + 1}");
        }
    }

    void UpdateDialogueText()
    {
        if (sequencerEngine.verticalScroller != null && lessons[currentLessonIndex] != null)
        {
            sequencerEngine.verticalScroller.inputLines = lessons[currentLessonIndex].dialogueText;
        }
    }

    void StartAutoPromptTimer()
    {
        autoPromptCoroutine = StartCoroutine(AutoPromptAfterDelay());
    }

    IEnumerator AutoPromptAfterDelay()
    {
        yield return new WaitForSeconds(autoPromptDelay);

        // Trigger flashing prompt
        if (flashingBorder != null)
            flashingBorder.ResetAndStartFlashing();

        if (playFlashing != null)
            playFlashing.ResetAndStartFlashing();
    }
    
        void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}