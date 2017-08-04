using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MLStaticAction : MLGameState {

    [SerializeField, Header("Add an updater set for only this action")]
    private bool addUpdater = true;

    public override void Awake() {
        base.Awake();
        gameStateSaver.type = MLGameSavedStateType.DONT_SAVE;
        MLGameStateParamUpdater paramUpdater = ComponentHelper.AddIfNotPresent<MLGameStateParamUpdater>(transform);
        if(addUpdater) {
            MLUpdaterSet updaterSet = ComponentHelper.AddIfNotPresent<MLUpdaterSet>(transform);
            updaterSet.setUpdaters(paramUpdater);
        }
    }

    public override MLNumericParam param {
        get {
            return false;
        }

        protected set {
            performStaticAction(value);
        }
    }

    protected abstract void performStaticAction(MLNumericParam value);

}
