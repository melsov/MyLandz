using System;
using UnityEngine;

public class MLGenericGameState : MLGameState
{
    private MLNumericParam _param;

    public override MLNumericParam param {
        get {
            return _param;
        }

        protected set {
            _param = value;
        }
    }
}