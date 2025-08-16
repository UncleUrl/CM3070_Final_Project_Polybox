//  Version 0.8.5
//  This is the method to play back samples.

using UnityEngine;
using TMPro;

public class SynthAudioEngine : MonoBehaviour
{
    [Header("Assign Synth Sample Libraries")]
    public AudioClip[] synthLeadSamples;  // Assign SynthLead samples in inspector (Library 1)
    public AudioClip[] synthPadSamples;   // Assign SynthPad samples in inspector (Library 2)

    public AudioClip[] noSynthSamples = new AudioClip[0]; // Assign  samples in inspector (Silence)

    public bool HasSamples => currentSamples != null && currentSamples != noSynthSamples;

    public bool IsNoSynth => currentSynthVoiceIndex == 0;

    public bool isNoSamples { get; private set; } = true;

    public GameObject nullSynthButtonPrefab;

    public SequencerRowBuilder sequencerRowBuilder;

    public GameObject synthButtonPrefab;

    public AudioClip[] currentSamples;

    private AudioSource[] audioSources;

    public TextMeshProUGUI debugText;

    void Awake()
    {
        currentSamples = synthLeadSamples; // default
        

        InitializeAudioSources();
    }
    

public void InitializeAudioSources()
    {
        var existingSources = GetComponents<AudioSource>();
        foreach (var src in existingSources)
            Destroy(src);

        int count = (currentSamples != null) ? currentSamples.Length : 0;
        audioSources = new AudioSource[count];

        for (int i = 0; i < count; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].playOnAwake = false;
            audioSources[i].spatialBlend = 0f; // 2D sound
        }

        UpdateDebugText($"Initialized {count} synth sources.");
    }

    // Play a note by its index in the samples array
    public void PlayNote(int index)
    {
        if (currentSamples == null || index < 0 || index >= currentSamples.Length)
        {
            UpdateDebugText("PlayNote skipped: invalid index or missing samples.");
            return;
        }

        AudioClip clip = currentSamples[index];
        if (clip != null && audioSources != null && audioSources.Length > index && audioSources[index] != null)
        {
            audioSources[index].PlayOneShot(clip);
            UpdateDebugText($"Playing synth note {index}: {clip.name}");
        }
        else
        {
            UpdateDebugText($"No clip or audioSource found for index {index}");
        }
    }

    // Play a note by its name (used by the sequencer)
    public void PlayNote(string noteName)
    {
        if (currentSamples == null || currentSamples.Length == 0)
        {
            UpdateDebugText("PlayNote skipped: no samples loaded.");
            return;
        }

        for (int i = 0; i < currentSamples.Length; i++)
        {
            if (currentSamples[i] != null && currentSamples[i].name == noteName)
            {
                PlayNote(i); 
                return;
            }
        }

        UpdateDebugText($"No sample found for note '{noteName}'");
    }

    public void SwitchSynthLibrary(int index)
    {
        currentSynthVoiceIndex = index;

        switch (index)
        {
            case 0:
                currentSamples = noSynthSamples;

                if (sequencerRowBuilder != null)
                {
                    sequencerRowBuilder.SetSynthButtonPrefab(nullSynthButtonPrefab);
                    sequencerRowBuilder.ToggleSynthRowVisibility(false); 
                }

                UpdateAudioSources();
                UpdateDebugText("Switched to No Synth");
                break;

            case 1:
                currentSamples = synthLeadSamples;

                if (sequencerRowBuilder != null)
                {
                    sequencerRowBuilder.SetSynthButtonPrefab(synthButtonPrefab);
                    sequencerRowBuilder.ToggleSynthRowVisibility(true); 
                }

                UpdateAudioSources();
                UpdateDebugText("Switched to SynthLead");
                break;

            case 2:
                currentSamples = synthPadSamples;

                if (sequencerRowBuilder != null)
                {
                    sequencerRowBuilder.SetSynthButtonPrefab(synthButtonPrefab);
                    sequencerRowBuilder.ToggleSynthRowVisibility(true); 
                }

                UpdateAudioSources();
                UpdateDebugText("Switched to SynthPad");
                break;

            default:
                currentSamples = noSynthSamples;

                if (sequencerRowBuilder != null)
                {
                    sequencerRowBuilder.SetSynthButtonPrefab(nullSynthButtonPrefab);
                    sequencerRowBuilder.ToggleSynthRowVisibility(false);
                }

                UpdateAudioSources();
                UpdateDebugText("Switched to No Synth");
                break;
        }
    }


    void UpdateAudioSources()
    {
        // Remove existing AudioSources
        var existingSources = GetComponents<AudioSource>();
        foreach (var src in existingSources)
        {
            Destroy(src);
        }

        InitializeAudioSources();
    }

    public void SetMuted(bool mute)
    {
        AudioListener.volume = mute ? 0f : 1f;
    }

    private int currentSynthVoiceIndex = 0; // 0 = off, 1 = lead, 2 = pad

    public int GetCurrentSynthVoiceIndex()
    {
        return currentSynthVoiceIndex;
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
