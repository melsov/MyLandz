using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLItemReceiverState : MLGameState {

    public override MLNumericParam param {
        get {
            return itemReceiver.itemDisplayProxy != null;
        }

        protected set {
            if(value) {
                if(!itemReceiver.itemDisplayProxy) {
                    Item prefab = Item.FromName(itemReceiver.acceptItemName, ItemStatus.DEPLOYED);
                    ItemDisplayProxy idp = ItemDisplayProxy.InstantiateFromTemplate(prefab);
                    itemReceiver.reinstateItemProxy(idp);
                } 
                if(itemReceiver.itemDisplayProxy) {
                    itemReceiver.itemDisplayProxy.GetComponentInChildren<DragDrop>().disabled = true;
                }
            }
        }
    }

    private ItemReceiver _itemReceiver;
    private ItemReceiver itemReceiver {
        get { if (!_itemReceiver) { _itemReceiver = GetComponent<ItemReceiver>(); } return _itemReceiver; }
    }

    public override void Awake() {
        base.Awake();
        if(!GetComponent<MLGameStateParamUpdater>()) {
            MLGameStateParamUpdater mlgsp = gameObject.AddComponent<MLGameStateParamUpdater>();
            mlgsp.onValue = 1f;
            mlgsp.updateMode = UpdateMode.ONE_VALUE;
        }
    }

	
}
