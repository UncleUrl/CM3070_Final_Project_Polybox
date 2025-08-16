// Version 0.8.5
// This script is used to highlight sequencer buttons during playback.
//  This script acts as the main timing to coordinate all flashing features 
//  in time to the bpm. Effectively it acts as a crude MIDI clock.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SequencerMetronomeFlash : MonoBehaviour
{
    public Transform beatRow;
    public Transform subdivisionRow;
    public Transform synthRow;

    private float totalBarDuration = 4f;
    private int beatCount = 4;
    private int subdivisionCount = 4;
    private int synthCount = 4;

    private List<Renderer> beatButtons = new List<Renderer>();
    private List<Color> beatBaseColors = new List<Color>();

    private List<Renderer> subdivisionButtons = new List<Renderer>();
    private List<Color> subdivisionBaseColors = new List<Color>();

    private List<Renderer> synthButtons = new List<Renderer>();
    private List<Color> synthBaseColors = new List<Color>();

    private Color flashColor = Color.white;
    private float flashDuration = 0.15f;

    private Coroutine flashingCoroutine;

    public TextMeshProUGUI debugText;

    public void InitializeAndStart(float barDuration, int beats, int subdivisions)
    {
        totalBarDuration = barDuration;
        beatCount = beats;
        subdivisionCount = subdivisions;
        synthCount = synthRow != null ? synthRow.childCount : 0;

        if (beatRow != null)
            CacheRenderers(beatRow, beatButtons, beatBaseColors);

        if (subdivisionRow != null)
            CacheRenderers(subdivisionRow, subdivisionButtons, subdivisionBaseColors);

        if (synthRow != null)
            CacheRenderers(synthRow, synthButtons, synthBaseColors);

        if (flashingCoroutine != null)
            StopCoroutine(flashingCoroutine);

        flashingCoroutine = StartCoroutine(FlashSequencer());
    }

    // Cache the renderers and their base colors for flashing
    //  This function clears the existing lists and populates them with the current renderers and their colors
    void CacheRenderers(Transform parent, List<Renderer> buttons, List<Color> colors)
    {
        buttons.Clear();
        colors.Clear();

        foreach (Transform child in parent)
        {
            Renderer r = child.GetComponent<Renderer>();
            if (r != null)
            {
                buttons.Add(r);
                colors.Add(r.material.color);
            }
        }
    }

    // Internal flashing where needed (displays)
    IEnumerator FlashSequencer()
    {
        while (true)
        {
            yield return null;
        }
    }

    void FlashAndRestore(List<Renderer> buttons, List<Color> baseColors, int index)
    {
        if (buttons == null || buttons.Count == 0) return;
        if (index < 0 || index >= buttons.Count) return;
        StartCoroutine(FlashRoutine(buttons[index], baseColors[index]));
    }

    IEnumerator FlashRoutine(Renderer buttonRenderer, Color baseColor)
    {
        buttonRenderer.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        buttonRenderer.material.color = baseColor;
    }

    public void FlashBeatAtIndex(int index)
    {
        FlashAndRestore(beatButtons, beatBaseColors, index);
    }

    public void FlashSubdivisionAtIndex(int index)
    {
        FlashAndRestore(subdivisionButtons, subdivisionBaseColors, index);
    }

    public void FlashSynthAtIndex(int index)
    {
        FlashAndRestore(synthButtons, synthBaseColors, index);
    }

    public void Reset()
    {
        // Instantly reset all button colors to their base state
        for (int i = 0; i < beatButtons.Count; i++)
            beatButtons[i].material.color = beatBaseColors[i];

        for (int i = 0; i < subdivisionButtons.Count; i++)
            subdivisionButtons[i].material.color = subdivisionBaseColors[i];

        for (int i = 0; i < synthButtons.Count; i++)
            synthButtons[i].material.color = synthBaseColors[i];
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
