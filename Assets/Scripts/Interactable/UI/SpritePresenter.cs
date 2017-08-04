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
    public void present(SpriteRenderer sr, Action callback) {
        StartCoroutine(animate(sr, callback));
    }

    private IEnumerator animate(SpriteRenderer sr, Action callback) {
        sr.gameObject.SetActive(true);
        sr.enabled = true;
        sr.transform.position = podium.position;
        yield return new WaitForSeconds(2f);
        sr.enabled = false;
        callback.Invoke();
    }
}
