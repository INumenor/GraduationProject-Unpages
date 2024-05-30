using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider environmentSlider;

    public const string effectMusic = "Effect";
    public const string environmentMusic = "Environment";

    void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        environmentSlider.onValueChanged.AddListener(SetEnvironmentVolume);
    }
    void SetMusicVolume(float value)
    {
        mixer.SetFloat(effectMusic, Mathf.Log10(value) * 20);
    }
    void SetEnvironmentVolume(float value)
    {
        mixer.SetFloat(environmentMusic, Mathf.Log10(value) * 20);
    }
}