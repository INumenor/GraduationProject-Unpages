using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicSlider;

    const string effectMusic = "Effect";
    const string environmentMusic = "Environment";

    void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }
    void SetMusicVolume(float value)
    {
        mixer.SetFloat(effectMusic,value);
    }
    void SetEnvironmentVolume(float value)
    {
        mixer.SetFloat(environmentMusic, value);
    }
}
