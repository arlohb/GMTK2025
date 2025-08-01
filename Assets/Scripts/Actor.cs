using System.Collections.Generic;
using System.Linq;
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

        Move move = note == null
            ? Move.None
            : isEnemy ? note.enemyMove : note.playerMove;

        text.SetText(move.ToString());

        GameObject sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        List<Transform> children = sprite.GetComponentsInChildren<Transform>(true)
            .Where(t => t.name != sprite.name)
            .ToList();

        children
            .Where(t => t.name == "Shield")
            .ToList()
            .ForEach(t => t.gameObject.SetActive(move == Move.Shield));
    }
}
