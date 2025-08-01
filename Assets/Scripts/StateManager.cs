using TMPro;
using UnityEngine;

public enum GameState
{
    Starting,
    Playing,
    AboutToWin,
    Won,
}

public class StateManager : MonoBehaviour
{
    public Actor enemy;
    public PianoRoll pianoRoll;
    public TextMeshProUGUI countdown;
    public GameObject win;
    public float countdownLength = 3f;
    public float countdownScale = 1f;

    public static GameState State { get; private set; } = GameState.Starting;
    private float startTime = -1;
    private int lastBar;

    void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);
    }

    void NewBeat(int currentBar, int _currentBeat)
    {
        if (lastBar == currentBar) return;
        lastBar = currentBar;

        // Bar just changed, check win state

        if (State == GameState.AboutToWin)
        {
            State = GameState.Won;
            win.SetActive(true);
            // Do big enemy death animation here
            // Enable next level / menu buttons here
        }
        else
        {
            // Check win state

            Sequence enemySequence = enemy.Sequence;
            Sequence playerSequence = pianoRoll.GetSequence();

            if (enemySequence.Equals(playerSequence))
            {
                State = GameState.AboutToWin;
            }
        }
    }

    void PlayingTransition()
    {
        State = GameState.Playing;
        countdown.gameObject.SetActive(false);
        BeatManager.Get().StartRunning();
    }

    void Update()
    {
        if (startTime == -1) startTime = Time.time;

        if (State == GameState.Starting)
        {
            float elapsedTime = (Time.time - startTime) / countdownScale;
            if (elapsedTime > countdownLength)
            {
                PlayingTransition();
            }
            else
            {
                int countdownInt = (int)(countdownLength - elapsedTime);
                countdown.text = countdownInt == 0
                    ? "GO!"
                    : countdownInt.ToString();
            }
        }
    }
}
