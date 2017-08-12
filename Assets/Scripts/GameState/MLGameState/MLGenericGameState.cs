using System;
using UnityEngine;

public class MLGenericGameState : MLGameState
{
    [SerializeField]
    private bool debug;

    private MLNumericParam _param;

    public override MLNumericParam param {
        get {
            return _param;
        }

        protected set {
            if (debug) { Debug.Log(string.Format("ML generic GS {0} set to: {1}", name, value.ToString())); }
            _param = value;
        }
    }
}