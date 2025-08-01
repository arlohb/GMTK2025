using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected bool isEnemy = false;
    public Sequence Sequence { get; protected set; }
    public GameObject bullet;

    public virtual void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);
    }

    void NewBeat(int _currentBar, int currentBeat)
    {
        Note note = Sequence.GetNote(currentBeat);

        Move move = note == null
            ? Move.None
            : isEnemy ? note.enemyMove : note.playerMove;

        DoMove(move);
    }

    protected virtual void Shoot()
    {
        GameObject sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        List<Transform> children = sprite.GetComponentsInChildren<Transform>(true)
            .Where(t => t.name != sprite.name)
            .ToList();

        children
            .Where(t => t.name == "BulletOrigin")
            .ToList()
            .ForEach(t =>
            {
                Instantiate(bullet, t.position, t.rotation);
            });
    }

    protected virtual void Shield(bool isShield)
    {
        GameObject sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        List<Transform> children = sprite.GetComponentsInChildren<Transform>(true)
            .Where(t => t.name != sprite.name)
            .ToList();

        children
            .Where(t => t.name == "Shield")
            .ToList()
            .ForEach(t => t.gameObject.SetActive(isShield));
    }
    
    protected virtual void Charge() {}

    protected void DoMove(Move move)
    {
        Shield(move == Move.Shield);

        if (move == Move.Shoot) Shoot();
        else if (move == Move.Charge) Charge();
    }
}
