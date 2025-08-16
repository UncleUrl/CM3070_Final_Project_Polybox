//  Version 0.8.5
//  This script controls the color change of buttons when raycast.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonColourChanger : MonoBehaviour
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
            UpdateDebugText("Renderer or material not found!");
            return;
        }

        colorOriginal = renderer.material.color;
        materialButton = new Material(renderer.material);
        renderer.material = materialButton;
    }

    public void ChangeButtonColourAndRevert()
    {
        if (materialButton == null) return;

        // brighten button colour (light up)
        Color litColor = new Color(
            Mathf.Min(colorOriginal.r + 0.85f, 1f),
            Mathf.Min(colorOriginal.g + 0.85f, 1f),
            Mathf.Min(colorOriginal.b + 0.85f, 1f),
            colorOriginal.a
        );

        materialButton.color = litColor;
        Invoke(nameof(RevertColor), durationColorChange);
    }

    public void RevertColor()
    {
        if (materialButton != null)
            materialButton.color = colorOriginal;
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
// last working render