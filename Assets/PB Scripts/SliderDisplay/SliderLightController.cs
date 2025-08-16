//  Version 0.8.5
//  This script animates the lights on the sliders for user feedback.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderLightController : MonoBehaviour
{
    public Transform tempoSlider;
    public Transform beatSlider;
    public Transform synthSlider;

    public Renderer tempoLight;
    public Renderer beatLight;
    public Renderer synthLight;

    public float flashDuration = 0.03f; // Seconds

    Color onColor = new Color32(0xB7, 0x25, 0x1B, 0xFF);  // #B7251B
    Color offColor = new Color32(0x14, 0x00, 0x00, 0xFF); // #140000

    float tempoFlashTimer = 0f;
    float beatFlashTimer = 0f;
    float synthFlashTimer = 0f;

    float lastTempoY;
    float lastBeatY;
    float lastSynthY;

    void Start()
    {
        lastTempoY = tempoSlider.localPosition.y;
        lastBeatY = beatSlider.localPosition.y;
        lastSynthY = synthSlider.localPosition.y;
    }

    void Update()
    {
        float yTempo = tempoSlider.localPosition.y;
        float yBeat = beatSlider.localPosition.y;
        float ySynth = synthSlider.localPosition.y;

        if (!Mathf.Approximately(yTempo, lastTempoY))
        {
            tempoFlashTimer = flashDuration;
            lastTempoY = yTempo;
        }

        if (!Mathf.Approximately(yBeat, lastBeatY))
        {
            beatFlashTimer = flashDuration;
            lastBeatY = yBeat;
        }

        if (!Mathf.Approximately(ySynth, lastSynthY))
        {
            synthFlashTimer = flashDuration;
            lastSynthY = ySynth;
        }

        if (tempoFlashTimer > 0f)
        {
            tempoLight.material.color = onColor;
            tempoFlashTimer -= Time.deltaTime;
        }
        else
        {
            tempoLight.material.color = offColor;
        }

        if (beatFlashTimer > 0f)
        {
            beatLight.material.color = onColor;
            beatFlashTimer -= Time.deltaTime;
        }
        else
        {
            beatLight.material.color = offColor;
        }

        if (synthFlashTimer > 0f)
        {
            synthLight.material.color = onColor;
            synthFlashTimer -= Time.deltaTime;
        }
        else
        {
            synthLight.material.color = offColor;
        }
    }
}
