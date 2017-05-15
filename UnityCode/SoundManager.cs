using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    Dictionary<string, AudioClip> audioClips;

    //public AudioClip[] audioClip;
    AudioSource audioSource;

    


    public static SoundManager instance;

    void Awake()
    {
        if (SoundManager.instance == null)
            SoundManager.instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundName)
    {
        audioSource.PlayOneShot(audioClips[soundName]);
    }
    
    public void AddSound(string soundName, AudioClip audioClip)
    {
        if (audioClips[soundName] != null)
            return;

        audioClips.Add(soundName, audioClip);
    }
}
