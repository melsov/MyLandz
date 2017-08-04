using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual void Awake() {
        ComponentHelper.EnforceTriggerCollider(transform);
    }

    private Collider _collidr;
    protected Collider collidr {
        get {
            if(!_collidr) { _collidr = GetComponentInChildren<Collider>(); }
            return _collidr;
        }
    }

    //public Vector3 mouseWorldPosition {
    //    get {
    //        Vector3 v = Input.mousePosition;
    //        return Camera.main.ScreenToWorldPoint(new Vector3(v.x, v.y, transform.position.z)); // Camera.main.nearClipPlane + 4));
    //    }
    //}

    //protected bool cursorRaycastPosition(out Vector3 pos) {
    //    RaycastHit rh;
    //    bool result = CursorInput.CursorRayhit(collidr, out rh);
    //    pos = rh.point;
    //    return result;
    //}

    [SerializeField, Header("If none, searches sibling components")]
    private MLUpdaterSet _mlUpdaterSet;
    protected MLUpdaterSet mlUpdaterSet {  //TODO: PURGE UNUSED?
        get {
            if (!_mlUpdaterSet) {
                _mlUpdaterSet = ComponentHelper.AddIfNotPresent<MLUpdaterSet>(transform);
            }
            return _mlUpdaterSet;
        }
    }

    protected abstract void OnMouseDown();
    protected abstract void OnMouseDrag();
    protected abstract void OnMouseUp();


}
