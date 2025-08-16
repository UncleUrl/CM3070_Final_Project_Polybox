//  Version 0.8.5
//  This script controls the Time signature display, including updating and flashing in sync.

using System.Collections;
using UnityEngine;
using TMPro;

public class TimeSignatureDisplay : MonoBehaviour
{
    public int beatValue; 

    public TextMeshProUGUI timeSigText;
    private BeatDisplay beatDisplay;

    public int beatCount;
    private int lastFlashIndex = -1;

    private float flashDuration = 0.15f;

    [Header("UI")]
    public TextMeshProUGUI debugText;

    void Start()
    {   
        beatDisplay = Object.FindFirstObjectByType<BeatDisplay>();

        RefreshDisplay();
        StartCoroutine(FlashTopNumber());
    }

    public void RefreshDisplay()
    {
        string dash = (beatCount > 10 || beatValue > 10) ? "--" : "-";
        timeSigText.text = $"{beatCount}\n{dash}\n{beatValue}";
        UpdateDebugText($"beatCount: {beatCount}");
    }

    IEnumerator FlashTopNumber()
    {
        while (true)
        {
            int flashIndex = beatDisplay.flashIndex;

            if (flashIndex != lastFlashIndex && flashIndex >= 0 && flashIndex < beatCount)
            {
                lastFlashIndex = flashIndex;
                // change length of divider dash depending on whether one or two digits
                string dash = (beatCount > 10 || beatValue > 10) ? "--" : "-";

                // show top number
                timeSigText.text = $"{beatCount}\n{dash}\n{beatValue}";
                yield return new WaitForSeconds(flashDuration);

                // hide top number (flash)
                timeSigText.text = $"\n{dash}\n{beatValue}";
            }

            yield return null;
        }
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
