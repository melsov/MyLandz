using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[CustomEditor(typeof(LerpDelimiter))]
public class LerpDelimiterDataEd : Editor
{
    private static Dictionary<LerpDelimiter, float> guiLerps = new Dictionary<LerpDelimiter, float>();

    public override void OnInspectorGUI() {
        LerpDelimiter ld = (LerpDelimiter)target;
        float lerp = guiLerps.ContainsKey(ld) ? guiLerps[ld] : 0f;
        EditorGUILayout.LabelField("Slide to interpolate");
        lerp = EditorGUILayout.Slider(ld.cursor, 0f, 1f);
        guiLerps[ld] = lerp;
        ld.interp(lerp);
        base.OnInspectorGUI();
    }
}

public class LerpDelimiter : MonoBehaviour
{
    [SerializeField]
    private Transform _start;

    [SerializeField]
    private Transform end;

    [SerializeField, Header("Avoid having target be a parent of either start or end!")]
    private Transform _target;

    [SerializeField]
    private float hideWhenBelow = -1f;

    [SerializeField]
    private float hideWhenAbove = 2f;

    [SerializeField]
    private bool useCurve;
    [SerializeField]
    private AnimationCurve curve;

    public float cursor {
        get; private set;
    }

    private void Start() {
        foreach(Transform child in HierarchyHelper.ChildrenRecursive(_target)) {
            Assert.IsFalse(child == _start);
            Assert.IsFalse(child == end);
        }
    }

    private Vector3 difference { get { return end.position - _start.position; } }
    private Vector3 startToTarget { get { return _target.position - _start.position; } }

    private float evaluate {
        get {
            if (useCurve) return curve.Evaluate(Mathf.Sqrt(startToTarget.sqrMagnitude / difference.sqrMagnitude));
            return Mathf.Sqrt(startToTarget.sqrMagnitude / difference.sqrMagnitude);
        }
    }

    public Vector3 interpolate(float zeroToOne) {
        return Vector3.Lerp(_start.position, end.position, Mathf.Clamp01(zeroToOne));
    }

    public Quaternion interpolateR(float zeroToOne) {
        return Quaternion.Lerp(_start.rotation, end.rotation, Mathf.Clamp01(zeroToOne));
    }

    public void interp(float zeroToOne) {
        cursor = Mathf.Clamp01(zeroToOne);
        float pos = cursor;
        if(useCurve) {
            pos = curve.Evaluate(cursor);
        }
        _target.position = interpolate(pos);
        _target.rotation = interpolateR(pos);

        _target.gameObject.SetActive(cursor > hideWhenBelow && cursor < hideWhenAbove);
    }
}
