using UnityEngine;
using System;

public class WatchableRotationXY : WatchableFloatProvider
{
    [SerializeField, Header("this transform if none")]
    private Transform _target;
    private Transform target {
        get {
            if(!_target) { _target = transform; }
            return _target;
        }
    }

    private void FixedUpdate() {
        val = target.localRotation.eulerAngles.z;
    }

}