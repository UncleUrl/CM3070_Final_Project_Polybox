//  Version 0.8.5
//  This script allows to adjust the number of synth beats

using UnityEngine;
using TMPro;

public class SynthBeatAdjuster : MonoBehaviour
{
    public enum AdjustmentType { Up, Down }
    public AdjustmentType adjustment = AdjustmentType.Up;

    public TextMeshProUGUI debugText;
    private float cooldownTime = 0.5f; // half a second between presses
    private float nextAllowedTime = 0f;

    public void TriggerAdjust()
{
    if (Time.time < nextAllowedTime)
    {
        UpdateDebugText("Trigger ignored (cooldown active)");
        return;
    }

    SynthPlaybackEngine synth = FindFirstObjectByType<SynthPlaybackEngine>();
    if (synth == null)
    {
        UpdateDebugText("SynthPlaybackEngine not found.");
        return;
    }

    UnifiedSequencerPlaybackEngine unified = FindFirstObjectByType<UnifiedSequencerPlaybackEngine>();
    if (unified == null)
    {
        UpdateDebugText("UnifiedSequencerPlaybackEngine not found.");
        return;
    }

    int delta = adjustment == AdjustmentType.Up ? 1 : -1;
    synth.AdjustStepCount(delta, unified.GetBarDuration());

    nextAllowedTime = Time.time + cooldownTime;
}


    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
