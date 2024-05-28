using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOneShotPlay : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
    // Start is called before the first frame update
    public void PlaySound()
    {
        audioSource.PlayOneShot(clip,1f);
    }
}
