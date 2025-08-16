// Version 0.8.5
// This script manages the adjustment of synth notes on the Polybox.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class RaycastSynthNoteTrigger : MonoBehaviour
{
    public float rayLength = 10f;
    public Transform rayOrigin;
    public TextMeshProUGUI debugText;
    public SynthNoteAssignmentManager synthNoteAssignmentManager;

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

                if (hitObj.CompareTag("Synth"))
                {
                    synthNoteAssignmentManager?.OnSynthButtonRaycast(hitObj.name);
                    UpdateDebugText($"Synth button hit: {hitObj.name}");
                    return;
                }

                if (hitObj.CompareTag("SynthNoteUp") || hitObj.CompareTag("SynthNoteDown"))
                {
                    synthNoteAssignmentManager?.OnSynthNoteAdjustRaycast(hitObj.tag);
                    return;
                }
            }
        }
        else if (!isPressed)
        {
            triggerPreviouslyHeld = false;
        }
    }

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
