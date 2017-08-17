using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DebugableFloat
{

    public float storage;
    public string whatHappened = "";

    public DebugableFloat(float storage) : this(storage, string.Format("Start value: {0}", storage)) { }

    public DebugableFloat(float storage, string whatHappened) {
        this.whatHappened = whatHappened;
        this.storage = storage;
    }

    public void debug() {
        Debug.Log(whatHappened);        
    }

    public static implicit operator float(DebugableFloat d) { return d.storage; }
    public static implicit operator DebugableFloat(float f) { return new DebugableFloat(f); }


    public static DebugableFloat operator *(DebugableFloat a, DebugableFloat b) {
        float result = a.storage * b.storage;
        return new DebugableFloat(result, string.Format("A[ {0} ] | B[ {1} ] -> {2}={3}*{4} ", a.whatHappened, b.whatHappened, result, a.storage, b.storage));
    }

    public static DebugableFloat operator +(DebugableFloat a, DebugableFloat b) {
        float result = a.storage + b.storage;
        return new DebugableFloat(result, string.Format("A[ {0} ] | B[ {1} ] -> {2}={3}+{4} ", a.whatHappened, b.whatHappened, result, a.storage, b.storage));
    }
}
