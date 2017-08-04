using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogButton : MonoBehaviour {

    [HideInInspector]
    public int index;

    private void Awake() {
        Button b = GetComponent<Button>();
        b.onClick.AddListener(clicked);
    }

    private void clicked() {
        DialogBoss.Instance.OnClicked(this);
    }
}
