using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : Hoverable {

    public bool highlightOnCursorHover = true;

    private bool unhighlightSafetyProcess;
    private float unhighlightStartTime;

    private Renderer _rendrr;
    private Renderer rendrr {
        get {
            if(!_rendrr) {
                _rendrr = GetComponentInChildren<Renderer>();
            }
            return _rendrr;
        }
    }

    public void Start() {
        highlight(false);
    }

    public void highlight(bool _highlight) {
        if(!rendrr || !rendrr.material || !rendrr.gameObject.activeSelf) { return; }
        rendrr.material.SetFloat("_OutlineScale", _highlight ? 1f : 0f);
        if(!highlightOnCursorHover) {
            if(_highlight) {
                unhighlightStartTime = Time.time;
                if (!unhighlightSafetyProcess) {
                    StartCoroutine(unhighlightAfterABit());
                }
            }
        }
    }

    private IEnumerator unhighlightAfterABit() {
        unhighlightSafetyProcess = true;
        while(Time.time < unhighlightStartTime + 2f) {
            yield return new WaitForSeconds(.3f);
        }
        rendrr.material.SetFloat("_OutlineScale", 0f);
        unhighlightSafetyProcess = false;
    }

    public override void OnMouseEnter() {
        if(highlightOnCursorHover)
            highlight(true);
    }

    public override void OnMouseExit() {
        if(highlightOnCursorHover)
            highlight(false);
    }

    public override void OnMouseOver() {
    }
}
