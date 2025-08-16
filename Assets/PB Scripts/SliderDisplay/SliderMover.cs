//  Version 0.8.5
//  This script animates the sliders to move on raycast.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SliderMover : MonoBehaviour
{
    public float maxY = 0.07f;
    public float minY = 0.05f;

    private GameObject selectedSlider;
    private Vector3 lastControllerPos;
    private bool wasTriggerHeld = false;

    private readonly Dictionary<string, float[]> sliderPositions = new Dictionary<string, float[]>();
    private readonly Dictionary<string, int> currentIndex = new Dictionary<string, int>();

    private SynthAudioEngine synthAudioEngine;
    private UnifiedSequencerPlaybackEngine unifiedSequencer;

    void Start()
    {
        float range = maxY - minY;
        float step = range / 2f;

        sliderPositions["Beat Division"] = new float[] {
            minY,
            minY + step,
            maxY
        };

        sliderPositions["Synth"] = new float[] {
            minY,
            minY + step,
            maxY
        };

        currentIndex["Beat Division"] = 0;
        currentIndex["Synth"] = 0;

        synthAudioEngine = FindAnyObjectByType<SynthAudioEngine>();
        unifiedSequencer = FindAnyObjectByType<UnifiedSequencerPlaybackEngine>();
    }

    void Update()
    {
        bool triggerPressed = IsTriggerPressed();

        if (selectedSlider == null)
        {
            if (triggerPressed && !wasTriggerHeld)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Slider"))
                    {
                        selectedSlider = hit.collider.gameObject;
                        lastControllerPos = transform.position;

                        if (selectedSlider.name == "Beat Division")
                        {
                            IncrementSlider("Beat Division");
                            selectedSlider = null;
                        }
                        else if (selectedSlider.name == "Synth")
                        {
                            IncrementSlider("Synth");
                            selectedSlider = null;
                        }
                    }
                }
            }
        }
        else
        {
            if (triggerPressed)
            {
                if (selectedSlider.name == "Tempo")
                {
                    Vector3 delta = transform.position - lastControllerPos;
                    Vector3 pos = selectedSlider.transform.localPosition;
                    pos.y = Mathf.Clamp(pos.y + delta.y, minY, maxY);
                    selectedSlider.transform.localPosition = pos;
                    lastControllerPos = transform.position;
                }
            }
            else
            {
                selectedSlider = null;
            }
        }

        wasTriggerHeld = triggerPressed;
    }

    void IncrementSlider(string name)
    {
        int i = currentIndex[name];
        i++;
        if (i >= sliderPositions[name].Length)
            i = 0;

        currentIndex[name] = i;

        GameObject sliderObj = GameObject.Find(name);
        if (sliderObj != null)
        {
            Vector3 pos = sliderObj.transform.localPosition;
            pos.y = sliderPositions[name][i];
            sliderObj.transform.localPosition = pos;
        }

        if (name == "Synth" && synthAudioEngine != null)
        {
            synthAudioEngine.SwitchSynthLibrary(i);
        }
        else if (name == "Beat Division" && unifiedSequencer != null)
        {
            unifiedSequencer.SetSubdivisionFromSlider(i);
        }
    }

    bool IsTriggerPressed()
    {
        var devices = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller, devices);

        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool value) && value)
                return true;
        }
        return false;
    }
}
