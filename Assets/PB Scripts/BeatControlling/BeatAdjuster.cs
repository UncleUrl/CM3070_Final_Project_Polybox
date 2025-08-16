//  Version 0.8.5
// This code allows for adjusting the beat count via raycasting.

using UnityEngine;
using TMPro;

public class BeatAdjuster : MonoBehaviour
{
    public enum AdjustmentType { Up, Down }
    public AdjustmentType adjustment = AdjustmentType.Up;

    public TextMeshProUGUI debugText;
    private float cooldownTime = 0.3f;
    private float nextAllowedTime = 0f;

    public void TriggerAdjust()
    {
        if (Time.time < nextAllowedTime)
        {
            UpdateDebugText("Trigger ignored (cooldown active)");
            return;
        }

        BeatPlaybackEngine beatEngine = FindFirstObjectByType<BeatPlaybackEngine>();
        if (beatEngine == null)
        {
            UpdateDebugText("BeatPlaybackEngine not found.");
            return;
        }

        int delta = adjustment == AdjustmentType.Up ? 1 : -1;
        beatEngine.AdjustBeatCount(delta);

        UpdateDebugText($"{gameObject.name} triggered. Beat count adjusted by {delta}.");

        nextAllowedTime = Time.time + cooldownTime;
    }

    void OnRaycastHit()
{
    if (UnifiedSequencerPlaybackEngine.interactionLocked)
        return;
}

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
