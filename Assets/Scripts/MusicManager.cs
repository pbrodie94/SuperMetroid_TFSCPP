using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudio;

    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip bossMusic;

    public void TransitionMusic(int music)
    {
        switch (music)
        {
            case 0:

                musicAudio.clip = levelMusic;
                musicAudio.loop = true;
                musicAudio.Play();

                break;

            case 1:

                musicAudio.clip = bossMusic;
                musicAudio.loop = true;
                musicAudio.Play();

                break;

            default:

                musicAudio.clip = levelMusic;
                musicAudio.loop = true;
                musicAudio.Play();

                break;
        }
    }
}
