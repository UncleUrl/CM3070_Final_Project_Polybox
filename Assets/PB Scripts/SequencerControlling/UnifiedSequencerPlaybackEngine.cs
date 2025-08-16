//  Version 0.8.5
//  This script manages the playback of both lesson audio and synth audio in a unified sequencer system.
//  This is one of the main controllers of the application, along with
// LessonPlaybackEngine and SynthPlaybackEngine

using System.Collections;
using UnityEngine;
using TMPro;

public class UnifiedSequencerPlaybackEngine : MonoBehaviour
{
    [Header("Sequencer Components")]
    public SequencerRowBuilder sequencerRowBuilder;
    public SequencerMetronomeFlash metronomeFlash;
    public SequencerMetronomeFlash synthMetronomeFlash;


    [Header("Audio Engines")]
    public LessonAudioEngine lessonAudioEngine;
    public SynthAudioEngine synthAudioEngine;
    public SynthPlaybackEngine synthPlaybackEngine;

    [Header("Lesson Data ")]
    public LessonData currentLesson;

    [Header("Synth Settings")]
    public int synthSteps = 4;
    public int bpm = 120;

    [Header("UI")]
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI lessonText;

    [Header("Displays")]
    public BeatDisplay beatDisplay;
    public TimeSignatureDisplay timeSigDisplay;

    public VerticalScroller verticalScroller;
    public LessonDisplayHelper displayHelper;

    [Header("Slider Input")]
    public Transform tempoSlider;
    public Transform beatDivisionSlider;
    private float lastTempoSliderY;

    private float barDuration;
    private float beatDuration;
    private int beatCount;
    private int subdivisionCount;

    private bool isPlaying = false;
    public static bool interactionLocked = false;

    private Coroutine beatRoutine;
    private Coroutine subdivisionRoutine;

    //  This is not fully implemented. This would assist in future devlopment
    //  if a user wanted to open the application to use only as a sequencer.
    public enum PlaybackMode
    {
        LessonOnly,
        SynthOnly,
        Combined
    }

    public PlaybackMode playbackMode = PlaybackMode.LessonOnly;

    void Start()
    {
        InitializeSequencer();
    }

    void Update()
    {
        HandleTempoSlider(); 
        timeSigDisplay.beatValue = subdivisionCount;
    }

    public void SetLesson(LessonData lesson)
    {
        currentLesson = lesson;
        InitializeSequencer(); 

        // Update lesson text display
        if (lessonText != null && currentLesson != null)
        {
            lessonText.text = currentLesson.lessonText;
        }
    }

    void HandleTempoSlider()
    {
        if (tempoSlider == null) return;

        float sliderY = tempoSlider.localPosition.y;
        float t = Mathf.InverseLerp(0.05f, 0.07f, sliderY);
        int newBpm = Mathf.RoundToInt(Mathf.Lerp(80f, 200f, t));

        bool isSliderMoving = !Mathf.Approximately(sliderY, lastTempoSliderY);
        lastTempoSliderY = sliderY;

        UnifiedSequencerPlaybackEngine.interactionLocked = isSliderMoving;

        bool mute = isSliderMoving;
        lessonAudioEngine?.SetMuted(mute);
        synthAudioEngine?.SetMuted(mute);

        //  Logic to adjust tempo and update sequencer
        if (newBpm != bpm)
        {
            bpm = newBpm;
            barDuration = 30f * beatCount / bpm;
            beatDuration = 60f / bpm;

            metronomeFlash?.InitializeAndStart(barDuration, beatCount, subdivisionCount);

            int currentSynthSteps = synthPlaybackEngine?.GetCurrentStepCount() ?? synthSteps;
            synthPlaybackEngine?.InitializeSynth(barDuration, currentSynthSteps);

            if (isPlaying)
            {
                synthPlaybackEngine?.ResetStep();
                synthPlaybackEngine?.StartPlayback();
            }
        }
    }

