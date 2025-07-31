using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Note", menuName = "Scriptable Objects/Note")]
public class Note : ScriptableObject
{
    public AudioResource source;
    public string playerMove;
    public InputAction inputAction;
    public string enemyMove;
}
