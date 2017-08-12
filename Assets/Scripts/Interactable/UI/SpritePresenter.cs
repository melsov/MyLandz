using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePresenter : MonoBehaviour {

    [SerializeField, Header("If none, this transform")]
    private Transform _podium;
    private Transform podium {
        get {
            if(!_podium) { _podium = transform; }
            return _podium;
        }
    }

    [SerializeField]
    private Transform _backdrop;

    private void Start() {
        _backdrop.gameObject.SetActive(false);
    }

    public void present(SpriteRenderer sr, Action callback) {
        StartCoroutine(animate(sr, callback));
    }

    private IEnumerator animate(SpriteRenderer orig, Action callback) {
        UIManager.Instance.hidePopupUI();
        _backdrop.gameObject.SetActive(true);
        SpriteRenderer sr = Instantiate<SpriteRenderer>(orig);
        sr.gameObject.SetActive(true);
        sr.enabled = true;
        sr.transform.position = podium.position;
        sr.transform.SetParent(podium);
        yield return new WaitForSeconds(2f);
        sr.enabled = false;
        Destroy(sr.gameObject);
        _backdrop.gameObject.SetActive(false);
        UIManager.Instance.showPopupUI();
        callback.Invoke();
    }
}
