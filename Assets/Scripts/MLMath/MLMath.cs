using UnityEngine;
using System;

public static class MLMath
{

    public static float fmod(float value, float mod) {
        if(Mathf.Abs(mod) < Mathf.Epsilon) { return 0f; }
        return value - (float)Math.Truncate(value / mod) * mod;
    }
}
