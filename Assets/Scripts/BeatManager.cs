using UnityEngine;
using System;
using System.Collections.Generic;

public class BeatManager : MonoBehaviour
{
    static BeatManager instance;

    public static BeatManager Get()
    {
        return instance;
    }

    public int beatCount = 8;
    public int bpm = 100;
    private float beatLength;
    private float barLength;
    private int currentBeat = 0;
    private int currentBar = 1;
    private float startTime;
    private readonly List<Action<int, int>> listeners = new();

    public void RegisterListener(Action<int, int> action)
    {
        listeners.Add(action);
    }

    void Start()
    {
        instance = this;

        startTime = Time.time;
        beatLength = 60f / bpm;
        barLength = beatCount * beatLength;
    }

    void PlayBeat()
    {
        Debug.Log("Playing beat " + currentBar + "-" + currentBeat);

        foreach (var action in listeners)
        {
            action(currentBar, currentBeat);
        }
    }

    void Update()
    {
        float currentTime = Time.time - startTime;
        int bar = 1 + (int)(currentTime / barLength);

        if (bar == currentBar)
        {
            float timeInBar = currentTime % barLength;
            int beat = 1 + (int)(timeInBar / beatLength);

            if (beat != currentBeat)
            {
                currentBeat = beat;
                PlayBeat();
            }
        }
        else
        {
            currentBeat = 1;
            currentBar = bar;
            PlayBeat();
        }
    }
}
