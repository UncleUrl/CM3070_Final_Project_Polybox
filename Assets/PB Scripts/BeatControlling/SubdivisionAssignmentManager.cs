//  Version 0.8.5
//  This script controls audio sample changes to the subdivision.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubdivisionAssignmentManager : MonoBehaviour
{
    public LessonAudioEngine lessonAudioEngine;  
    public TextMeshProUGUI debugText;

    private bool assignmentModeActive = false;

    // Call this on raycast hit of a subdivision button
    public void OnSubdivisionButtonRaycast()
    {
        if (assignmentModeActive)
        {
            UpdateDebugText("Already in assignment mode. Select a drum button.");
            return;
        }

        assignmentModeActive = true;
        UpdateDebugText("Subdivision assignment mode active. Now select a drum button.");
    }

    // Call this on raycast hit of a drum button, passing its AudioSource clip
    public void OnDrumButtonRaycast(AudioClip drumClip)
    {
        if (!assignmentModeActive)
        {
            UpdateDebugText("Not in assignment mode. Raycast a subdivision button first.");
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

        lessonAudioEngine.subdivisionClip = drumClip;
        UpdateDebugText($"Subdivision clip assigned: {drumClip.name}");

        assignmentModeActive = false;
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
