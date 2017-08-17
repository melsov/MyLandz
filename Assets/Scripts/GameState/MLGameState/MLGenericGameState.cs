using System;
using UnityEngine;

public class MLGenericGameState : MLGameState
{
    [SerializeField]
    private bool debug;

    private Watchable<MLNumericParam> __param;
    private Watchable<MLNumericParam> _param {
        get {
            if(__param == null) { __param = new Watchable<MLNumericParam>(0); }
            return __param;
        }
    }

    public WatchableWrapper<MLNumericParam> getWatchableParam() { return new WatchableWrapper<MLNumericParam>(_param); }

    public override MLNumericParam param {
        get {
            return _param.val;
        }

        protected set {
            if (debug) { Debug.Log(string.Format("[ML_GENERIC_GS]: {0} [PARAM]: {1}", name, value.ToString())); }
            _param.val = value;
        }
    }
}