    public void SetSubdivisionFromSlider(int index)
    {
        if (isPlaying) return;

        int newSubdivision = 4;
        switch (index)
        {
            case 0: newSubdivision = 4; break;  // X/4 time signature
            case 1: newSubdivision = 8; break;  // X/8 time signature
            case 2: newSubdivision = 16; break; // X/16 time signature
        }

        if (newSubdivision == subdivisionCount) return;

        subdivisionCount = newSubdivision;
        barDuration = 30f * beatCount / bpm;
        beatDuration = 60f / bpm;

        sequencerRowBuilder?.BuildSequencer(1, beatCount, subdivisionCount);

        if (metronomeFlash != null)
        {
            metronomeFlash.beatRow = sequencerRowBuilder.beatRow;
            metronomeFlash.subdivisionRow = sequencerRowBuilder.subdivisionRow;
            metronomeFlash.InitializeAndStart(barDuration, beatCount, subdivisionCount);
        }

        if (currentLesson != null)
            currentLesson.subdivisionCount = subdivisionCount;

        if (displayHelper != null)
                displayHelper.SetTimeSignature(subdivisionCount);
    }

    //Send information to Display on PolyBox
    IEnumerator AssignDialogueToVerticalScroller(string dialogue)
    {
        yield return null; // wait one frame
        if (verticalScroller != null)
        {
            verticalScroller.inputLines = dialogue;
        }
    }

    void InitializeSequencer()
    {
        if (currentLesson != null)
        {
            beatCount = currentLesson.beatCount;
            subdivisionCount = currentLesson.subdivisionCount;
            bpm = 120;
        }
        else  // Default values if no lesson is set
        {
            beatCount = 4;
            subdivisionCount = 4;
            bpm = 120;
        }

        barDuration = 30f * beatCount / bpm;
        beatDuration = 60f / bpm;

        sequencerRowBuilder?.BuildSequencer(1, beatCount, subdivisionCount);

        if (metronomeFlash != null)
        {
            metronomeFlash.beatRow = sequencerRowBuilder.beatRow;
            metronomeFlash.subdivisionRow = sequencerRowBuilder.subdivisionRow;
            metronomeFlash.InitializeAndStart(barDuration, beatCount, subdivisionCount);
        }

        if (synthMetronomeFlash != null)
        {
            synthMetronomeFlash.beatRow = null;
            synthMetronomeFlash.subdivisionRow = null;
            synthMetronomeFlash.synthRow = sequencerRowBuilder.synthRowParent;
        }

        StartCoroutine(AssignDialogueToVerticalScroller(currentLesson.dialogueText));

        if (verticalScroller != null && currentLesson != null)
        {
            verticalScroller.inputLines = currentLesson.dialogueText;
            
        }

        if (synthPlaybackEngine != null)
        {
            synthPlaybackEngine.audioEngine = synthAudioEngine;
            synthPlaybackEngine.sequencerRowBuilder = sequencerRowBuilder;
            synthPlaybackEngine.synthMetronomeFlash = synthMetronomeFlash;

            synthAudioEngine.SwitchSynthLibrary(synthAudioEngine.GetCurrentSynthVoiceIndex());

            synthPlaybackEngine.InitializeSynth(barDuration, synthSteps);
        }

        //  Call DisplayHelper script to ensure sync
        if (displayHelper != null && currentLesson != null)
        {
            displayHelper.SetTimeSignature(currentLesson.subdivisionCount);
            displayHelper.SetBeatDigits(currentLesson.beatPattern);
            displayHelper.SetBeatCount(currentLesson.beatCount);
        }
    }

    public void StartPlayback()
    {
        if (isPlaying) return;

        isPlaying = true;

        if (playbackMode == PlaybackMode.LessonOnly || playbackMode == PlaybackMode.Combined)
        {
            beatRoutine = StartCoroutine(BeatRoutine());
            subdivisionRoutine = StartCoroutine(SubdivisionRoutine());
        }

        //  Synth predetermined in Lesson plan (OF implmented)
        if (playbackMode == PlaybackMode.SynthOnly || playbackMode == PlaybackMode.Combined)
        {
            synthPlaybackEngine?.ResetStep();
            synthPlaybackEngine?.StartPlayback();
        }
    }

