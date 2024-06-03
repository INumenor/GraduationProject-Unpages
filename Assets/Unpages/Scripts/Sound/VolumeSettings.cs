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
        mixer.SetFloat(generalSoundMusic, Mathf.Log10(value) * 20);
    }
    void SetMusicVolume(float value)
    {
        mixer.SetFloat(effectMusic, Mathf.Log10(value) * 20);
    }
    void SetEnvironmentVolume(float value)
    {
        mixer.SetFloat(environmentMusic, Mathf.Log10(value) * 20);
    }

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