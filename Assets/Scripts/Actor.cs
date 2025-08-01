using TMPro;
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected bool isEnemy = false;
    public Sequence Sequence { get; protected set; }

    private TextMeshPro text;

    public virtual void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);
        text = GetComponentInChildren<TextMeshPro>();
    }

    void NewBeat(int _currentBar, int currentBeat)
    {
        Note note = Sequence.GetNote(currentBeat);

        if (note == null)
        {
            text.SetText("none");
            return;
        }

        Move move = isEnemy ? note.enemyMove : note.playerMove;
        text.SetText(move.ToString());
    }
}
