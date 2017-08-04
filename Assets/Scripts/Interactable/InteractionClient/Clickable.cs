using UnityEngine;
using System.Collections;
using System;

public class Clickable : MonoBehaviour
{
    private BoxXY _boxXY;
    protected BoxXY boxXY {
        get {
            if (!_boxXY) {
                _boxXY = GetComponent<BoxXY>();
                if (!_boxXY) {
                    _boxXY = gameObject.AddComponent<BoxXY>();
                }
            }
            return _boxXY;
        }
    }

    private Collider _collidr;
    protected Collider collidr {
        get {
            if(!_collidr) { _collidr = GetComponentInChildren<Collider>(); }
            return _collidr;
        }
    }

    private CursorInteraction _cursorInteraction;
    protected CursorInteraction cursorInteraction {
        get {
            if(!_cursorInteraction) {
                _cursorInteraction = ComponentHelper.AddIfNotPresent<CursorInteraction>(transform);
            }
            return _cursorInteraction;
        }
    }

    //TODO: determine: do we need multiple mlgameStates to enforce sometimes?
    //ALSO: Does MLAction exist? (Game state that never saves; i.e. just a method call?)
    //CASE THAT PROBLEMATIZES: items. need to check if they have been collected. but not get re-collected
    // if they are simply reinstating their visibility status on re-load of app.
    [SerializeField, Header("If none, searches sibling components")]
    private MLUpdaterSet _paramUpdaterSet;
    private MLUpdaterSet paramUpdaterSet { 
        get {
            if (!_paramUpdaterSet) {
                _paramUpdaterSet = ComponentHelper.AddIfNotPresent<MLUpdaterSet>(transform);
            }
            return _paramUpdaterSet;
        }
    }

    public void OnEnable() {
        cursorInteraction.cursorUp += click;
    }

    public void OnDisable() {
        cursorInteraction.cursorUp -= click;
    }

    private void click(VectorXY worldPoint) {
        if(mouseIsHoveringOverUs()) {
            paramUpdaterSet.Invoke();
        }
    }

    protected bool mouseIsHoveringOverUs() {
        return CursorInput.Instance.IsColliderUnderCursor(collidr);
    }

    
}
