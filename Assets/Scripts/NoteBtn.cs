using System;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NoteBtn : MonoBehaviour, IPointerClickHandler
{
    public Color disabledColor;
    public Color enabledColor;

    private int instrument;
    private int beat;
    private Action<int, int, bool> setNote;

    private bool isEnabled = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Test" + instrument + beat);
        isEnabled = !isEnabled;
        setNote(instrument, beat, isEnabled);
        UpdateColor();
    }

    public void Setup(int instrument, int beat, Action<int, int, bool> setNote)
    {
        this.instrument = instrument;
        this.beat = beat;
        this.setNote = setNote;
    }

    public void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = isEnabled
            ? enabledColor
            : disabledColor;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
