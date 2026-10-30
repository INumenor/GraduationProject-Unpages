using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider generalSoundSlider;
    public Slider musicSlider;
    public Slider environmentSlider;

    public GameObject muteButton; 
    public GameObject unMuteButton; 

    public const string effectMusic = "Effect";
    public const string environmentMusic = "Environment";
    public const string generalSoundMusic = "Master";

    void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        environmentSlider.onValueChanged.AddListener(SetEnvironmentVolume);
        generalSoundSlider.onValueChanged.AddListener(SetGeneralMusicVolume);
    }
    void SetGeneralMusicVolume(float value)
    {
        mixer.SetFloat(generalSoundMusic, ToDecibel(value));
    }
    void SetMusicVolume(float value)
    {
        mixer.SetFloat(effectMusic, ToDecibel(value));
    }
    void SetEnvironmentVolume(float value)
    {
        mixer.SetFloat(environmentMusic, ToDecibel(value));
    }
    // Log10(0) is -Infinity, which the mixer rejects; clamp to the -80 dB floor.
    static float ToDecibel(float value) => Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;

    public void MuteMusic()
    {
        mixer.SetFloat(generalSoundMusic, -80);
        muteButton.SetActive(false);
        unMuteButton.SetActive(true);
    }
    public void UnMuteMusic()
    {
        mixer.SetFloat(generalSoundMusic, 0);
        muteButton.SetActive(true);
        unMuteButton.SetActive(false);
    }
}