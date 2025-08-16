// Version 0.8.5
// This script generates the correct number of sequencer buttons
// and arranges them in rows for the sequencer UI logic.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SequencerRowBuilder : MonoBehaviour
{
    [Header("Button Settings")]
    public GameObject synthButtonPrefab;
    public GameObject sequencerButtonPrefab;
    public GameObject beatButtonPrefab;

    public GameObject subdivisionButtonPrefab;

    public GameObject nullSynthButtonPrefab;

    public float spacing = 0.001f;

    [Header("Row Parents")]
    public Transform synthRow;
    public Transform beatRow;
    public Transform subdivisionRow;

    [Header("Sequencer Reference")]
    public Transform sequencerRow;

    [Header("SynthRow Reference")]
    public Transform synthRowParent;

    public TextMeshProUGUI debugText;

    public void BuildSequencer(int synthCount, int beatCount, int subdivisionCount)
    {
        BuildRow(beatRow, beatCount, beatButtonPrefab);
        BuildRow(subdivisionRow, subdivisionCount, subdivisionButtonPrefab);
    }

    public void SetSynthButtonPrefab(GameObject newPrefab)
    {
        synthButtonPrefab = newPrefab;
    }

    public void BuildSynthRow(int numberOfSteps)
    {
        if (synthRowParent == null || synthButtonPrefab == null)
            return;

        for (int i = synthRowParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(synthRowParent.GetChild(i).gameObject);

        float rowWidth = sequencerRow.localScale.y;
        float totalSpacing = spacing * (numberOfSteps - 1);
        float buttonWidth = (rowWidth - totalSpacing) / numberOfSteps;

        for (int i = 0; i < numberOfSteps; i++)
        {
            // because the Synth buttons can be individually assigned different samples they need to be instantiated
            GameObject button = Instantiate(synthButtonPrefab, synthRowParent);
            Vector3 originalScale = button.transform.localScale;
            button.transform.localScale = new Vector3(0.4f, buttonWidth, originalScale.z);
            float yPos = i * (buttonWidth + spacing) - (rowWidth / 2f) + (buttonWidth / 2f);
            button.transform.localPosition = new Vector3(0f, yPos, 0f);
            // Each Synth button also requires a unique name in order to assign different samples
            button.name = $"SynthStep_{i}";
        }
    }
 
    void BuildRow(Transform rowParent, int count, GameObject buttonPrefab)
    {
        if (sequencerRow == null)
        {
            UpdateDebugText("SequencerRow reference missing!");
            return;
        }

        for (int i = rowParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(rowParent.GetChild(i).gameObject);

        float rowWidth = sequencerRow.localScale.y;
        float totalSpacing = spacing * (count - 1);
        float buttonWidth = (rowWidth - totalSpacing) / count;

        for (int i = 0; i < count; i++)
        {
            GameObject button = Instantiate(buttonPrefab, rowParent);
            Vector3 originalScale = button.transform.localScale;
            button.transform.localScale = new Vector3(0.4f, buttonWidth, originalScale.z);
            float yPos = i * (buttonWidth + spacing) - (rowWidth / 2f) + (buttonWidth / 2f);
            button.transform.localPosition = new Vector3(0f, yPos, 0f);
        }
    }

    public void ToggleSynthRowVisibility(bool visible)
    {
        if (synthRowParent != null)
        {
            synthRowParent.gameObject.SetActive(visible);
            UpdateDebugText($"Synth row visibility set to {visible}");
        }
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
