using System;
using UnityEngine;

public class GameStateToggle : BinaryToggle
{
    [SerializeField, Header("If none, searches siblings")]
    private MLGameState _mlGameState;
    private MLGameState mlGameState {
        get {
            if(!_mlGameState) { _mlGameState = GetComponent<MLGameState>(); }
            return _mlGameState;
        }
    }

    public override bool state {
        set {
            Debug.Log("got state: " + value);
            mlGameState.enforce(value);
        }
    }
}