using UnityEngine;

[CreateAssetMenu(fileName = "Sequence", menuName = "Scriptable Objects/Sequence")]
public class Sequence : ScriptableObject
{
    public Note[] notes;
}
