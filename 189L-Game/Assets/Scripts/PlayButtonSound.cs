using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip buttonClickSound;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonSoundClip()
    {
        if (audioSource != null) 
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
