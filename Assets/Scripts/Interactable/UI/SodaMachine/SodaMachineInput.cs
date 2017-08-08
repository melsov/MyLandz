using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SodaMachineInput : MonoBehaviour {

    private CodeAndUpdater[] codes;
    private int longestCode;

    private void Awake() {
        codes = GetComponentsInChildren<CodeAndUpdater>();
        foreach(CodeAndUpdater cau in codes) {
            longestCode = cau.code.Length > longestCode ? cau.code.Length : longestCode;
        }
        GetComponent<AlphaNumericInput>().handleInput = (string code, Action<bool, MLUpdaterSet> callback) => {
            MLUpdaterSet upSet = null;
            bool result = evaluate(code, out upSet);
            callback.Invoke(result, upSet);
        };
    }

    private bool evaluate(string code, out MLUpdaterSet upSet) {
        upSet = null;
        foreach(CodeAndUpdater cau in codes) {
            if(cau.code.ToLower().Equals(code.ToLower())) {
                upSet = cau.updaterSet;
                return true;
            }
        }
        return code.Length >= longestCode;
    }

}
