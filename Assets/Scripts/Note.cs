using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Note : MonoBehaviour, IPointerClickHandler
{
    // Set by PianoRoll
    public int instrument;
    public int beat;
    public Action<int, int, bool> setNote;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Test" + instrument + beat);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
