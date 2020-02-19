using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioLoop : MonoBehaviour
{

    public AudioClip start;
    public AudioClip loop;

    private AudioClip[] clips;
    private double nextEventTime;
    private int flip = 0;
    private bool looping;
    private AudioSource[] audioSources = new AudioSource[2];
    private bool running = false;
    private void Awake()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TitleMusic"))
        {
            Destroy(obj);
        }

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


    void Start()
    {
        //gameObject.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        clips = new AudioClip[] { start, loop };
        for (int i = 0; i < 2; i++)
        {
            GameObject child = new GameObject("MusicBox");
            child.transform.parent = gameObject.transform;
            audioSources[i] = child.AddComponent<AudioSource>();
            audioSources[i].volume = .9f;
        }
#if UNITY_WEBGL
        nextEventTime = AudioSettings.dspTime + 1.0f;
#else
        nextEventTime = AudioSettings.dspTime;
#endif
        running = true;
        looping = false;
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
            audioSources[flip].clip = clips[flip];
            audioSources[flip].PlayScheduled(nextEventTime);

            nextEventTime += clips[flip].length;
            flip = 1 - flip;
            if(!looping)
            {
                clips[0] = clips[1];
                looping = true;
            }
        }
    }
}
