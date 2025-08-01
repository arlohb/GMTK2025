using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Note", menuName = "Scriptable Objects/Note")]
public class Note : ScriptableObject
{
    public AudioResource source;
    public InputAction inputAction;
    public Move playerMove;
    public Move enemyMove;

    public override int GetHashCode()
    {
        return HashCode.Combine(playerMove, enemyMove);
    }
}
