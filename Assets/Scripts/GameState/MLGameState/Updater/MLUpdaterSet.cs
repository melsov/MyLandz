using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLUpdaterSet : MonoBehaviour {

    [SerializeField, Header("If None, searches self and children")]
    private List<MLGameStateParamUpdater> _updaters;
    private List<MLGameStateParamUpdater> updaters {
        get {
            if (_updaters == null || _updaters.Count == 0) {
                    _updaters = findUpdatersRespectSubSets(); 
            }
            return _updaters;
        }
    }

    //Adds any MLGameStateParamUpdaters in self and children
    //Except if child has an MLUpdaterSet component
    private List<MLGameStateParamUpdater> findUpdatersRespectSubSets() {
        List<MLGameStateParamUpdater> result = new List<MLGameStateParamUpdater>();
        List<Transform> _tranforms = new List<Transform>();
        _tranforms.Add(transform);
        while(_tranforms.Count > 0) {
            Transform next = _tranforms[0];
            _tranforms.RemoveAt(0);
            MLUpdaterSet uset = next.GetComponent<MLUpdaterSet>();
            if(uset && uset != this) {
                continue;
            }
            MLGameStateParamUpdater updater = next.GetComponent<MLGameStateParamUpdater>();
            if(updater) {
                result.Add(updater);
            }
            foreach(Transform child in next) {
                _tranforms.Add(child);
            }
        }
        return result;
    }

    public void setUpdaters(params MLGameStateParamUpdater[] _updaters) {
        this._updaters = new List<MLGameStateParamUpdater>(_updaters);
    }

    public void Invoke() {
        foreach(MLGameStateParamUpdater updater in updaters) {
            updater.invokeNext();
        }
    }

}
