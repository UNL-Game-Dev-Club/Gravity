using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitchPlaySound : MonoBehaviour
{
    AudioSource sfxPlayer;
    void Start()
    {
        sfxPlayer = GetComponent<AudioSource>();
        sfxPlayer.pitch = Random.Range(0.8f, 1.2f);
    }

    public void PlayDeathSound()
    {
        sfxPlayer.Play();
    }
}
