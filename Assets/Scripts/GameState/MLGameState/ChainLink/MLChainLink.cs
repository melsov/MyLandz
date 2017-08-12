using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Assertions;

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
        PASS_ANY, ONLY_TRUE, INT_EQUALS
    }

    public ConditionType type;

    [Header("Used with INT_EQUALS")]
    public int equalsCondition;

    public bool doesPass(MLNumericParam param) {
        switch(type) {
            case ConditionType.PASS_ANY:
            default:
                return true;
            case ConditionType.ONLY_TRUE:
                return param == true;
            case ConditionType.INT_EQUALS:
                return ((int)param) == equalsCondition;
        }
    }
}

//Optionally scale or shift (or whatever!) incoming params
[System.Serializable]
public struct LinkFilter
{
    public enum FilterType
    {
        PASS_AS_IS, FLOAT_TO_BOOL, SCALE_ADD, SCALE_ADD_MOD
    }
    [SerializeField, Header("Used with scaled and add")]
    private float scale;
    [SerializeField, Header("Used with scaled and add")]
    private float add;
    [SerializeField, Header("Used with scaled add mod")]
    private float mod;


    [SerializeField]
    private FilterType type;
    public MLNumericParam filter(MLNumericParam param) {
        switch(type) {
            case FilterType.PASS_AS_IS:
            default:
                return param;
            case FilterType.FLOAT_TO_BOOL:
                return Mathf.Abs((float)param) > 0.001f ? 1f : 0f;
            case FilterType.SCALE_ADD:
                return param * scale + add;
            case FilterType.SCALE_ADD_MOD:
                return MPMath.fmod(param * scale + add, mod);
        }
    }
}

public class MLChainLink : MonoBehaviour
{
    [SerializeField]
    protected ParamCondition condition;
    [SerializeField]
    private LinkFilter filter;
    
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
            Debug.Log("pass on data: " + passOn.ToString());
            target_.enforce(passOn);
            //target_.enforce(filter.filter(data.getData()));
        }
    }
    
}