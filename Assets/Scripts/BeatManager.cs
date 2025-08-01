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
    [HideInInspector]
    public int CurrentBeat { get; private set; } = 0;
    [HideInInspector]
    public int CurrentBar { get; private set; } = 1;

    private bool running = false;
    private float beatLength;
    private float barLength;
    private float startTime;
    private readonly List<Action<int, int>> listeners = new();

    public void RegisterListener(Action<int, int> action)
    {
        listeners.Add(action);
    }

    public void StartRunning()
    {
        running = true;
        startTime = Time.time;
    }

    void Start()
    {
        instance = this;

        beatLength = 60f / bpm;
        barLength = beatCount * beatLength;
    }

    void PlayBeat()
    {
        foreach (var action in listeners)
        {
            action(CurrentBar, CurrentBeat);
        }
    }

    public float GetBeatFloat()
    {
        float currentTime = Time.time - startTime;
        float timeInBar = currentTime % barLength;
        float beat = 1f + timeInBar / beatLength;
        return beat;
    }

    void Update()
    {
        if (!running) return;

        float currentTime = Time.time - startTime;
        int bar = 1 + (int)(currentTime / barLength);

        if (bar == CurrentBar)
        {
            int beat = (int)GetBeatFloat();

            if (beat != CurrentBeat)
            {
                CurrentBeat = beat;
                PlayBeat();
            }
        }
        else
        {
            CurrentBeat = 1;
            CurrentBar = bar;
            PlayBeat();
        }
    }
}
