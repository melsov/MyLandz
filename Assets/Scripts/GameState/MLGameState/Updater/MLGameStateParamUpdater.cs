using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum UpdateMode
{
    ONE_VALUE,
    TOGGLE_VALUE,
    INCREMENT_LOOP,
    INCREMENT_PING_PONG,
//TODO: ADD INCREMENT ONCE
}

[System.Serializable]
public class IncrementableRange
{
    public float min = 0f;
    public float max = 10f;
    public float increment = 1f;
    public bool pingPong;
    private float direction = 1f;

    private float sign { get { return Mathf.Sign(max - min); } }
    public float next(float val) {
        val += increment * direction;
        if (val >= max) {
            if (pingPong) {
                direction = -1f * sign;
                val = max;
            } 
            else { val = min; }
        }
        else if (val < min) {
            if (pingPong) { direction = 1f * sign; }
        }
        return val;
    }
}

public class MLGameStateParamUpdater : MonoBehaviour {

    [SerializeField]
    public UpdateMode updateMode = UpdateMode.ONE_VALUE;

    [SerializeField, Header("If none, searches sibling components")]
    protected MLGameState _mlGameState;
    protected MLGameState mlGameState {
        get {
            if(!_mlGameState) {
                _mlGameState = GetComponent<MLGameState>();
            }
            Assert.IsFalse(_mlGameState == null, "No game state?");
            return _mlGameState;
        }
    }

    [SerializeField, Header("Used for one value and toggle modes")]
    public float onValue;
    [SerializeField, Header("Used for toggle mode")]
    public float offValue;

    [SerializeField, Header("For loop and ping pong")]
    public IncrementableRange range;

    public void Awake() {
        if(updateMode == UpdateMode.INCREMENT_PING_PONG) {
            range.pingPong = true;
        }
    }

    public void enforceNext() {
        mlGameState.enforce(next(mlGameState.param));
    }

    protected float next(MLNumericParam param) {
        switch (updateMode) {
            case UpdateMode.ONE_VALUE:
            default:
                return onValue;
            case UpdateMode.TOGGLE_VALUE:
                return Mathf.Abs(param - onValue) > Mathf.Abs(param - offValue) ? onValue : offValue;
            case UpdateMode.INCREMENT_LOOP:
            case UpdateMode.INCREMENT_PING_PONG:
                return range.next(param);
        }
    }
    
    
}


