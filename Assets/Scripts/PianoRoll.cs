using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PianoRoll : MonoBehaviour
{
    public float padding = 1;

    public GameObject square;
    public GameObject rowLabel;
    public GameObject rowKey;

    public InputAction clearAction;

    [Range(0f, 1f)]
    public float earlyAllow = 0.2f;
    [Range(0f, 1f)]
    public float lateAllow = 0.4f;

    public Note[] instruments;
    public AudioMixerGroup mixer;
    private AudioSource[] sources;

    private NoteBtn[,] noteBtns;
    private bool[,] notes;
    bool GetNote(int note, int beat) => notes[note, beat - 1];
    void SetNote(int note, int beat, bool value)
    {
        notes[note, beat - 1] = value;
        noteBtns[note, beat - 1].IsEnabled = value;
    }

    void Start()
    {
        clearAction.Enable();

        BeatManager beatManager = BeatManager.Get();
        beatManager.RegisterListener(PlayBeat);
        notes = new bool[instruments.Length, beatManager.beatCount];
        noteBtns = new NoteBtn[instruments.Length, beatManager.beatCount];

        float xSize = square.GetComponent<Renderer>().bounds.size.x;
        float ySize = square.GetComponent<Renderer>().bounds.size.y;

        float yHalfSize = Camera.main.orthographicSize;
        float xHalfSize = yHalfSize * Camera.main.aspect;

        GetComponent<Transform>().position = new(
            -xHalfSize + 5.0f,
            -yHalfSize + 1.0f,
            0
        );

        for (int y = 0; y < instruments.Length; y++)
        {
            float yPos = (ySize + padding) * y;

            GameObject newRowLabel = Instantiate(rowLabel, transform);
            newRowLabel.GetComponent<RectTransform>().pivot = new(1f, 0.5f);
            newRowLabel.GetComponent<RectTransform>().localPosition = new(
                -3.25f,
                yPos,
                0
            );
            newRowLabel.GetComponent<TextMeshPro>().text = instruments[y].source.name;
            newRowLabel.GetComponentInChildren<SpriteRenderer>().sprite = instruments[y].icon;

            GameObject newRowKey = Instantiate(rowKey, transform);
            newRowKey.GetComponent<RectTransform>().pivot = new(1f, 0.5f);
            newRowKey.GetComponent<RectTransform>().localPosition = new(
                -0.75f,
                yPos,
                0
            );
            newRowKey.GetComponent<TextMeshPro>().text = instruments[y]
                .inputAction.GetBindingDisplayString();

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
            source.outputAudioMixerGroup = mixer;
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
        if (StateManager.State == GameState.Starting) return;
        if (StateManager.State == GameState.Won) return;

        BeatManager beatManager = BeatManager.Get();

        float beatFloat = beatManager.GetBeatFloat();
        int beat = (int)beatFloat;

        // If we are at the end of a beat
        if (1 + beat - beatFloat <= earlyAllow)
        {
            beat += 1;
            if (beat > beatManager.beatCount) beat = 1;
        }
        // If we not at the start of a beat
        else if (beatFloat - beat > lateAllow)
        {
            return;
        }

        if (clearAction.WasPressedThisFrame())
        {
            ClearBeat(beat);
            return;
        }

        for (int i = 0; i < instruments.Length; i++)
        {
            Note note = instruments[i];

            if (note.inputAction.WasPressedThisFrame())
            {
                ClearBeat(beat);
                SetNote(i, beat, true);
            }
        }
    }

    public Sequence GetSequence()
    {
        int beatCount = BeatManager.Get().beatCount;

        Sequence sequence = ScriptableObject.CreateInstance<Sequence>();
        sequence.notes = new Note[beatCount];

        for (int i = 0; i < sequence.notes.Length; i++)
        {
            sequence.notes[i] = null;
        }

        for (int b = 0; b < beatCount; b++)
        {
            for (int i = 0; i < instruments.Length; i++)
            {
                if (notes[i, b])
                {
                    sequence.notes[b] = instruments[i];
                }
            }
        }

        return sequence;
    }
}
