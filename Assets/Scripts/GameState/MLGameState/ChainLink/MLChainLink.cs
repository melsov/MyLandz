using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(MLChainLink))]
public class ChainLinkDataEditor : Editor
{
    public string[] options = { "elf", "dwarf", "human" };
    public int index = 0;
    public int sliderVal = 3;
    public float knobVal = 2f;
    public override void OnInspectorGUI() {
        //TESTS
        base.OnInspectorGUI();
        MLChainLink cl = (MLChainLink)target;
        EditorGUILayout.LabelField("Flow", cl.flow.ToString());
        EditorGUILayout.BeginToggleGroup("Flow is great", cl.flow > 1);
        {
            EditorGUILayout.LabelField("flowing");
        }
        EditorGUILayout.EndToggleGroup();

        //popup
        index = EditorGUILayout.Popup(index, options);
        if(index==1) {
            EditorGUILayout.LabelField("chose dwarf");
        } else {
            sliderVal = EditorGUILayout.IntSlider(sliderVal, 0, 6);
            knobVal = EditorGUILayout.Knob(Vector2.one * 55f, knobVal, 1.2f, 4.8f, "gzurbs", Color.red, Color.green, true);
        }

    }
}

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

public class MLChainLink : MonoBehaviour
{
    public int fake;
    public float flow { get { return fake / 2f; } }

    [SerializeField]
    MLGameState target;

    public void link(ChainLinkData data) {
        target.enforce(data.getData());
    }
    
}