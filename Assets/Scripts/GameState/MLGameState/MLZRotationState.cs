using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MLZRotationState : MLGameState
{

    private Vector3 eulers;

    public override MLNumericParam param {
        get {
            return transform.rotation.eulerAngles.z;
        }

        protected set {
            eulers = transform.rotation.eulerAngles;
            eulers.z = value;
            transform.rotation = Quaternion.Euler(eulers);
        }
    }
}
