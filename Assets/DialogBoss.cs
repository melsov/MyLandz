using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoss : Singleton<DialogBoss> {

    [SerializeField]
    private RectTransform box;

    [SerializeField]
    private Text text;

    [SerializeField]
    private Button[] buttons;

    private DialogNode node;

    private void Start() {
        box.gameObject.SetActive(true);
        int i = 0;
        foreach(Button b in buttons) {
            ComponentHelper.AddIfNotPresent<DialogButton>(b.transform).index = i++;
        }
        box.gameObject.SetActive(false);
    }

    public void load(DialogNode dnode) {
        node = dnode;
        box.gameObject.SetActive(true);
        text.text = dnode.text;
        hideButtons();
        for(int i = 0; i < dnode.Length; ++i) {
            if(i >= buttons.Length) { break; }
            Button b = buttons[i];
            b.gameObject.SetActive(true);
            DialogEdge de = dnode[i];
            b.GetComponentInChildren<Text>().text = de.label;
        }
    }

    private void hideButtons() {
        foreach(Button b in buttons) {
            b.gameObject.SetActive(false);
        }
    }

    internal void hide() {
        box.gameObject.SetActive(false);
    }

    internal void OnClicked(DialogButton dialogButton) {
        if(!node) { return; }
        DialogEdge de = node[dialogButton.index];
        de.invoke();
    }
}
