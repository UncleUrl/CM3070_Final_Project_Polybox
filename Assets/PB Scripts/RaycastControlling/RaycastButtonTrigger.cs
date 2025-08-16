// Version 0.8.5
// This script to control the responses of raycasting on the Polybox.
// This is the key component for handling user interactions in the VR environment.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class RaycastButtonTrigger : MonoBehaviour
{
    public float rayLength = 10f;
    public Transform rayOrigin;

    public SubdivisionAssignmentManager subdivisionAssignmentManager;
    public BeatAssignmentManager beatAssignmentManager;

    public SynthPlaybackEngine synthPlaybackEngine;

    public TextMeshProUGUI debugText;

    private bool triggerPreviouslyHeld = false;

    void Update()
    {
        bool isPressed = IsTriggerPressed();

        if (isPressed && !triggerPreviouslyHeld)
        {
            triggerPreviouslyHeld = true;

            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, rayLength))
            {
                GameObject hitObj = hit.collider.gameObject;
                UpdateDebugText($"Hit: {hitObj.name}");

                if (hitObj.CompareTag("Sound"))
                {
                    var audioSource = hitObj.GetComponent<AudioSource>();
                    if (audioSource)
                    {
                        subdivisionAssignmentManager?.OnDrumButtonRaycast(audioSource.clip);
                        beatAssignmentManager?.OnDrumButtonRaycast(audioSource.clip);

                        if (!audioSource.isPlaying)
                            audioSource.Play();
                    }

                    hitObj.GetComponent<ButtonColourChanger>()?.ChangeButtonColourAndRevert();
                    return;
                }

                if (hitObj.CompareTag("Transport"))
                {
                    hitObj.GetComponent<SynthBeatAdjuster>()?.TriggerAdjust();
                    hitObj.GetComponent<BeatAdjuster>()?.TriggerAdjust();
                    hitObj.GetComponent<PlaybackControl>()?.TriggerControl();
                    return;
                }

                if (hitObj.CompareTag("Subdivision"))
                {
                    subdivisionAssignmentManager?.OnSubdivisionButtonRaycast();
                    return;
                }

                if (hitObj.CompareTag("Beat"))
                {
                    beatAssignmentManager?.OnBeatButtonRaycast();
                    return;
                }

                if (hitObj.CompareTag("Synth"))
                {
                    UpdateDebugText("Synth Hit");
                    return;
                }

                if (hitObj.CompareTag("SynthBeatUp") || hitObj.CompareTag("SynthBeatDown"))
                {
                    var unified = FindFirstObjectByType<UnifiedSequencerPlaybackEngine>();
                    if (unified == null)
                    {
                        UpdateDebugText("UnifiedSequencerPlaybackEngine not found.");
                        return;
                    }

                    float barDuration = unified.GetBarDuration();
                    int delta = hitObj.CompareTag("SynthBeatUp") ? +1 : -1;

                    synthPlaybackEngine?.AdjustStepCount(delta, barDuration);
                    UpdateDebugText($"Synth steps adjusted by {delta}");
                    return;
                }

                if (hitObj.CompareTag("Interactor"))
                {
                    hitObj.GetComponent<Interactor>()?.OnInteract();
                    UpdateDebugText("Interactor activated - moving to next state");
                    return;
                }

                if (hitObj.CompareTag("Info"))
        {
                    hitObj.GetComponent<InformationButton>()?.OnInteract();
                    UpdateDebugText("Instructions shown");
                    return;
                }
            }
        }
        else if (!isPressed)
        {
            triggerPreviouslyHeld = false;
        }
    }

    //  Check for trigger press
    bool IsTriggerPressed()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller, devices);

        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool value) && value)
                return true;
        }
        return false;
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
