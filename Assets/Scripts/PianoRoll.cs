using UnityEngine;

public class PianoRoll : MonoBehaviour
{
    public float padding = 1;

    public GameObject square;

    public Note[] instruments;
    private AudioSource[] sources;

    private bool[,] notes;

    void Start()
    {
        BeatManager beatManager = BeatManager.Get();
        beatManager.RegisterListener(PlayBeat);
        notes = new bool[instruments.Length, beatManager.beatCount];

        float xSize = square.GetComponent<Renderer>().bounds.size.x;
        float ySize = square.GetComponent<Renderer>().bounds.size.y;

        float yHalfSize = Camera.main.orthographicSize;
        float xHalfSize = yHalfSize * Camera.main.aspect;

        GetComponent<Transform>().position = new(-xHalfSize + 1.5f, 0, 0);

        for (int y = 0; y < instruments.Length; y++)
        {
            for (int x = 0; x < beatManager.beatCount; x++)
            {
                Vector3 position = new(
                    (xSize + padding) * x,
                    (ySize + padding) * y,
                    0
                );

                GameObject newSquare = Instantiate(
                    square,
                    gameObject.transform
                );
                newSquare.transform.localPosition = position;

                NoteBtn note = newSquare.GetComponent<NoteBtn>();
                note.Setup(y, x, SetNote);
            }
        }

        sources = new AudioSource[instruments.Length];
        for (int i = 0; i < instruments.Length; i++) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.resource = instruments[i].source;
            sources[i] = source;
        }

    }

    void PlayBeat(int _currentBar, int currentBeat)
    {
        for (int i = 0; i < instruments.Length; i++)
        {
            if (!notes[i, currentBeat - 1]) continue;

            sources[i].Play();
        }
    }

    void Update()
    {
    }

    void SetNote(int instrument, int beat, bool value)
    {
        notes[instrument, beat] = value;
    }
}
