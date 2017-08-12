using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : PhoenixLikeSingleton<UIManager>
{
    protected UIManager() { }

    private RectTransform _rt;
    private RectTransform rt {
        get {
            if(!_rt) { _rt = GetComponent<RectTransform>(); }
            return _rt;
        }
    }


    private Dictionary<GameObject, bool> wasActive = new Dictionary<GameObject, bool>();

    internal void hidePopupUI() {
        refreshWasActive();
        foreach(RectTransform childRT in getRectTransformChildren()) {
            childRT.gameObject.SetActive(false);
        }
    }

    internal void showPopupUI() {
        restoreChildren();
    }

    private void refreshWasActive() {
        wasActive.Clear();
        foreach(RectTransform childRT in getRectTransformChildren()) {
            wasActive.Add(childRT.gameObject, childRT.gameObject.activeSelf);
        }
    }

    private IEnumerable<RectTransform> getRectTransformChildren() {
        for(int i=0; i < rt.childCount; ++i) {
            Transform child = rt.GetChild(i);
            RectTransform childRT = child.GetComponent<RectTransform>();
            if(!childRT) { continue; }
            yield return childRT;
        }
    }


    private void restoreChildren() {
        foreach(GameObject key in wasActive.Keys) {
            key.SetActive(wasActive[key]);
        }
    }
}