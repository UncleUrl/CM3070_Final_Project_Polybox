//  Version 0.8.5
//  This script controls handling of the beat display on the Polybox.

using UnityEngine;
using TMPro;

public class BeatDisplay : MonoBehaviour
{
    public string digitSequence = "";
    public int flashIndex = 0;
    public TextMeshProUGUI counterText;

     [Header("UI")]
    public TextMeshProUGUI debugText;

    // void Awake()
    // {
    //     // counterText = GetComponentInChildren<TextMeshProUGUI>();
    // }

    public void SetDigitSequence(string digits)
    {
        digitSequence = digits;
        UpdateDisplay();
    }

    public void SetFlashIndex(int index)
    {

        flashIndex = index;
        UpdateDebugText($"Flash Index: {flashIndex} ");
        UpdateDisplay();
    }

    // This function changes and the displays the beat by flashing through a string 
    // with the count values of the number of beats, playing them in sync with the audio.
    public void UpdateDisplay()
    {
        if (string.IsNullOrEmpty(digitSequence))
        {
            counterText.text = "";
            return;
        }

        string[] parts = digitSequence.Split(' ');
        string singleDigits = parts[0];
        string doubleDigits = parts.Length > 1 ? parts[1] : "";

        if (flashIndex >= 0 && flashIndex < singleDigits.Length)
        {
            string spaces = new string(' ', flashIndex);
            counterText.text = spaces + singleDigits[flashIndex] + "\n";
        }
        else if (flashIndex >= singleDigits.Length)
        {
            int doubleIndex = flashIndex - singleDigits.Length;
            int pairIndex = doubleIndex * 2;
            if (pairIndex + 1 < doubleDigits.Length)
            {
                string spaces = new string(' ', doubleIndex);
                string pair = doubleDigits.Substring(pairIndex, 2);
                counterText.text = "\n" + spaces + pair;
            }
            else
            {
                counterText.text = "\n";
            }
        }
        else
        {
            counterText.text = singleDigits + (string.IsNullOrEmpty(doubleDigits) ? "" : "\n" + doubleDigits);
        }
    }
    
    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
