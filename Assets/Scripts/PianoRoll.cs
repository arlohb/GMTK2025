using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class PianoRoll : MonoBehaviour
{
    public int beatCount = 8;
    public float padding = 1;

    public int bpm = 100;
    private float beatLength;
    private float barLength;
    private int currentBeat = 0;
    private int currentBar = 1;
    private float startTime;

    public GameObject square;

    public AudioResource[] instruments;
    private AudioSource[] sources;

    private bool[,] notes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        notes = new bool[instruments.Length, beatCount];

        float xSize = square.GetComponent<Renderer>().bounds.size.x;
        float ySize = square.GetComponent<Renderer>().bounds.size.y;

        for (int y = 0; y < instruments.Length; y++)
        {
            for (int x = 0; x < beatCount; x++)
            {
                Vector3 position = new(
                    (xSize + padding) * x,
                    (ySize + padding) * y,
                    0
                );

                GameObject newSquare = Instantiate(square, position, Quaternion.identity);

                Note note = newSquare.GetComponent<Note>();
                note.Setup(y, x, SetNote);
            }
        }

        sources = new AudioSource[instruments.Length];
        for (int i = 0; i < instruments.Length; i++) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.resource = instruments[i];
            sources[i] = source;
        }

        startTime = Time.time;
        beatLength = 60f / bpm;
        barLength = beatCount * beatLength;
    }

    void PlayBeat()
    {
        Debug.Log("Playing beat " + currentBar + "-" + currentBeat);

        for (int i = 0; i < instruments.Length; i++)
        {
            if (!notes[i, currentBeat - 1]) continue;

            sources[i].Play();
        }
    }

    void UpdateBeat()
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

    void Update()
    {
        UpdateBeat();
    }

    void SetNote(int instrument, int beat, bool value)
    {
        notes[instrument, beat] = value;
    }
}
