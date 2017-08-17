using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;


using UnityEditor;
using System.Reflection;
using System.Collections;

[CustomEditor(typeof(MLLogicGate))]
public class MLLogicGateDataEd : Editor
{
    public override void OnInspectorGUI() {
        MLLogicGate state = (MLLogicGate)target;
        try {
            EditorGUIHelper.guiColorForState(state.transform, state.state, Color.green, new Color(.3f, .8f, .8f));
        } catch (Exception e) {

        }
        base.OnInspectorGUI();
    }
}

public class MLLogicGate : MonoBehaviour
{
    [System.Serializable]
    struct BoolInput
    {
        public MLBooleanGameState state;
        public bool invert;
        
        public bool evaluate() { return invert != state.getWatchableBool().val; }

        public static implicit operator bool(BoolInput b) { return b.state; }
    }

#if UNITY_EDITOR
    public void setInputsToDefaults() {
        inputs = new BoolInput[2];
    }
#endif
    [SerializeField]
    private BoolInput[] inputs;


    [SerializeField]
    private LogicGate gate;

    [SerializeField, Header("If none, this")]
    protected MLGameState _target;
    protected MLGameState target_ {
        get {
            if(!_target) {
                _target = GetComponent<MLGameState>();
            }
            return _target;
        }
    }


    private void Start() {
        Assert.IsTrue(hasInputs);
        foreach(BoolInput input in inputs) {
            input.state.getWatchableBool().addListener((bool b) => {
                eval();
            });
        }
    }


    private bool getInput(int index) { return inputs[index].evaluate(); }

    public bool state {
        get {
            return gate.eval(getInput(0), getInput(1));
        }
    }

    private void eval() {
        if(!hasInputs) { return; }
        if (target_) {
            target_.enforce(state); 
        }
    }

    protected virtual bool hasInputs {
        get {
            if(inputs.Length != 2) { return false; }
            foreach(BoolInput input in inputs) {
                if(!input) { return false; }
            }
            return true;
        }
    }

    public override string ToString() {
        return string.Format("{0} a: {1} b: {2}", base.ToString(), getInput(0), getInput(1));
    }
}

[System.Serializable]
public struct LogicGate
{
    public enum Operation
    {
        AND, OR, NAND, NOR, XOR, XNOR
    }

    public Operation operation;

    public bool eval(bool a, bool b) {
        switch(operation) {
            case Operation.AND:
            default:
                return a && b;
            case Operation.OR:
                return a || b;
            case Operation.NAND:
                return !(a && b);
            case Operation.NOR:
                return !a && !b;
            case Operation.XOR:
                return a == b;
            case Operation.XNOR:
                return a != b;
        }
    }
    
}