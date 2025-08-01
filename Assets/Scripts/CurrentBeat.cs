using UnityEngine;

public class CurrentBeat : MonoBehaviour
{
    private float xOffset;

    void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);

        PianoRoll pianoRoll = gameObject.GetComponentInParent<PianoRoll>();
        float squareWidth = pianoRoll.square.GetComponent<Renderer>().bounds.size.x;
        float squareHeight = pianoRoll.square.GetComponent<Renderer>().bounds.size.y;

        float padding = (transform.localScale.x - squareWidth) / 2f;

        int count = pianoRoll.instruments.Length;
        float height = count * (squareHeight + pianoRoll.padding) - pianoRoll.padding + 2 * padding;
        transform.localScale = new(transform.localScale.x, height, transform.localScale.z);
        transform.localPosition = new(
            transform.localPosition.x,
            (count - 1) * (squareHeight + pianoRoll.padding) / 2f,
            transform.localPosition.z
        );

        xOffset = squareWidth + pianoRoll.padding;

        NewBeat(1, 1);
    }

    void NewBeat(int _currentBar, int currentBeat)
    {
        transform.localPosition = new(
            xOffset * (currentBeat - 1),
            transform.localPosition.y,
            transform.localPosition.z
        );
    }
}
