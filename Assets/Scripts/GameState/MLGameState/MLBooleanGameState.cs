using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

[CustomEditor(typeof(MLBooleanGameState))]
public class MLBooleanGameStateDataEd : Editor
{
    public override void OnInspectorGUI() {
        MLBooleanGameState state = (MLBooleanGameState)target;
        EditorGUIHelper.guiColorForState(state.transform, state.param, new Color(.4f, 1f, .2f), new Color(.5f, .7f, .3f));
        base.OnInspectorGUI();
    }
}

public class MLBooleanGameState : MLGameState 
{
#if UNITY_EDITOR
    [SerializeField]
    private bool debug;
#endif

    private Watchable<bool> _storage;
    private Watchable<bool> storage {
        get {
            if (_storage == null) { _storage = new Watchable<bool>(false); }
            return _storage;
        }
    }

    public override MLNumericParam param {
        get {
            return storage.val;
        }

        protected set {
            storage.val = value.Bool;
#if UNITY_EDITOR
            if(debug) { print(string.Format("[{0}]: {1}", name, storage.val)); }
#endif
        }
    }

    public WatchableWrapper<bool> getWatchableBool() {
        return new WatchableWrapper<bool>(storage);
    }
}
