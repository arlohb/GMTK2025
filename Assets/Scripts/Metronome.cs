using UnityEngine;
using UnityEngine.Audio;

public class Metronome : MonoBehaviour
{
    public AudioMixerGroup pitchBendMixer;
    public AudioSource firstBeat;
    public AudioSource beat;
    public AudioSource background;

    void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);

        float bpm = BeatManager.Get().bpm;
        if (bpm != 100f)
        {
            float scale = bpm / 100f;

            // Bit of a hack to speed up audio without changing the pitch
            // https://discussions.unity.com/t/how-i-can-change-the-speed-of-a-song-or-sound/6623/6
            background.pitch = scale;
            pitchBendMixer.audioMixer.SetFloat("PitchBend", 1f / scale);
        }
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
