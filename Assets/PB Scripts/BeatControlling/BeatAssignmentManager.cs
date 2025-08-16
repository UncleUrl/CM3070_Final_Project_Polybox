//  Version 0.8.5
//  This script controls handling of the adjustments of samples used by the Beat.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeatAssignmentManager : MonoBehaviour
{
    public LessonAudioEngine lessonAudioEngine;  // Assign in inspector
    public TextMeshProUGUI debugText;

    private bool assignmentModeActive = false;

    // Call this on raycast hit of a beat button
    public void OnBeatButtonRaycast()
    {
        if (assignmentModeActive)
        {
            UpdateDebugText("Already in beat assignment mode. Select a drum button.");
            return;
        }

        assignmentModeActive = true;
        UpdateDebugText("Beat assignment mode active. Now select a drum button.");
    }

    // Call this on raycast hit of a drum button, passing its AudioSource clip
    public void OnDrumButtonRaycast(AudioClip drumClip)
    {
        if (!assignmentModeActive)
        {
            UpdateDebugText("Not in beat assignment mode. Raycast a beat button first.");
            return;
        }

        if (lessonAudioEngine == null)
        {
            UpdateDebugText("LessonAudioEngine reference missing!");
            assignmentModeActive = false;
            return;
        }

        if (drumClip == null)
        {
            UpdateDebugText("Selected drum button has no AudioClip.");
            return;
        }

        lessonAudioEngine.beatClip = drumClip;
        UpdateDebugText($"Beat clip assigned: {drumClip.name}");

        assignmentModeActive = false;
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
