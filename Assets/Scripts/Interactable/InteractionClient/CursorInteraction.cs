using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class CursorInteraction : MonoBehaviour {

    //public void Start() {
    //    Collider coll = GetComponent<Collider>();
    //    Assert.IsFalse(coll == null, name + ": Need a collider in CursorInteraction");
    //    if (coll) {
    //        coll.isTrigger = true; //OnMouse... functions called only on triggers
    //    }
    //}

    public delegate void CursorDown(VectorXY worldPoint);
    public CursorDown cursorDown;

    public delegate void CursorDrag(VectorXY worldPoint);
    public CursorDrag cursorDrag;

    public delegate void CursorUp(VectorXY worldPoint);
    public CursorUp cursorUp;

    public virtual void mouseDown(VectorXY worldPoint) {
        if(cursorDown != null) cursorDown(worldPoint);
    }

    public virtual void drag(VectorXY worldPoint) {
        if(cursorDrag != null) cursorDrag(worldPoint);
    }

    public virtual void mouseUp(VectorXY worldPoint) {
        if(cursorUp != null) cursorUp(worldPoint);
    }

/*
 * NOT USING THESE TO REPLACE CURSORINPUT BECAUSE:
 * Cursor Input works with physics colliders and concave mesh colliders (i.e. colliders from blender)
 * TODO: implement hovering (in a separate MonoBehaviour)
 *  */
    //public void OnMouseEnter() {

    //}

    //public void OnMouseOver() {
    //    if (Input.GetButtonUp("Fire1")) {
    //        cursorUp(new VectorXY(Input.mousePosition));
    //    }
    //}

    //public void OnMouseExit() {

    //}
}


