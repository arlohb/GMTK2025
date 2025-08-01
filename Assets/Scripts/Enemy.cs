using UnityEngine;

public class Enemy : Actor
{
    public Sequence enemySequence;

    public override void Start()
    {
        base.Start();
        Sequence = enemySequence;
        isEnemy = true;
    }
}
