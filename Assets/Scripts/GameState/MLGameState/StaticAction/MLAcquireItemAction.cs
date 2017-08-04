using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLAcquireItemAction : MLStaticAction {

    [SerializeField]
    protected bool giveInfinitely;

    //Update sets are painful to construct...
    //TODO: make them construct themselves reliably at least: perhaps MLGameStates can be asked for their MLGameStateUpdaters
    //OR: (better?) make actions chainable...
    [SerializeField, Header("If none, searches siblings and parent")]
    private Item _item;
    protected Item item {
        get {
            if(!_item) {
                _item = HierarchyHelper.SearchUpAndDown<Item>(transform);
            }
            return _item;
        }
    }

    protected override void performStaticAction(MLNumericParam value) {
        Item theItem = item;
        if(giveInfinitely) {
            theItem = Instantiate<Item>(item);
        }
        GameManager.Instance.player.inventory.add(theItem);
    }
}
