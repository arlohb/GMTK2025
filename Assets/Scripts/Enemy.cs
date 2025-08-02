using UnityEngine;

public class Enemy : Actor
{
    public Sequence enemySequence;
    public Transform healthForeground;
    public int initialHealth = 1;

    private int health;
    private int Health
    {
        get => health;
        set
        {
            health = value;
            float percent = (float)health / initialHealth;

            healthForeground.localScale = new(
                percent,
                healthForeground.localScale.y,
                healthForeground.localScale.z
            );
            healthForeground.localPosition = new(
                -(1f - percent) / 2f,
                healthForeground.localPosition.y,
                healthForeground.localPosition.z
            );
        }
    }

    public override void Start()
    {
        base.Start();
        Sequence = enemySequence;
        isEnemy = true;

        BeatManager.Get().RegisterListener(NewBeat);
    }

    private void NewBeat(int _currentBar, int currentBeat)
    {
        // Only run at start of bar
        if (currentBeat != 1) return;

        Health = initialHealth;
    }

    public override bool Hit(bool isFromEnemy)
    {
        if (!base.Hit(isFromEnemy)) return false;

        Health -= 1;

        return true;
    }
}
