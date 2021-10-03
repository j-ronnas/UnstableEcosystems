using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource source;


    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        
    }

    public void SetSound(bool b)
    {
        source.mute = !b;
    }

    public void PlayPop()
    {
        if (!source.isPlaying)
        {
            source.Play();
        }
       
        
        
    }
}
