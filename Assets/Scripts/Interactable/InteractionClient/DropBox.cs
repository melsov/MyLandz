using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class ObservableTransform
//{
//    private Transform _target;
//    public Transform target {
//        get { return _target; }
//        private set {
//            _target = value;
//        }
//    }

//    public delegate void OnSetParent(ObservableTransform ot);
//    public OnSetParent onSetParent;

//    public Vector3 position {
//        get { return target.position; }
//        set { target.position = value; }
//    }

//    public void SetParent(Transform par) {
//        parent = par;
//    }

//    public Transform parent {
//        get { return target.parent; }
//        set {
//            target.SetParent(value);
//            onSetParent(this);
//        }
//    }

//    public ObservableTransform(Transform target) {
//        this.target = target;
//    }

//    public static implicit operator Transform(ObservableTransform ot) { if(!ot) { return null; } return ot.target; }

//    public static implicit operator bool(ObservableTransform ot) { return ot != null; }
//}

[RequireComponent (typeof(Highlightable))]
public class DropBox : MonoBehaviour {

    public delegate bool CanAccept(Transform t);
    private CanAccept _canAccept;

    public delegate void Take(Transform t);
    private Take take;

    private Highlightable _highlightable;
    private Highlightable highlightable {
        get {
            if(!_highlightable) {
                _highlightable = GetComponent<Highlightable>();
            }
            return _highlightable;
        }
    }

    private void Start() {
        highlightable.highlightOnCursorHover = false;
    }

    public void setDelegates(CanAccept canAcc, Take take) {
        _canAccept = canAcc;
        this.take = take;
    }

    void OnDisable() {
        removeDelegates();
    }

    private void removeDelegates() {
        _canAccept = null;
        take = null;
    }

    public bool canAccept(Transform t) {
        if(_canAccept == null) { return false; }
        bool result = _canAccept(t);
        if(result) {
            highlightable.highlight(true);
        }
        return result;
    }

	public bool accept(Transform t) {
        if(_canAccept == null || take == null) {
            return false;
        }
        if(_canAccept(t)) {
            highlightable.highlight(false);
            take(t);
            return true;
        }
        return false;
    }

    internal void cancel() {
        highlightable.highlight(false);
    }
}
