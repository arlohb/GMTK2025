using TMPro;
using UnityEngine;

public class PianoRoll : MonoBehaviour
{
    public float padding = 1;

    public GameObject square;
    public GameObject rowLabel;

    [Range(0f, 1f)]
    public float earlyAllow = 0.2f;
    [Range(0f, 1f)]
    public float lateAllow = 0.4f;

    public Note[] instruments;
    private AudioSource[] sources;

    private NoteBtn[,] noteBtns;
    private bool[,] notes;
    bool GetNote(int note, int beat) => notes[note, beat - 1];
    void SetNote(int note, int beat, bool value) {
        notes[note, beat - 1] = value;
        noteBtns[note, beat - 1].IsEnabled = value;
    }

    void Start()
    {
        BeatManager beatManager = BeatManager.Get();
        beatManager.RegisterListener(PlayBeat);
        notes = new bool[instruments.Length, beatManager.beatCount];
        noteBtns = new NoteBtn[instruments.Length, beatManager.beatCount];

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

                NoteBtn noteBtn = newSquare.GetComponent<NoteBtn>();
                noteBtn.Setup(y, x + 1, SetNote);
                noteBtns[y, x] = noteBtn;
            }
        }

        sources = new AudioSource[instruments.Length];
        for (int i = 0; i < instruments.Length; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.resource = instruments[i].source;
            sources[i] = source;

            instruments[i].inputAction.Enable();
        }

    }

    void PlayBeat(int _currentBar, int currentBeat)
    {
        for (int i = 0; i < instruments.Length; i++)
        {
            if (!GetNote(i, currentBeat)) continue;

            sources[i].Play();
        }
    }

    void ClearBeat(int beat)
    {
        for (int i = 0; i < instruments.Length; i++)
        {
            SetNote(i, beat, false);
        }
    }

    void Update()
    {
        BeatManager beatManager = BeatManager.Get();

        for (int i = 0; i < instruments.Length; i++)
        {
            Note note = instruments[i];

            if (note.inputAction.WasPressedThisFrame())
            {
                Debug.Log(note.name);
                float beatFloat = beatManager.GetBeatFloat();
                int beatInt = (int)beatFloat;

                if (beatFloat - beatInt <= lateAllow)
                {
                    ClearBeat(beatInt);
                    SetNote(i, beatInt, true);
                }
                else if (1 + beatInt - beatFloat <= earlyAllow)
                {
                    beatInt += 1;
                    if (beatInt > beatManager.beatCount) beatInt = 1;

                    ClearBeat(beatInt);
                    SetNote(i, beatInt, true);
                }
            }
        }
    }
}
