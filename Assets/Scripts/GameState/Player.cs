using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public string handle = "Melba";

    private Inventory _inventory;
    public Inventory inventory {
        get {
            if (!_inventory) {
                _inventory = GetComponentInChildren<Inventory>();
            }
            return _inventory;
        }
    }

}
