using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoop : MonoBehaviour
{
    public float bpm = 121.0f;
    public int numBeatsPerSegment = 24;

    private double nextEventTime;
    private bool running = false;
    private AudioSource audio;
    public AudioClip start;
    public AudioClip loop;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = start;
        audio.loop = false;
        nextEventTime = AudioSettings.dspTime;
        running = true;
    }

    void Update()
    {
        if (!running)
        {
            return;
        }

        double time = AudioSettings.dspTime;

        if (time + 1.0f > nextEventTime)
        {
            audio.clip = loop;
            audio.loop = true;
            audio.PlayScheduled(nextEventTime);
            nextEventTime += 60.0f / bpm * numBeatsPerSegment;
            running = false;
        }
    }
}
