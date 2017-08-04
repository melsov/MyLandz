using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLShowHideAlphaNumInputAction : MLStaticAction {
    [SerializeField]
    private bool show = true;

    [SerializeField]
    private AlphaNumericInput alpha;

    protected override void performStaticAction(MLNumericParam value) {
        alpha.gameObject.SetActive(show);
    }
}
