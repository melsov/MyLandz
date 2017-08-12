using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : Interactable {

    [SerializeField]
    private bool returnToPositionIfUnclaimed = true;
    private Vector3 startPos;

    private Vector3 dragRelative;
    private Vector3 mwp;
    private DropBox dropBox;

    public bool disabled = false;

    private static Vector3 cameraToCursor {
        get {
            return CursorHelper.cursorGlobalXYCameraNearClipPlaneZ - Camera.main.transform.position;
        }
    }

    private Vector3 cursorOnTransform {
        get {
            Vector3 camToTrans = transform.position - Camera.main.transform.position;
            return Camera.main.transform.position + 
                cameraToCursor * 
                (Vector3.Dot(Camera.main.transform.forward, camToTrans) / Vector3.Dot(Camera.main.transform.forward, cameraToCursor));
        }
    }

    protected override void OnMouseDown() {
        if(disabled) { return; }
        startPos = transform.position;
        dragRelative = transform.position - cursorOnTransform;
    }

    protected override void OnMouseDrag() {
        if(disabled) { return; }
        transform.position = cursorOnTransform + dragRelative;
        DropBox nextBox = RaycastMaster.ComponentUnderCursor<DropBox>(collidr);
        if(nextBox) {
            dropBox = nextBox;
            dropBox.canAccept(transform);
        } else {
            if (dropBox) {
                dropBox.cancel();
                dropBox = null;
            }
        }
    }

    protected override void OnMouseUp() {
        if(disabled) { return; }        
        if(dropBox && dropBox.accept(transform)) {
            return;
        }
        if(returnToPositionIfUnclaimed) {
            transform.position = startPos;
        }
    }
}
