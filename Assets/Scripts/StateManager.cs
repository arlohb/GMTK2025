using UnityEngine;

public class StateManager : MonoBehaviour
{
    public Enemy enemy;
    public PianoRoll pianoRoll;

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
}
