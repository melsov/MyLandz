using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CursorDownWatcher : MonoBehaviour
{
    private MLBooleanGameState _state;
    private MLBooleanGameState state {
        get {
            if(!_state) { _state = GetComponent<MLBooleanGameState>(); }
            return _state;
        }
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            state.enforce(true);
        }
        else if(Input.GetMouseButtonUp(0)) {
            state.enforce(false);
        }
    }
}
