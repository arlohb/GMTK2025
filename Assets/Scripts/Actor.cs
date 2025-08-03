using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected bool isEnemy = false;
    public Sequence Sequence { get; protected set; }
    public GameObject bullet;

    // Whether we've been hit this beat
    // As we can only be hit once per beat,
    // even though multiple bullets might hit us
    private bool hasBeenHit = false;
    protected bool isShielding = false;
    protected bool isDead = false;

    public virtual void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);
    }

    void NewBeat(int _currentBar, int currentBeat)
    {
        hasBeenHit = false;

        if (currentBeat == 1)
        {
            isDead = false;
            GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }

        Note note = Sequence.GetNote(currentBeat);

        Move move = note == null
            ? Move.None
            : isEnemy ? note.enemyMove : note.playerMove;

        DoMove(move);
    }

    protected virtual void Shoot()
    {
        isShielding = false;
        GameObject sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        List<Transform> children = sprite.GetComponentsInChildren<Transform>(true)
            .Where(t => t.name != sprite.name)
            .ToList();

        children
            .Where(t => t.name == "BulletOrigin")
            .ToList()
            .ForEach(t =>
            {
                GameObject newBullet = Instantiate(bullet, t.position, t.rotation);
                newBullet.GetComponent<Bullet>().isFromEnemy = isEnemy;
            });
    }

    protected virtual void Shield(bool isShield)
    {
        isShielding = true;
        GameObject sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        List<Transform> children = sprite.GetComponentsInChildren<Transform>(true)
            .Where(t => t.name != sprite.name)
            .ToList();

        children
            .Where(t => t.name == "Shield")
            .ToList()
            .ForEach(t => t.gameObject.SetActive(isShield));
    }

    protected virtual void Charge()
    {
        isShielding = false;
    }

    protected void DoMove(Move move)
    {
        if (isDead) return;

        Shield(move == Move.Shield);

        if (move == Move.Shoot) Shoot();
        else if (move == Move.Charge) Charge();
        else if (move == Move.None) isShielding = false;
    }

    public virtual bool Hit(bool isFromEnemy)
    {
        // If this is our own bullet, ignore hit
        if (isFromEnemy == isEnemy) return false;

        // If we've already been hit, ignore hit
        if (hasBeenHit) return false;

        hasBeenHit = true;
        return true;
    }

    protected void Die()
    {
        if (!isShielding)
        {
            isDead = true;
            GetComponentInChildren<Animator>().Play("Explosion");
            GetComponentInChildren<SpriteRenderer>().color = Color.gray;
        }
    }
}
