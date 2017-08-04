using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

    public bool isSceneEntryPoint;

    [SerializeField, Header("If none, this transform")]
    private Transform _cameraAnchor;
    private Transform cameraAnchor {
        get {
            if(!_cameraAnchor) { _cameraAnchor = transform; }
            return _cameraAnchor;
        }
    }

    private LayerSet _layerSet;      
    protected LayerSet layerSet {
        get {
            if(!_layerSet) {
                _layerSet = GetComponentInChildren<LayerSet>();
            }
            return _layerSet;
        }
    }

    public void activate() {
        gameObject.SetActive(true);
        Camera.main.transform.position = cameraAnchor.position;
        //transform.position = NodeBoss.Instance.anchorPos;
        //transform.rotation = NodeBoss.Instance.anchorRo;
        saveState();
    }

    public void deactivate() {
        gameObject.SetActive(false);
        saveState();
    }

    private void saveState() {
        PlayerPrefs.SetInt(key, gameObject.activeSelf ? 1 : 0);
    }

#if UNITY_EDITOR
    public void setPlayerPrefState(bool active) {
        PlayerPrefs.SetInt(key, active ? 1 : 0);
    }
#endif

    public bool getIsActiveNodeFromPrefs() {
        return 1 == PlayerPrefs.GetInt(key);
    }

    private string key {
        get { return HierarchyHelper.GenerateKey(transform); }
    }

}
