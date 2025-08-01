using UnityEngine;

public class Metronome : MonoBehaviour
{
    public AudioSource firstBeat;
    public AudioSource beat;
    public AudioSource background;

    void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);
    }

    void NewBeat(int _currentBar, int currentBeat)
    {
        if ((currentBeat - 1) % 4 == 0) background.Play();

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
