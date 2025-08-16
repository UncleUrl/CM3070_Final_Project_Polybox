// Version 0.8.5
// This script acts as the main playback engine for the lessons.
//  This is one of the main controllers of the application, along with
// UnifiedSequencerPlaybackEngine and SynthPlaybackEngine

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SynthPlaybackEngine : MonoBehaviour
{
    [Header("Synth Settings")]
    [Header("References")]
    public SynthAudioEngine audioEngine;
    public SequencerRowBuilder sequencerRowBuilder;
    public SequencerMetronomeFlash synthMetronomeFlash;
    public SynthNoteAssignmentManager noteAssignmentManager;
    public TextMeshProUGUI debugText;

    private float stepInterval;
    private int numberOfSteps;
    private int currentStep = 0;
    private Coroutine stepRoutine;
    private bool isPlaying = false;
    public int stepCount = 4;

    private Dictionary<string, string> stepNoteMap = new Dictionary<string, string>();

    void Start()
    {
        if (sequencerRowBuilder != null)
            sequencerRowBuilder.BuildSynthRow(stepCount);
    }

    // External/Manual triggering if necessary (debug)
    public void PlayStep(int stepIndex)
    {
        string noteToPlay;
        string stepKey = $"SynthStep_{currentStep}";
        if (stepNoteMap.TryGetValue(stepKey, out noteToPlay))
        {
            if (audioEngine != null)
                audioEngine.PlayNote(noteToPlay);
        }
    }

    public void InitializeSynth(float barDuration, int numberOfSteps)
    {
        this.numberOfSteps = numberOfSteps;
        stepInterval = barDuration / numberOfSteps;

        if (sequencerRowBuilder != null)
            sequencerRowBuilder.SetSynthButtonPrefab(
                audioEngine.IsNoSynth ? sequencerRowBuilder.nullSynthButtonPrefab : sequencerRowBuilder.synthButtonPrefab
            );

        if (numberOfSteps > 0 && sequencerRowBuilder != null)
            sequencerRowBuilder.BuildSynthRow(numberOfSteps);

        if (numberOfSteps > 0 && synthMetronomeFlash != null)
        {
            synthMetronomeFlash.synthRow = sequencerRowBuilder.synthRowParent;
            synthMetronomeFlash.InitializeAndStart(barDuration, numberOfSteps, 1);
        }

        currentStep = 0;
    }

    public void StartPlayback()
    {
        if (isPlaying) return;

        SyncDictionaryFromManager();

        if (debugText != null)
        {
            if (stepNoteMap == null || stepNoteMap.Count == 0)
            {
                debugText.text = "stepNoteMap is empty or null.";
            }
            else
            {
                List<string> entries = new List<string>();
                foreach (var kvp in stepNoteMap)
                {
                    entries.Add($"{kvp.Key} = {kvp.Value}");
                }
                debugText.text = "Imported Notes: " + string.Join(" | ", entries);
            }
        }

        stepRoutine = StartCoroutine(StepRoutine());
        isPlaying = true;
    }

    public void StopPlayback()
    {
        if (!isPlaying) return;

        if (stepRoutine != null)
            StopCoroutine(stepRoutine);

        isPlaying = false;
    }

    IEnumerator StepRoutine()
    {
        while (true)
        {
            string noteToPlay;
            // Try to get the note for the current step from the dictionary
            string stepKey = $"SynthStep_{currentStep}";
            if (stepNoteMap.TryGetValue(stepKey, out noteToPlay))
            {
                if (audioEngine != null)
                    audioEngine.PlayNote(noteToPlay); // Play the note from the dictionary
            }
            else
            {
                // Otherwise handle steps with no assigned note (e.g., silence)
                //  Null routine that seems to be needed by Unity to avoid crashes.
                //  Unity is an odd beast. It does not seem to like the notion of playing samples in a row. 
            }

            if (synthMetronomeFlash != null)
                synthMetronomeFlash.FlashSynthAtIndex(currentStep);

            currentStep = (currentStep + 1) % numberOfSteps;
            yield return new WaitForSeconds(stepInterval);
        }
    }

    public void AdjustStepCount(int delta, float barDuration)
    {
        numberOfSteps = Mathf.Clamp(numberOfSteps + delta, 1, 32);
        currentStep = 0;

        if (sequencerRowBuilder != null)
            sequencerRowBuilder.BuildSynthRow(numberOfSteps);

        stepInterval = barDuration / numberOfSteps;

        if (synthMetronomeFlash != null)
            synthMetronomeFlash.InitializeAndStart(stepInterval * numberOfSteps, numberOfSteps, 1);
    }

    public int GetCurrentStepCount()
    {
        return numberOfSteps;
    }

    public void ResetStep()
    {
        currentStep = 0;
    }

    private void SyncDictionaryFromManager()
    {
        if (noteAssignmentManager == null)
        {
            UpdateDebugText("Manager is NULL");
            return;
        }

        Dictionary<string, string> sourceMap = noteAssignmentManager.GetStepNoteMap();

        if (sourceMap == null || sourceMap.Count == 0)
        {
            UpdateDebugText("Imported dictionary is empty");
            return;
        }

        stepNoteMap.Clear();
        foreach (var kvp in sourceMap)
            stepNoteMap[kvp.Key] = kvp.Value;

        UpdateDebugText("Imported Keys: " + string.Join(", ", stepNoteMap.Keys));
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
