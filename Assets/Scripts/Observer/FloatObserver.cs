using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FloatObserver : MonoBehaviour
{
    [SerializeField, Header("comparison uses integers")]
    protected float condition = 0f;

    [SerializeField, Header("If none, searches this")]
    protected WatchableFloatProvider _provider;
    protected WatchableFloatProvider provider {
        get {
            if(!_provider) {
                _provider = GetComponent<WatchableFloatProvider>();
            }
            return _provider;
        }
    }

    [SerializeField]
    private MLUpdaterSet updaterSet;

    private void Start() {
        _provider.addOnChangeListener((float f) => { onValueChanged(f); });
    }

    private void onValueChanged(float f) {
        if((int) condition == (int) f) { //dirty secret
            if(updaterSet) { updaterSet.Invoke(); }
        }
    }

    
}
