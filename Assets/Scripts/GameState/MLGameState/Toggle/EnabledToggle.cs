using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnabledToggle : BinaryToggle {
    public override bool state {
        set {
            gameObject.SetActive(value);
        }
    }

}
