using UnityEngine;

public class Metronome : MonoBehaviour
{
    public AudioSource firstBeat;
    public AudioSource beat;

    void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);
    }

    void NewBeat(int _currentBar, int currentBeat)
    {
        if (currentBeat == 1)
        {
            firstBeat.Play();
        }
        else
        {
            beat.Play();
        }
    }
}
