using TMPro;
using UnityEngine;

public class CurrentBar : MonoBehaviour
{
    void Start()
    {
        BeatManager.Get().RegisterListener(NewBeat);
    }

    void NewBeat(int currentBar, int _currentBeat)
    {
        GetComponent<TextMeshProUGUI>().text = currentBar.ToString().PadLeft(2, '0');
    }
}
