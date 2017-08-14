using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MLBooleanGameState : MLGameState 
{
    private WatchableWrapper<bool> _storage;
    private Watchable<bool> storage {
        get {
            if (_storage == null) { _storage = new WatchableWrapper<bool>(false); }
            return _storage.watchable;
        }
    }

    public override MLNumericParam param {
        get {
            return storage.val;
        }

        protected set {
            storage.val = value.Bool;
        }
    }

    public Watchable<bool> getWatchableBool() {
        return storage;
    }
}
