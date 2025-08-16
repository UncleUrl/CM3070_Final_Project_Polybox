// Version 0.8.5
// This script plays each synth step based on the sample assign to it.

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SynthStepPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private ButtonColourChanger colourChanger;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        colourChanger = GetComponent<ButtonColourChanger>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
    }

    public void AssignClip(AudioClip clip)
    {
        // Store clip reference for PlayOneShot
        assignedClip = clip;
    }

    private AudioClip assignedClip;

    public void PlayNote()
    {
        if (audioSource != null && assignedClip != null)
        {
            audioSource.PlayOneShot(assignedClip);
        }

        // Manual change of colour as backup
        if (colourChanger != null)
            colourChanger.ChangeButtonColourAndRevert();
    }
}
