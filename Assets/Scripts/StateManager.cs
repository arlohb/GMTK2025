using TMPro;
using UnityEngine;

public enum GameState
{
    Starting,
    Playing
}

public class StateManager : MonoBehaviour
{
    public Enemy enemy;
    public PianoRoll pianoRoll;
    public TextMeshProUGUI countdown;
    public float countdownLength = 3f;
    public float countdownScale = 1f;

    private GameState state = GameState.Starting;
    private float startTime = -1;
    private int lastBar;

    void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);

        /* Time.timeScale = 0f; */
    }

    void NewBeat(int currentBar, int _currentBeat)
    {
        if (lastBar == currentBar) return;
        lastBar = currentBar;

        // Bar just changed, check win state

        Sequence enemySequence = enemy.sequence;
        Sequence playerSequence = pianoRoll.GetSequence();

        if (enemySequence.Equals(playerSequence))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    void PlayingTransition()
    {
        state = GameState.Playing;
        Time.timeScale = 1f;
        countdown.gameObject.SetActive(false);
        BeatManager.Get().StartRunning();
    }

    void Update()
    {
        if (startTime == -1) startTime = Time.unscaledTime;

        if (state == GameState.Starting)
        {
            float elapsedTime = (Time.unscaledTime - startTime) / countdownScale;
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
