using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public TextMeshProUGUI endBarText;
    public float countdownLength = 3f;
    public float countdownScale = 1f;

    public static GameState State { get; private set; } = GameState.Starting;
    private float startTime = -1;
    private int lastBar;

    void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);
        State = GameState.Starting;
    }

    void NewBeat(int currentBar, int _currentBeat)
    {
        if (lastBar == currentBar) return;
        lastBar = currentBar;

        if (State == GameState.Won) return;

        // Bar just changed, check win state

        if (State == GameState.AboutToWin)
        {
            State = GameState.Won;

            // Do big enemy death animation here

            win.SetActive(true);
            endBarText.text = $"Won in {currentBar} bars!";
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

    public static void MenuBtn()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public static void NextLevelBtn()
    {
        string name = SceneManager.GetActiveScene().name;
        int current = int.Parse(name[5..]);
        int next = current + 1;
        string nextScene = "Level" + next.ToString().PadLeft(2, '0');

        if (SceneUtility.GetBuildIndexByScenePath("Scenes/" + nextScene) != -1)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
