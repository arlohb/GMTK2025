using System;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Note", menuName = "Scriptable Objects/Note")]
public class Note : ScriptableObject
{
    public AudioResource source;
    public string playerMove;
    public string enemyMove;
}
