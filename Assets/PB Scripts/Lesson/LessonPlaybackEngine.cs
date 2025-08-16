// Version 0.8.5
// This script acts as the main playback engine for the lessons.
//  This is one of the main controllers of the application, along with
// UnifiedSequencerPlaybackEngine and SynthPlaybackEngine

using System.Collections;
using UnityEngine;
using TMPro;

public class LessonPlaybackEngine : MonoBehaviour
{
    [Header("Sequencer Components")]
    public SequencerRowBuilder sequencerRowBuilder;
    public SequencerMetronomeFlash metronomeFlash;

    [Header("Displays")]
    public BeatDisplay beatDisplay;
    public TimeSignatureDisplay timeSigDisplay;
    public VerticalScroller verticalScroller;

    [Header("Lesson Data")]
    public LessonData currentLesson;

    private int totalFlashSteps;
    private int currentFlashIndex = 0;

    private float totalBarDuration;

    public TextMeshProUGUI debugText;

    void Start()
    {
        if (currentLesson == null)
        {
            UpdateDebugText("No LessonData assigned to LessonPlaybackEngine!");
            return;
        }

        // Calculate bar duration from bpm and beat count
        totalBarDuration = (60f / currentLesson.bpm) * currentLesson.beatCount;

        // Build Sequencer Rows
        if (sequencerRowBuilder != null)
        {
            sequencerRowBuilder.BuildSequencer(1, currentLesson.beatCount, currentLesson.subdivisionCount);
        }

        // Assign rows and start metronome flash with timing
        if (metronomeFlash != null && sequencerRowBuilder != null)
        {
            metronomeFlash.beatRow = sequencerRowBuilder.beatRow;
            metronomeFlash.subdivisionRow = sequencerRowBuilder.subdivisionRow;
            metronomeFlash.InitializeAndStart(totalBarDuration, currentLesson.beatCount, currentLesson.subdivisionCount);
        }

        // Set TimeSignatureDisplay
        if (timeSigDisplay != null)
        {
            timeSigDisplay.beatValue = currentLesson.subdivisionCount;
        }

        // Set Dialogue
        if (verticalScroller != null)
        {
            verticalScroller.inputLines = currentLesson.dialogueText;
            UpdateDebugText(currentLesson.dialogueText);
        }

        // Prepare and send digit sequence to BeatDisplay
        string combinedDigits = GenerateDigitSequence(new string[] { currentLesson.beatPattern });

        if (beatDisplay != null)
        {
            beatDisplay.SetDigitSequence(combinedDigits);
        }

        totalFlashSteps = currentLesson.beatPattern.Length;
        currentFlashIndex = 0;

        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        while (true)
        {
            float interval = totalBarDuration / totalFlashSteps;

            if (beatDisplay != null)
            {
                beatDisplay.SetFlashIndex(currentFlashIndex);
            }

            currentFlashIndex = (currentFlashIndex + 1) % totalFlashSteps;

            yield return new WaitForSeconds(interval);
        }
    }

    //  This Updates the digit sequence in the Beat display.
    string GenerateDigitSequence(string[] beatPattern)
    {
        int count = 1;
        string singleDigits = "";
        string doubleDigits = "";

        foreach (string bar in beatPattern)
        {
            foreach (char c in bar)
            {
                if (count <= 9)
                {
                    singleDigits += count.ToString();
                }
                else
                {
                    doubleDigits += count.ToString("D2");
                }
                count++;
            }
        }

        return singleDigits + (string.IsNullOrEmpty(doubleDigits) ? "" : " " + doubleDigits);
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
