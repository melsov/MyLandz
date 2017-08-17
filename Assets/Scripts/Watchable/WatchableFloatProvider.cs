using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

//public struct NameForType
//{
//    public System.Type type;
//    public string name;

//    public NameForType(System.Type type) : this(type, type.ToString()) { }

//    public NameForType(System.Type type, string name) {
//        this.type = type; this.name = name;
//    }

//    public static implicit operator NameForType(System.Type type) { return new NameForType(type); }

//    public override string ToString() {
//        return name;
//    }
//}

//[CustomEditor(typeof(WatchableProvider))]
//public class EditorWatchableProvider : Editor
//{
//    static NameForType[] typeOptions = {
//        new NameForType( typeof(float), "float" ),
//        typeof(bool), 
//        typeof(MonoBehaviour),
//    };

//    private static int selection;

//    private static string[] typeNames {
//        get {
//            string[] result = new string[typeOptions.Length];
//            for(int i=0; i< result.Length; ++i) {
//                result[i] = typeOptions[i].ToString();
//            }
//            return result;
//        }
//    }

//    public override void OnInspectorGUI() {
//        base.OnInspectorGUI();
//        WatchableProvider wp = (WatchableProvider)target;
//        selection = EditorGUILayout.Popup("Type", selection, typeNames);
//        Debug.Log(selection);
//    }
//}

#endif

public class WatchableFloatProvider : MonoBehaviour
{
    private Watchable<float> storage = new Watchable<float>(0f);
    protected float val {
        get {
            return storage.val;
        }
        set {
            storage.val = value;
        }
    }

    public void addOnChangeListener(Action<float> a) {
        storage.addListener(a);
    }


}