using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip introTune;
    [SerializeField] List<AudioClip> gameMusic = new List<AudioClip>();
    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayIntroTune(bool play)
    {
        if (play)
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(introTune);
        }
        else
        {
            if (audioSource.clip == introTune)
            {
                audioSource.Stop();
            }
        }
    }
}
