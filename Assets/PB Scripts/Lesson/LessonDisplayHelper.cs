// Version 0.8.5
// This script manages the Beat Count/Counter display.

using UnityEngine;
using TMPro;

public class LessonDisplayHelper : MonoBehaviour
{
    public TimeSignatureDisplay timeSigDisplay;
    public BeatDisplay beatDisplay;

    [Header("UI")]
    public TextMeshProUGUI debugText;

 
    public void SetFlashIndex(int index)
    {
        if (beatDisplay != null)
            beatDisplay.SetFlashIndex(index);
    }

    public void SetTimeSignature(int subdivisionCount)
    {
        UpdateDebugText($"subdivisionCount: {subdivisionCount}");
        if (timeSigDisplay != null)
            timeSigDisplay.beatValue = subdivisionCount;
    }

    public void SetBeatDigits(string beatPattern)
    {
        string digits = GenerateDigitSequence(new string[] { beatPattern });
        UpdateDebugText($"beatCount: {beatPattern}");
        if (beatDisplay != null)
        {
            beatDisplay.SetDigitSequence(digits);
        }
    }

    public void SetBeatCount(int beatCount)
    {
        UpdateDebugText($"beatCount: {beatCount}");
        if (timeSigDisplay != null)
             {
            timeSigDisplay.beatCount = beatCount;
            timeSigDisplay.RefreshDisplay();
        }
    }

    public string GenerateDigitSequence(string[] beatPattern)
    {
        int count = 1;
        string singleDigits = "";
        string doubleDigits = "";

        foreach (string bar in beatPattern)
        {
            foreach (char c in bar)
            {
                if (count <= 9)
                {
                    singleDigits += count.ToString();
                }
                else
                {
                    doubleDigits += count.ToString("D2");
                }
                count++;
            }
        }

        return singleDigits + (string.IsNullOrEmpty(doubleDigits) ? "" : " " + doubleDigits);
    }
    
        void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}