//  Version 0.8.5
//  This script manages the playback from raycast button assignment of drum sounds..

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(XRSimpleInteractable))]


public class DrumButtonPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    public TextMeshProUGUI debugText;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(PlaySound);
    }


    void PlaySound(SelectEnterEventArgs args)
    {
        if (audioSource && audioSource.clip)
        {
            audioSource.Play();
            UpdateDebugText("Playing sound: " + audioSource.clip.name);
        }
        else
        {
            UpdateDebugText("No audio source or no clip on: " + gameObject.name);
        }
    }
    
    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
