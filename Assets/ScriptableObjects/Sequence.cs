using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Sequence", menuName = "Scriptable Objects/Sequence")]
public class Sequence : ScriptableObject, IEquatable<Sequence>
{
    public Note[] notes;

    public Note GetNote(int beat)
    {
        return notes[beat - 1];
    }

    public override int GetHashCode()
    {
        return notes.Aggregate(
            0,
            (hash, note) => HashCode.Combine(hash, note)
        );
    }

    public override bool Equals(object other)
    {
        return Equals(other as Sequence);
    }

    public bool Equals(Sequence other)
    {
        return GetHashCode() == other.GetHashCode();
    }
}
