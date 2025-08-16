// Version 0.8.5
// This script manages audio playback for a lesson, including beats, subdivisions, and bar accents.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class LessonAudioEngine : MonoBehaviour
{
    [Header("Assign Audio Clips")]
    public AudioClip beatClip;
    public AudioClip subdivisionClip;
    public AudioClip barAccentClip;
    public AudioClip silentClip;

    private AudioSource beatSource;
    private AudioSource subdivisionSource;
    private AudioSource barAccentSource;

    public TextMeshProUGUI debugText;

    void Awake()
    {
        // Create three separate AudioSources
        beatSource = gameObject.AddComponent<AudioSource>();
        subdivisionSource = gameObject.AddComponent<AudioSource>();
        barAccentSource = gameObject.AddComponent<AudioSource>();

        // Set all to 2D global (non-spatial)
        beatSource.spatialBlend = 0f;
        subdivisionSource.spatialBlend = 0f;
        barAccentSource.spatialBlend = 0f;

        // Disable play on awake
        beatSource.playOnAwake = false;
        subdivisionSource.playOnAwake = false;
        barAccentSource.playOnAwake = false;
    }

    public void PlayBeat(bool isActive)
    {
        if (isActive && beatClip != null)
        {
            beatSource.PlayOneShot(beatClip);
        }
        else if (!isActive && silentClip != null)
        {
            beatSource.PlayOneShot(silentClip);
        }
    }

    public void PlaySubdivision()
    {
        if (subdivisionClip != null)
        {
            subdivisionSource.PlayOneShot(subdivisionClip);
        }
    }

    public void PlayBarAccent()
    {
        if (barAccentClip != null)
        {
            barAccentSource.PlayOneShot(barAccentClip);
        }
    }

    public void SetMuted(bool mute)
    {
        AudioListener.volume = mute ? 0f : 1f;
    }

    // Message for declaring values loaded for debugging
    public void SetBeatTiming(int beatCount, int subdivisionCount, int bpm)
    {
        UpdateDebugText($"AudioEngine updated: {beatCount} beats, {subdivisionCount} subdivisions, {bpm} bpm.");
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
