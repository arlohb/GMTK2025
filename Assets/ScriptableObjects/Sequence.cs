using UnityEngine;

[CreateAssetMenu(fileName = "Sequence", menuName = "Scriptable Objects/Sequence")]
public class Sequence : ScriptableObject
{
    public Note[] notes;

    public Note GetNote(int beat)
    {
        return notes[beat - 1];
    }
}
