using UnityEngine;
using System;

public class WatchableIntProvider : MonoBehaviour
{
    private Watchable<int> storage = new Watchable<int>(0);
    public int val {
        get {
            return storage.val;
        }
        set {
            storage.val = value;
        }
    }

    public void addOnChangeListener(Action<int> a) {
        storage.addListener(a);
    }
}