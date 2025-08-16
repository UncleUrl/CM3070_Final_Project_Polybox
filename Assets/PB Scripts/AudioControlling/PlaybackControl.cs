//  Version 0.8.5
//  This script controls handling of the Play and Stop buttons.

using UnityEngine;
using TMPro;

public class PlaybackControl : MonoBehaviour
{
    public enum ControlType { Play, Stop }
    public ControlType control = ControlType.Play;

    public TextMeshProUGUI debugText;

    public void TriggerControl()
    {
        // Grab UnifiedSequencerPlaybackEngine 
        var unified = FindFirstObjectByType<UnifiedSequencerPlaybackEngine>();
        var synth = FindFirstObjectByType<SynthPlaybackEngine>();

        if (unified == null && synth == null)
        {
            UpdateDebugText("Playback engines not found.");
            return;
        }

        if (control == ControlType.Play)
        {
            unified?.StartPlayback();
            synth?.StartPlayback();
        }
        else // Stop
        {
            unified?.StopPlayback();
            synth?.StopPlayback();
        }
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
