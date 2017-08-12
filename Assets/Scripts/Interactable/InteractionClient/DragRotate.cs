using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

//TODO: minute hand should remember (save) its rotation?p

public class DragRotate : Interactable
{

    [SerializeField]
    private MLUpdaterSet updaterSet;

    private float angOffset;
    private Vector3 eulers;

    private List<Action<float>> _endDragListeners;
    private List<Action<float>> endDragListeners {
        get {
            if(_endDragListeners == null) {
                _endDragListeners = new List<Action<float>>();
            }
            return _endDragListeners;
        }
    }

    public void addEndDragListener(Action<float> endDragAction) {
        endDragListeners.Add(endDragAction);
    }

    public void removeEndDragListener(Action<float> endDragAction) {
        if(endDragListeners.Contains(endDragAction)) {
            endDragListeners.Remove(endDragAction);
        }
    }

    private void setAngOffset() {
        angOffset = CursorHelper.angleDegreesWithCursorXY(transform.position) - transform.rotation.eulerAngles.z;
    }

    protected override void OnMouseDown() {
        setAngOffset();
    }

    protected override void OnMouseDrag() {
        eulers = transform.rotation.eulerAngles;
        eulers.z = CursorHelper.angleDegreesWithCursorXY(transform.position) + angOffset;
        transform.rotation = Quaternion.Euler(eulers);
    }

    protected override void OnMouseUp() {
        if(updaterSet) {
            updaterSet.Invoke();
        }
    }
}
