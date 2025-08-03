using TMPro;
using UnityEngine;

public class Player : Actor
{
    public PianoRoll pianoRoll;
    public TextMeshProUGUI ammoText;

    public int initialAmmo = 1;
    private int ammo = 1;
    public int Ammo
    {
        get => ammo;
        set
        {
            ammo = value;
            ammoText.text = ammo.ToString().PadLeft(2, '0');
        }
    }

    private bool isDead = false;

    public override void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);
        Sequence = pianoRoll.GetSequence();
        Ammo = initialAmmo;

        // We want our listener before Actor's
        base.Start();
    }

    void NewBeat(int _currentBar, int currentBeat)
    {
        Sequence = pianoRoll.GetSequence();
        if (currentBeat == 1)
        {
            Ammo = initialAmmo;
            isDead = false;
        }
    }

    protected override void Shoot()
    {
        isShielding = false;
        if (Ammo <= 0) return;
        base.Shoot();
        Ammo -= 1;
    }

    protected override void Charge()
    {
        base.Charge();
        Ammo += 1;
    }

    public override bool Hit(bool isFromEnemy)
    {
        if (!base.Hit(isFromEnemy)) return false;

        if (isDead == false) Die();
        isDead = true;

        return true;
    }
}
