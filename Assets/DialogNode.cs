using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogEdge
{
    public string label;

    [Header("If none, invokes updaterSet")]
    public DialogNode dialogNode;

    [Header("If none, loads node")]
    public MLUpdaterSet updaterSet;

    public void invoke() {
        if (dialogNode && !dialogNode.isPlaceHolder) {
            DialogBoss.Instance.load(dialogNode);
            return;
        }
        if(updaterSet) {
            updaterSet.Invoke();
        }
        DialogBoss.Instance.hide();
    }
}

public class DialogNode : MonoBehaviour {

    [SerializeField, TextArea]
    private string _text;
    public string text { get { return _text; } }

    public bool isPlaceHolder { get { return string.IsNullOrEmpty(text); } }

    [SerializeField]
    private DialogEdge[] _edges;

    public IEnumerable<DialogEdge> edges() {
        foreach(DialogEdge de in _edges) { yield return de; }
    }

    public int Length { get { return _edges.Length; } }

    public DialogEdge this[int i] {
        get {
            if (i < _edges.Length) { return _edges[i]; }
            return default(DialogEdge);
        }
    }
}
