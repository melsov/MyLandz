using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PrefabGenerator : MonoBehaviour
{

    [MenuItem("PrefabGen/Add selected prefab to closest layer, break instance %&#c")]
    public static void addPrefabBreakInstance() {
        Layer closest = SelectionHelper.findClosestToMainCameraXY<Layer>();
        Transform parent = closest ? closest.transform : null;
        addPrefabBreakInstance(SelectionHelper.getSelectedGameObject(), parent);
    }

    [MenuItem("PrefabGen/Create go-to-node button &#w" )]
    public static void addGoToNodeButton() {
        Layer layer = SelectionHelper.fromSelectionOrParents<Layer>();
        Node from = null;
        if (!layer) {
            from = SelectionHelper.fromSelectionOrParents<Node>();
            if (!from) {
                Debug.Log("select a node in the hierarchy to indicate the embarcation point for this button");
                return;
            }
            layer = from.GetComponentInChildren<Layer>();
        }

        if(!layer) { Debug.Log("no layer"); return; }

        Transform parent = layer.transform;
        SpriteRenderer bgSprite = SelectionHelper.findBackgroundSprite(layer.transform);
        if(bgSprite) { parent = bgSprite.transform; }

        Transform instance = Instantiate<Transform>(Resources.Load<Transform>(PathMaster.ResourceRelPathForGSModule("GoToNodeClick")));
        if(instance.GetComponent<MLNodeState>()) {
            instance.GetComponent<MLNodeState>().setNodeInEditor(SelectionHelper.getIslandMapNode());
        }
        instance.position = parent.position;
        instance.SetParent(parent);
        PrefabUtility.DisconnectPrefabInstance(instance.gameObject);
    }


    private static void addPrefabBreakInstance(GameObject target, Transform parent) {
        if(!target) { return; }
        Transform _instance = Instantiate(target.transform);
        _instance.position = parent.position + Vector3.forward * -.2f;
        _instance.SetParent(parent);
        PrefabUtility.DisconnectPrefabInstance(_instance);
        SelectionHelper.setSelected(_instance.gameObject);
    }
}

public static class SelectionHelper
{
    public static Object getSelectedObject() {
        if (!Selection.activeObject) {
            Debug.Log("No object selected");
        }
        return Selection.activeObject;
    }

    public static void setSelected(params GameObject[] gos) {
        Selection.objects = gos;
    }

    public static Transform getSelectedTransform() {
        if(!Selection.activeTransform) {
            Debug.Log("No transform selected");
        }
        return Selection.activeTransform;
    }

    public static GameObject getSelectedGameObject() {
        if(!Selection.activeGameObject) {
            Debug.Log("No gameObject Selected");
        }
        return Selection.activeGameObject;
    }

    public static T fromSelection<T>() where T : Component {
        Transform selected = getSelectedTransform();
        if(!selected) { return null; }
        return selected.GetComponent<T>();
    }

    public static T fromSelectionOrParents<T>() where T : Component {
        T result = fromSelection<T>();
        if(!result) {
            Transform selected = getSelectedTransform();
            if(selected) {
                foreach(Transform parent in HierarchyHelper.Parents(selected)) {
                    result = parent.GetComponent<T>();
                    if(result) { break; }
                }
            }
        }
        return result;
    }

    public static T findClosestToMainCameraXY<T>() where T : Component {
        if(!Camera.main) { return null; }
        VectorXY dist = new VectorXY(999999f); VectorXY next;
        T result = null;
        foreach(T t in Object.FindObjectsOfType<T>()) {
            next = Camera.main.transform.position - t.transform.position;
            if(next.magnitudeSquared < dist.magnitudeSquared) {
                dist = next;
                result = t;
            }
        }
        return result;
    }

    public static SpriteRenderer findBackgroundSprite(Transform t) {
        foreach (SpriteRenderer srendrr in t.GetComponentsInChildren<SpriteRenderer>()) {
            if(srendrr.CompareTag("NodeBG") || srendrr.name.Equals("NodeBGSprite")) {
                return srendrr;
            }
        }
        return null;
    }

    public static Node getIslandMapNode() {
        foreach(Node n in Object.FindObjectsOfType<Node>()) {
            if(n.name.Equals("islandMapNode")) {
                return n;
            }
        }
        return null;
    }

}