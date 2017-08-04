using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetGenerator : MonoBehaviour {

    [MenuItem("MyLandz/Generate Item")]
    static void generateItem() {
        Layer layr = FindSomewhere<Layer>(Selection.activeGameObject);
        Generate<Item>("ItemDemo", layr.transform);
    }

    [MenuItem("MyLandz/Generate Item Receiver")]
    static void generateReceiver() {
        Layer layr = FindSomewhere<Layer>(Selection.activeGameObject);
        Generate<ItemReceiver>("ItemReceiver", layr.transform);
    }

    [MenuItem("MyLandz/Add Updaters And UpSets")]
    static void addUpdatersAndSets() {
        if(Selection.gameObjects.Length == 0) { print("nothing selected"); }
        foreach(GameObject go in Selection.gameObjects) {
            if(go.GetComponent<MLGameState>()) {
                ComponentHelper.AddIfNotPresent<MLGameStateParamUpdater>(go.transform);
                ComponentHelper.AddIfNotPresent<MLUpdaterSet>(go.transform);
            }
        }
    }

    private static T Generate<T>(string generateFolderRelPath, Transform par) where T : MonoBehaviour {
        T prefab = Resources.Load<T>(PathMaster.ResourcesRelativeGenerateFolder + "/" + generateFolderRelPath);
        if (!prefab) {
            print("Prefab not found: " + typeof(T));
        }
        T copy = Instantiate<T>(prefab);
        copy.transform.position = par.position;
        copy.transform.SetParent(par);
        return copy;
    }

    private static T FindSomewhere<T>(GameObject lookHereFirst) where T : MonoBehaviour {
        T result = null;
        if(lookHereFirst) {
            result = HierarchyHelper.SearchUpAndDown<T>(lookHereFirst.transform);
        }
        if(!result) {
            result = FindObjectOfType<T>();
        }
        return result;
    }
}
