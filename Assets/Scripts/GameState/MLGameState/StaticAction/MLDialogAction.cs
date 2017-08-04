using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLDialogAction : MLStaticAction {

    protected override void performStaticAction(MLNumericParam value) {
        DialogBoss.Instance.load(GetComponent<DialogNode>());
    }
}
