using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;

//#if UNITY_EDITOR
//using UnityEditor;
//[CustomEditor(typeof(MLLogicGate))]
//public class MLLogicGateDataEditor : Editor
//{
//    public List<Transform> objects = new List<Transform>();
//    private Component a, b;

//    public override void OnInspectorGUI() {
//        setupAndShowInputs();
//        base.OnInspectorGUI();
//    }

//    private void setupAndShowInputs() {
//        //FIXME: brutally ugly
//        MLLogicGate gate = (MLLogicGate)target;
//        WatchableBoolProvider[] inputs = gate.getInputs();
//        if(inputs == null || inputs.Length == 0) { inputs = new WatchableBoolProvider[2]; }
//        a = inputs[0] != null ? inputs[0] as Component : null;
//        b = inputs[1] != null ? inputs[1] as Component : null;
//        a = (Component) EditorGUILayout.ObjectField("input a: ", a, typeof(Component), true);
//        b = (Component)EditorGUILayout.ObjectField("input b: ", b, typeof(Component), true);
//        gate.setInputs(new WatchableBoolProvider[] {
//            a ? a.GetComponent<WatchableBoolProvider>() : null,
//            b ? b.GetComponent<WatchableBoolProvider>() : null
//        });
//    }
//}
//#endif


public class MLLogicGate : MonoBehaviour
{

    [SerializeField]
    private MLBooleanGameState[] inputs;


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
        foreach(MLBooleanGameState input in inputs) {
            input.getWatchableBool().addListender((bool b) => {
                eval();
            });
        }
    }

    private bool getInput(int index) { return inputs[index].getWatchableBool().val; }

    private void eval() {
        if(!hasInputs) { return; }
        if (target_) {
            target_.enforce(gate.eval(getInput(0), getInput(1)));
        }
    }

    protected virtual bool hasInputs {
        get {
            if(inputs.Length != 2) { return false; }
            foreach(MLBooleanGameState input in inputs) {
                if(input == null) { return false; }
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