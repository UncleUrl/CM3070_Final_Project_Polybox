//  Version 0.8.5
//  This script Allows the user to change the sample of the synth note for playback.

using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SynthNoteAssignmentManager : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    private bool assignmentModeActive = false;
    private string currentSynthStep = null;

    private readonly List<string> noteCycle = new List<string> { "A", "B", "C", "D", "E", "F", "G", "silence" };
    private Dictionary<string, string> stepNoteMap = new Dictionary<string, string>();

    private void Awake()
    {
        // Prepopulate 4 default steps with "A"
        for (int i = 0; i <= 3; i++)
        {
            string stepName = $"SynthStep_{i}";
            if (!stepNoteMap.ContainsKey(stepName))
                stepNoteMap[stepName] = "A";
        }
    }

    public void OnSynthButtonRaycast(string stepName)
    {
        currentSynthStep = stepName;

        if (!stepNoteMap.ContainsKey(stepName))
            stepNoteMap[stepName] = "A"; // default to A

        assignmentModeActive = true;
        UpdateDebugText($"Current note for {stepName} is: {stepNoteMap[stepName]}");
    }

    public void OnSynthNoteAdjustRaycast(string tag)
    {
        if (!assignmentModeActive || string.IsNullOrEmpty(currentSynthStep))
        {
            UpdateDebugText("Not in synth assignment mode. Raycast a synth button first.");
            return;
        }

        string currentNote = stepNoteMap[currentSynthStep];
        int currentIndex = noteCycle.IndexOf(currentNote);

        if (currentIndex == -1) currentIndex = 0;

        UpdateDebugText($"Current note for {currentSynthStep} is: {currentNote}");

        if (tag == "SynthNoteUp")
            currentIndex = (currentIndex + 1) % noteCycle.Count;
        else if (tag == "SynthNoteDown")
            currentIndex = (currentIndex - 1 + noteCycle.Count) % noteCycle.Count;
        else
        {
            UpdateDebugText("Invalid synth note adjustment tag.");
            return;
        }

        string newNote = noteCycle[currentIndex];
        stepNoteMap[currentSynthStep] = newNote;

        UpdateDebugText($"{currentSynthStep} is now {newNote}");
        assignmentModeActive = false;
    }

    public Dictionary<string, string> GetStepNoteMap()
    {
        return stepNoteMap;
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
