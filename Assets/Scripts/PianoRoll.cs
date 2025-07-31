using UnityEngine;

public class PianoRoll : MonoBehaviour
{
    public int rows = 3;
    public int beatCount = 8;
    public float padding = 1;

    public GameObject square;

    private bool[,] notes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        notes = new bool[rows, beatCount];

        float xSize = square.GetComponent<Renderer>().bounds.size.x;
        float ySize = square.GetComponent<Renderer>().bounds.size.y;

        for (int y = 0; y < rows; y++)
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetNote(int instrument, int beat, bool value)
    {
        notes[instrument, beat] = value;
    }
}
