using TMPro;
using UnityEngine;

public enum EnemyMove
{
    None,
    Shoot,
    Shield,
}

public class Enemy : MonoBehaviour
{
    public Sequence sequence;

    void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);
    }

    void NewBeat(int _currentBar, int currentBeat)
    {
        Note note = sequence.GetNote(currentBeat);
        GetComponentInChildren<TextMeshPro>().SetText(note == null
            ? "none"
            : note.enemyMove.ToString());
    }
}
