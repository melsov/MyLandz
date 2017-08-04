using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NodeBoss : Singleton<NodeBoss> {
    protected NodeBoss() { }

    [SerializeField]
    private Node node;
    [SerializeField]
    private Transform anchor;
    public Vector3 anchorPos { get { return anchor.position; } }
    public Quaternion anchorRo { get { return anchor.rotation; } }

    public Node currentNode { get { return node; } }

    public void Start() {
        Node activeNode = null;
        Node[] nodes = FindObjectsOfType<Node>();
        foreach(Node n in nodes) {
            if (n.getIsActiveNodeFromPrefs()) {
                activeNode = n;
            } 
        }
        if(!activeNode) { activeNode = node; }
        else { node = activeNode; }
        Assert.IsFalse(node == null, "Hmm null node");
        foreach(Node n in nodes) {
            if(n == node) {
                n.activate();
            } else {
                n.deactivate();
            }
        }
    }

    public void activate(Node next) {
//TODO: handle case of next node in different scene
        if(node == next) { return; }
        node.deactivate();
        next.activate();
        node = next;
    }

    public bool isNodeActive(Node node) {
        return this.node.Equals(node);
    }
}
