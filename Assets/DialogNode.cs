using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(DialogNode))]
public class DialogNodeDataEditor : Editor
{
    private Color normalColor = new Color(.8f, .8f, .8f);
    private Color[] bgColors = { Color.cyan, Color.green, Color.yellow, new Color(.1f, .7f, .7f) };
    private Dictionary<DialogNode, bool> foldoutLookup = new Dictionary<DialogNode, bool>();

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.LabelField(
            "Graph of DialogNodes and DialogEdges. \nA node owns an array of edges.\n" +
            "Edges own references to a node and an updater. \n" +
            "If an edge has a non-null node and a non-null updater, \n" + 
            "the node takes predence when traversing the graph. ", GUILayout.MinHeight(80));

        DialogNode dn = (DialogNode)target;
        displayDNode(dn, 0);
    }

    private void displayDNode(DialogNode dn, int colorIndex) {
        EditorGUI.indentLevel = 1;
        GUIStyle style = EditorStyles.helpBox;
        RectOffset ro = new RectOffset();
        ro.left = 22;
        style.margin = ro;
        
        GUI.backgroundColor = normalColor; // bgColors[colorIndex % bgColors.Length];
        GUI.contentColor = bgColors[colorIndex % bgColors.Length];

        bool shouldFoldout = false;
        if(foldoutLookup.ContainsKey(dn)) { shouldFoldout = foldoutLookup[dn]; }
        foldoutLookup[dn] = EditorGUILayout.Foldout(shouldFoldout, dn.name);
        if (foldoutLookup[dn]) {
            EditorGUILayout.BeginVertical(style);
            EditorGUILayout.LabelField(string.Format("NODE: {0}", dn.name));

            dn.setText = EditorGUILayout.TextArea(dn.text, GUILayout.MinHeight(60));

            List<DialogEdge> edges = new List<DialogEdge>(dn.edges());

            for (int i = 0; i < edges.Count; ++i) {

                DialogEdge edge = edges[i];
                EditorGUI.indentLevel = 1;

                GUI.backgroundColor = bgColors[colorIndex % bgColors.Length];
                GUI.contentColor = bgColors[colorIndex % bgColors.Length];
                edge.label = EditorGUILayout.TextField(string.Format("EDGE {0}:", i), edge.label);

                EditorGUI.indentLevel = 2;
                edge.updaterSet = (MLUpdaterSet)EditorGUILayout.ObjectField(
                    string.Format("UPDATER:"),
                    edge.updaterSet,
                    typeof(MLUpdaterSet),
                    true);

                edge.dialogNode = (DialogNode)EditorGUILayout.ObjectField(
                    string.Format("NODE:"),
                    edge.dialogNode,
                    typeof(DialogNode),
                    true);

                if (!edge.dialogNode) {
                    GUIStyle buttonStyle = EditorStyles.miniButtonRight;
                    RectOffset bRo = new RectOffset(); bRo.left = 33; buttonStyle.margin = bRo;

                    if (GUILayout.Button("Attach new sub-node", buttonStyle)) {
                        DialogNode.CreateSubNode(dn, i);
                    }
                }

                if (edge.dialogNode) {
                    displayDNode(edge.dialogNode, colorIndex + 1);
                }
            }

            GUI.backgroundColor = bgColors[colorIndex % bgColors.Length];
            GUI.contentColor = bgColors[colorIndex % bgColors.Length];
            if (GUILayout.Button("Add edge")) {
                dn.addEdge(new DialogEdge());
            }

            EditorGUILayout.EndVertical();
        }
    }
}

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

#if UNITY_EDITOR
    public string setText {
        set {
            _text = value;
        }
    }

    public void addEdge(DialogEdge edge) {
        List<DialogEdge> temp = new List<DialogEdge>(_edges);
        temp.Add(edge);
        _edges = temp.ToArray();
    }

    public static void CreateSubNode(DialogNode parent, int edgeIndex) {
        DialogNode childNode = Instantiate<DialogNode>(parent);
        childNode.transform.SetParent(parent.transform);
        childNode.name = string.Format("{0}-sub-node", parent.name);
        childNode.setText = "--";
        parent._edges[edgeIndex].dialogNode = childNode;
    }
#endif
}
