using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;

public class SetActiveNode : MonoBehaviour
{
    private static int COLUMNS = 6;

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

	[MenuItem("PrefabGen/New node from selected %&#d")]
    static void makeNewNode() {
        Node orig = HierarchyHelper.SearchUpAndDown<Node>(Selection.activeTransform);
        if(!orig) {
            orig = Resources.Load<Node>(PathMaster.ResourceRelPathForGSModule("TemplateNode"));
        }
        Node next = makeNewNode(orig, getNodeParent());
    }

    [MenuItem("PrefabGen/New node from template %&#f")]
    private static void newNodeFromTemplate() {
        Node next = makeNewNode(Resources.Load<Node>(PathMaster.ResourceRelPathForGSModule("TemplateNode")), getNodeParent());
        SpriteRenderer sr = next.GetComponentInChildren<SpriteRenderer>();
        SelectionHelper.setSelected(sr.gameObject);
    }

    private static Node makeNewNode(Node orig, Transform nodeParent) {
        Node next = Instantiate<Node>(orig);
        next.transform.SetParent(null);
        next.transform.position = nextOpenNodePosition();
        next.transform.SetParent(nodeParent);
        SelectionHelper.setSelected(next.gameObject);
        return next;
    }

    private static Transform getNodeParent() {
        Transform nodeParent = null;
        Node anyNode = FindObjectOfType<Node>();
        if (anyNode) {
            nodeParent = anyNode.transform.parent;
        }
        Assert.IsTrue(nodeParent.name.Equals("Nodes"), "Is this really the node parent: " + nodeParent.name + "?");
        return nodeParent;
    }

    private static Vector3 nextOpenNodePosition() {
         Node[] nodes = FindObjectsOfType<Node>();
        if(nodes.Length == 0) { return Vector3.zero; }

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
        return target;
    }
}