    public void StopPlayback()
    {
        if (!isPlaying) return;

        if (beatRoutine != null) StopCoroutine(beatRoutine);
        if (subdivisionRoutine != null) StopCoroutine(subdivisionRoutine);

        synthPlaybackEngine?.StopPlayback();
        synthPlaybackEngine?.ResetStep(); // reset synth index to 0

        metronomeFlash?.Reset(); 
        synthMetronomeFlash?.Reset(); 

        isPlaying = false;
    }

    // Coroutine to sync beat flashing and audio playback
    IEnumerator BeatRoutine()
    {
        float interval = barDuration / beatCount;
        int index = 0;

            while (true)
            {
                string pattern = currentLesson.beatPattern;

                // Check if the current beat should play
                bool isActive = currentLesson.beatPattern[index] == '1';
                lessonAudioEngine.PlayBeat(isActive);

                metronomeFlash.FlashBeatAtIndex(index);

                displayHelper?.SetFlashIndex(index);

                index = (index + 1) % beatCount;
                yield return new WaitForSeconds(interval);
            }
    }

    // Coroutine to sync subdivision flashing and audio playback
    IEnumerator SubdivisionRoutine()
    {
        float interval = barDuration / subdivisionCount;
        int index = 0;

        while (true)
        {
            metronomeFlash.FlashSubdivisionAtIndex(index);
            lessonAudioEngine?.PlaySubdivision();

            index = (index + 1) % subdivisionCount;
            yield return new WaitForSeconds(interval);
        }
    }

    public int GetCurrentSubdivision()
    {
        return subdivisionCount;
    }

    public int GetCurrentBeatCount()
    {
        return beatCount;
    }

    public float GetBarDuration()
    {
        return barDuration;
    }

    // Method to deal with addition/subtraction of beats
    public void OnBeatCountChanged(int newBeatCount)
    {
        beatCount = Mathf.Clamp(newBeatCount, 1, 64);

        // Update the beat pattern to match new beat count
        if (currentLesson != null)
        {
            // Extend or truncate the pattern to match the new beat count
            string currentPattern = currentLesson.beatPattern;
            if (currentPattern.Length < beatCount)
            {
                // Add beats: extend with '1's
                currentLesson.beatPattern = currentPattern.PadRight(beatCount, '1');
            }
            else if (currentPattern.Length > beatCount)
            {
                // Remove beats: truncate
                currentLesson.beatPattern = currentPattern.Substring(0, beatCount);
            }
            
            // Update display with new pattern
            if (displayHelper != null)
            {
                displayHelper.SetBeatDigits(currentLesson.beatPattern);
                displayHelper.SetBeatCount(beatCount);
            }
        }

        barDuration = 30f * beatCount / bpm;
        beatDuration = 60f / bpm;

        sequencerRowBuilder?.BuildSequencer(1, beatCount, subdivisionCount);

        if (metronomeFlash != null)
        {
            metronomeFlash.beatRow = sequencerRowBuilder.beatRow;
            metronomeFlash.subdivisionRow = sequencerRowBuilder.subdivisionRow;
            metronomeFlash.InitializeAndStart(barDuration, beatCount, subdivisionCount);
        }

        if (synthPlaybackEngine != null)
        {
            int currentSynthSteps = synthPlaybackEngine.GetCurrentStepCount();
            synthPlaybackEngine.InitializeSynth(barDuration, currentSynthSteps);
        }

        // Restart coroutines if playing
        if (isPlaying)
        {
            if (beatRoutine != null) StopCoroutine(beatRoutine);
            if (subdivisionRoutine != null) StopCoroutine(subdivisionRoutine);
            beatRoutine = StartCoroutine(BeatRoutine());
            subdivisionRoutine = StartCoroutine(SubdivisionRoutine());
        }
    }

    //  Animated digit sequence 
    string GenerateDigitSequence(string[] beatPattern)
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

    public void RefreshDialogue()
    {
        if (verticalScroller != null && currentLesson != null)
        {
            StartCoroutine(AssignDialogueToVerticalScroller(currentLesson.dialogueText));
        }
    }

    //  Debugging method to update text in the UI
    // This is a workaround for Unity's lack of support in OSX
    public void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}

