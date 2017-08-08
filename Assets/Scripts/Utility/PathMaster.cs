using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class PathMaster {

    public static string PrefabAbsolutePath { get { return string.Format("{0}/{1}/{2}", Application.dataPath, "Resources", ResourcesRelativePrefabFolder); } }

    public static string ItemPrefabAbsolutePath { get { return string.Format("{0}/Item", PrefabAbsolutePath); } }

    public static string ResourcesRelativePrefabFolder { get { return "Prefab"; } }

    public static string ResourcesRelativeItemFolder { get { return string.Format("{0}/{1}", ResourcesRelativePrefabFolder, "Item"); } }

    public static string ResourcesRelativeGenerateFolder { get { return "GeneratePrefab"; } }

    public static string ResourcesLocal { get { return "Assets/Resources"; } }

    public static string ResourcesRelGSModulePath { get { return string.Format("{0}/GSModule", ResourcesRelativePrefabFolder); } }

    public static string ResourceRelPathForGSModule(string moduleName) {
        return string.Format("{0}/{1}", ResourcesRelGSModulePath, moduleName);
    }
	
}

public static class Prefabulator
{

    public static T GetOrCreatePrefab<T>(string resourcesRelPath, string prefabName, T orig) where T : MonoBehaviour {
        return GetOrCreatePrefabInstance<T>(resourcesRelPath, prefabName, orig, ReplacePrefabOptions.Default);
    }

    /// <summary>
    /// Unity Editor function
    /// </summary>
    /// <param name="resourcesRelPath">Path to a folder inside of the Resources folder and relative to it</param>
    /// <param name="prefabName">Name of the prefab, omitting '.prefab'</param>
    /// <param name="orig">Transform in the scene on which to base a new prefab if none is found</param>
    /// <param name="rpos">Replace prefab options</param>
    /// <returns></returns>
    public static T GetOrCreatePrefabInstance<T>(string resourcesRelPath, string prefabName, T orig, ReplacePrefabOptions rpos) where T : MonoBehaviour {
        string resourcesLocalName = string.Format("{0}/{1}", resourcesRelPath, prefabName);
        T result = Resources.Load<T>(resourcesLocalName);
        if(result) {
            return Object.Instantiate(result);
        }
#if UNITY_EDITOR
        if(!result) {
            Assert.IsFalse(orig == null, "Whoops. we need an original of this transform at this point");
            if(orig == null) { return null; }

            string localPath = string.Format("{0}/{1}{2}", PathMaster.ResourcesLocal, resourcesLocalName, ".prefab");
            GameObject go = PrefabUtility.CreatePrefab(localPath, orig.gameObject, rpos);
            if(go) {
                result = Object.Instantiate(go.GetComponent<T>());
            }
        }
#endif
        return result;
    }

    public static T CreatePrefabInstance<T>(string resourcesRelPath, string prefabName) where T : MonoBehaviour {
        string resourcesLocalName = string.Format("{0}/{1}", resourcesRelPath, prefabName);
        T result = Resources.Load<T>(resourcesLocalName);
        if(result) {
            return Object.Instantiate(result);
        }
        return null;
    }
}
