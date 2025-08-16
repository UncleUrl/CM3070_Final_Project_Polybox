// Version 0.8.5
// This script is used to highlight sequencer buttons.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SequencerButtonHighlighter : MonoBehaviour
{
    private Material materialButton;
    private Color colorOriginal;
    public float durationColorChange = 0.3f;
    public TextMeshProUGUI debugText;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null || renderer.material == null)
        {
            UpdateDebugText("Renderer or material missing on SequencerButton.");
            return;
        }

        colorOriginal = renderer.material.color;
        materialButton = new Material(renderer.material);
        renderer.material = materialButton;
    }

    public void HighlightButton()
    {
        if (materialButton == null) return;

        //  increase button colour closer to white (light up)
        Color litColor = new Color(
            Mathf.Min(colorOriginal.r + 0.85f, 1f),
            Mathf.Min(colorOriginal.g + 0.85f, 1f),
            Mathf.Min(colorOriginal.b + 0.85f, 1f),
            colorOriginal.a
        );

        materialButton.color = litColor;
        Invoke(nameof(RevertColor), durationColorChange);
    }

    private void RevertColor()
    {
        if (materialButton != null)
            materialButton.color = colorOriginal;
    }

    public void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
