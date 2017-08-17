using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLItemState : MLGameState {

    [SerializeField]
    protected bool reappearOnAppAwake;

    private Item _item;

    public Item item {
        get {
            if(!_item) {
                _item = HierarchyHelper.SearchUpAndDown<Item>(transform);
            }
            return _item;
        }
    }

    public override MLNumericParam param {
        get {
            return (int)item.status;
        }
        protected set {
            //if(value == (int)ItemStatus.IN_INVENTORY) {
            //    if (item.itemStatus == ItemStatus.YET_TO_BE_ACQUIRED) {
            //        placeInInventory();
            //    }
            //}
            item.status = (ItemStatus)((int)value);
        }
    }


    protected override void _Awake() {
        base._Awake();
        if(!GetComponent<MLGameStateParamUpdater>()) {
            MLGameStateParamUpdater mlgsp = gameObject.AddComponent<MLGameStateParamUpdater>();
            mlgsp.onValue = 1f;
            mlgsp.updateMode = UpdateMode.ONE_VALUE;
        }
    }

    protected override void _Start() {
        base._Start();
        gameStateSaver.type = MLGameSavedStateType.SAVE_INT;
    }

    //TODO: plan how to save / recover items in Inventory
}
