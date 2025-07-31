using TMPro;
using UnityEngine;

public class PianoRoll : MonoBehaviour
{
    public float padding = 1;

    public GameObject square;
    public GameObject rowLabel;

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

        GetComponent<Transform>().position = new(-xHalfSize + 4.0f, 0, 0);

        for (int y = 0; y < instruments.Length; y++)
        {
            float yPos = (ySize + padding) * y;

            GameObject newRowLabel = Instantiate(rowLabel, transform);
            newRowLabel.GetComponent<RectTransform>().pivot = new(1f, 0.5f);
            newRowLabel.GetComponent<RectTransform>().localPosition = new(
                -1.5f,
                yPos,
                0
            );
            newRowLabel.GetComponent<TextMeshPro>().text = instruments[y].playerMove
                + "\n" + instruments[y].source.name;

            for (int x = 0; x < beatManager.beatCount; x++)
            {
                GameObject newSquare = Instantiate(square, transform);
                newSquare.transform.localPosition = new(
                    (xSize + padding) * x,
                    yPos,
                    0
                );

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
