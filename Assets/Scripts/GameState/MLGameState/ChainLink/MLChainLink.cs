using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Assertions;
using System;

//[CustomEditor(typeof(MLChainLink))]
//public class ChainLinkDataEditor : Editor
//{
//    public string[] options = { "elf", "dwarf", "human" };
//    public int index = 0;
//    public int sliderVal = 3;
//    public float knobVal = 2f;
//    public override void OnInspectorGUI() {
//        //TESTS
//        base.OnInspectorGUI();
//        MLChainLink cl = (MLChainLink)target;
//        EditorGUILayout.LabelField("Flow", cl.flow.ToString());
//        EditorGUILayout.BeginToggleGroup("Flow is great", cl.flow > 1);
//        {
//            EditorGUILayout.LabelField("flowing");
//        }
//        EditorGUILayout.EndToggleGroup();

//        if(GUILayout.Button("Test B")) {
//            Debug.Log("test b");
//        }

//        //popup
//        index = EditorGUILayout.Popup(index, options);
//        if(index==1) {
//            EditorGUILayout.LabelField("chose dwarf");
//        } else {
//            sliderVal = EditorGUILayout.IntSlider(sliderVal, 0, 6);
//            knobVal = EditorGUILayout.Knob(Vector2.one * 55f, knobVal, 1.2f, 4.8f, "gzurbs", Color.red, Color.green, true);
//        }

//    }
//}

//public enum ParamConversionType
//{
//    AS_IS, FLIP_ZERO_POSITIVE
//}

//[System.Serializable]
//public class ParamConverter
//{
//    ParamConversionType conversionType;
//    public float positiveValue = 1;

//    public MLNumericParam convert(MLNumericParam param) {
//        switch (conversionType) {
//            case ParamConversionType.AS_IS:
//            default:
//                return param;
//            case ParamConversionType.FLIP_ZERO_POSITIVE:
//                return new MLNumericParam(param.value_ > 0f ? positiveValue : 0f);
//        }
//}

[System.Serializable]
public struct ChainLinkData
{
    private MLGameState caller; 
    public MLNumericParam getData() {
        return caller.param;
    }

    public ChainLinkData(MLGameState caller) {
        this.caller = caller;
    }
}

[System.Serializable]
public struct ParamCondition
{
    public enum ConditionType
    {
        PASS_ANY, ONLY_TRUE, INT_EQUALS,
        FLOAT_GREATER_THAN,
    }

    public ConditionType type;

    [Header("Used with INT_EQUALS")]
    public int equalsCondition;
    public float paramGreaterThan;

    public bool doesPass(MLNumericParam param) {
        switch(type) {
            case ConditionType.PASS_ANY:
            default:
                return true;
            case ConditionType.ONLY_TRUE:
                return param == true;
            case ConditionType.INT_EQUALS:
                return param == equalsCondition;
            case ConditionType.FLOAT_GREATER_THAN:
                Debug.Log(string.Format("param: {0} greater than {1} : {2}", param.value_, paramGreaterThan, param.value_ > paramGreaterThan));
                return param > paramGreaterThan;
        }
    }
}

//Optionally scale or shift (or whatever!) incoming params
[System.Serializable]
public struct LinkFilter
{
    [SerializeField]
    private bool debug;

    public enum FilterType
    {
        PASS_AS_IS, FLOAT_TO_BOOL, SCALE_ADD, SCALE_ADD_MOD, SCALE_ADD_CLAMP, SCALE_ADD_MOD_ROUND,
        SCALE_ADD_MOD_FLOOR, SCALE_ADD_MOD_CEIL, SCALE_ADD_MOD_OFFSET,
        FLOAT_TO_BOOL_INVERT, TRUE_IF_EQUALS_AS_INT,
    }
    [SerializeField]
    private FilterType type;

    [SerializeField, Header("Used with scaled and add")]
    private float scale;
    [SerializeField, Header("Used with scaled and add")]
    private float add;
    [SerializeField, Header("Used with scaled add mod")]
    private float mod;
    [SerializeField, Header("Used with scale add mod offset")]
    private float offset;
    [SerializeField]
    private int intCompare;
    [System.Serializable]
    struct ClampRange
    {
        public float min, max;
    }
    [SerializeField]
    private ClampRange clampRange;

    public MLNumericParam filter(MLNumericParam param) {
        if(debug) {
            return debugFilter((float)param);
        }
        switch(type) {
            case FilterType.PASS_AS_IS:
            default:
                return param;
            case FilterType.FLOAT_TO_BOOL:
                return param.Bool ? 1f : 0f;
            case FilterType.SCALE_ADD:
                return param * scale + add;
            case FilterType.SCALE_ADD_MOD:
                return MLMath.fmod(param * scale + add, mod);
            case FilterType.SCALE_ADD_CLAMP:
                return Mathf.Clamp(param * scale + add, clampRange.min, clampRange.max);
            case FilterType.SCALE_ADD_MOD_ROUND:
                return Mathf.Round(MLMath.fmod(param * scale + add, mod));
            case FilterType.SCALE_ADD_MOD_FLOOR:
                return Mathf.Floor(MLMath.fmod(param * scale + add, mod));
            case FilterType.SCALE_ADD_MOD_CEIL:
                return Mathf.Ceil(MLMath.fmod(param * scale + add, mod));
            case FilterType.SCALE_ADD_MOD_OFFSET:
                return MLMath.fmod(param * scale + add, mod) + offset;
            case FilterType.FLOAT_TO_BOOL_INVERT:
                return param.Bool ? 0f : 1f;
            case FilterType.TRUE_IF_EQUALS_AS_INT:
                return (int)param.value_ == intCompare ? 1f : 0f;
        }
    }

    private MLNumericParam debugFilter(DebugableFloat param_) {
        DebugableFloat param = param_;
        param = param * scale + add;
        param.debug();
        switch(type) {
//DEBUG
            case FilterType.PASS_AS_IS:
            default:
                return (float)param_;
            case FilterType.FLOAT_TO_BOOL:
                param = Mathf.Abs((float)param) > 0.001f ? 1f : 0f;
                break;
            case FilterType.SCALE_ADD:
                break;
            case FilterType.SCALE_ADD_MOD:
                param = MLMath.fmod(param, mod);
                break;
            case FilterType.SCALE_ADD_CLAMP:
                param = Mathf.Clamp(param, clampRange.min, clampRange.max);
                break;
            case FilterType.SCALE_ADD_MOD_ROUND:
                param = Mathf.Round(MLMath.fmod(param, mod));
                break;
            case FilterType.SCALE_ADD_MOD_FLOOR:
                param = Mathf.Floor(MLMath.fmod(param, mod));
                break;
            case FilterType.SCALE_ADD_MOD_CEIL:
                param = Mathf.Ceil(MLMath.fmod(param, mod));
                break;
            case FilterType.SCALE_ADD_MOD_OFFSET:
                param = MLMath.fmod(param, mod) + offset;
                break;
//DEBUG
        }
        return (float)param;
    }
}

public class MLChainLink : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private bool debug;
    protected void DBUG(string s) { if (debug) Debug.Log(s); }
#endif

    [SerializeField]
    protected ParamCondition condition;
    [SerializeField]
    protected LinkFilter filter;

    private void Start() {
        _Start();
    }

    protected virtual void _Start() { }

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

    public virtual void link(ChainLinkData data) {
        if (condition.doesPass(data.getData())) {
            MLNumericParam passOn = filter.filter(data.getData());
            target_.enforce(passOn);
        }
    }
    
}