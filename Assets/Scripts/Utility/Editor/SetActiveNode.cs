using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;

public class SetActiveNode : MonoBehaviour {
    [MenuItem("MyLandz/Set Active Node %&#a")]
    static void _setActiveNode() {
        if(!Selection.activeGameObject) { print("nothing selected"); return; }
        Node selected = HierarchyHelper.SearchUpAndDown<Node>(Selection.activeGameObject.transform);
        //try to set pprefs
        foreach(Node node in FindObjectsOfType<Node>()) { 
#if UNITY_EDITOR
                node.setPlayerPrefState(node == selected);
#endif
        }
        if(!selected) { print("node not found."); return; }
        selected.activate();
    }

	[MenuItem("MyLandz/Make New Node %&#d")]
    static void makeNewNode() {
        int COLUMNS = 6;
        Node[] nodes = FindObjectsOfType<Node>();
        if(nodes.Length == 0) { print("no nodes in this scene?"); return; }
        Transform nodeParent = nodes[0].transform.parent;
        Assert.IsTrue(nodeParent.name.Equals("Nodes"), "Is this really the node parent: " + nodeParent.name + "?");
        VectorXY lowerRight = new VectorXY(-9999f);
        VectorXY upperLeft = new VectorXY(9999f);
        foreach(Node n in nodes) {
            lowerRight = VectorXY.max(lowerRight, new VectorXY(n.transform.position));
            upperLeft = VectorXY.min(upperLeft, new VectorXY(n.transform.position));
        }
        Vector3 target = lowerRight.vector3() + Vector3.right * 16f;
        if(nodes.Length % COLUMNS == 0) {
            target = new Vector3(upperLeft.x, lowerRight.y - 16f, 0f);
        }
        Node orig = HierarchyHelper.SearchUpAndDown<Node>(Selection.activeGameObject.transform);
        if(!orig) {
            orig = nodes[nodes.Length - 1];
        }
        Node next = Instantiate<Node>(orig);
        next.transform.SetParent(null);
        next.transform.position = target;
        next.transform.SetParent(nodeParent);
        
    }
}
