// Version 0.8.5
// This code controlls the playback of the rhythm per the data from the Lessons.

using UnityEngine;
using TMPro;

public class BeatPlaybackEngine : MonoBehaviour
{
    [Header("References")]
    public UnifiedSequencerPlaybackEngine unifiedEngine;
    public TextMeshProUGUI debugText;

    private int beatCount = 4;

    void Update()
    {
        if (unifiedEngine == null || debugText == null) return;

        int subdivisionCount = unifiedEngine.GetCurrentSubdivision();
        int bpm = unifiedEngine.bpm;

        debugText.text = $"[BeatPlaybackEngine]\nBeats: {beatCount}\nSubs: {subdivisionCount}\nBPM: {bpm}";
    }

    public int GetBeatCount()
    {
        return beatCount;
    }

    public void SetBeatCount(int newCount)
    {
        beatCount = Mathf.Clamp(newCount, 1, 64);
        unifiedEngine?.OnBeatCountChanged(beatCount);
        UpdateDebugText($"[BeatEngine] Set to {beatCount}");
    }

    public void AdjustBeatCount(int delta)
    {
        beatCount = Mathf.Clamp(beatCount + delta, 1, 64);
        unifiedEngine?.OnBeatCountChanged(beatCount);
        UpdateDebugText($"[BeatEngine] Adjusted to {beatCount}");
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
