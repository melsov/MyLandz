using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/*
 * The UI representation of the inventory
* *  */
public class InventoryPanel : Singleton<InventoryPanel> {
    protected InventoryPanel() { }

    public void Awake() {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    internal void putBackInInventory(Transform transform) {
        throw new NotImplementedException();
    }
}
