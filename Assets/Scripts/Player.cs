using UnityEngine;

public class Player : Actor
{
    public PianoRoll pianoRoll;

    public override void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);
        Sequence = pianoRoll.GetSequence();

        // We want out listener before Actor's
        base.Start();
    }

    void NewBeat(int _currentBar, int _currentBeat)
    {
        Sequence = pianoRoll.GetSequence();
    }
}
