using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip mainMenuSong;

    void Start()
    {
        if (!audioSource.isPlaying)
        {
            PlayMainMenuSong(true);
        }
    }
    public void PlayMainMenuSong(bool play)
    {
        if (play)
        {
            audioSource.PlayOneShot(mainMenuSong);
        }
        else
        {
            audioSource.Stop();
        }

    }
}
