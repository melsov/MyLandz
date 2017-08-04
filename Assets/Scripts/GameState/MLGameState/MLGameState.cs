using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
NOTES:
What if we want (for example):
An item to be received (MLItemReceiverState).
Which causes an animation
Whose completion causes another state to change
Currently:
An item receiver state toggles on and...not sure...
guess we'd just use an updaterset

MLGameState callbacks would... 
--get called within param set; (or 'Invoke' if we change to MLAction as the base class)
     --new overridable structure: 
         --doInvoke(MLInvokeData data) {
            Invoke(data);
            StartCorou(postInvoke(data, () => { //postInvoke yields returns null, invokes the action and is done by default
                 callCallbacks(data);
            }
         }
     --or: don't use a Coroutine: and there's some kind of MLAction connector object/class??? (named MLChainLink?)
     --so:
        --doInvoke(data) {
           Invoke(data);
           callCallbacks(data); //where callback nudge any chainlinks:
                                // chainlinks can:
                                //  --simply call doInvoke(data) on another MLAction/GameState immediately
                                //  --do something over time...
         }

--conclusion: callbacks MLChainLinks? are just another hook for chaining MLActions

 *  */

//Duks (random notes): boomerangs push the pinata back

[System.Serializable]
public struct MLNumericParam
{
    public float value_;

    public static implicit operator int(MLNumericParam mlp) { return Mathf.RoundToInt(mlp.value_); }
    public static implicit operator bool(MLNumericParam mlp) { return Mathf.Abs(mlp.value_) > .001f; }
    public static implicit operator float(MLNumericParam mlp) { return mlp.value_; }

    public static implicit operator MLNumericParam(int i) { return new MLNumericParam(i); }
    public static implicit operator MLNumericParam(bool b) { return new MLNumericParam(b ? 1f : 0f); }
    public static implicit operator MLNumericParam(float f) { return new MLNumericParam(f); }

    public MLNumericParam(float value_) {
        this.value_ = value_;
    }

    public static MLNumericParam fromAnimator(Animator anim, AnimatorControllerParameter acp) {
        switch(acp.type) {
            case AnimatorControllerParameterType.Bool:
                return anim.GetBool(acp.name);
            case AnimatorControllerParameterType.Float:
                return anim.GetFloat(acp.name);
            case AnimatorControllerParameterType.Int:
                return anim.GetInteger(acp.name);
            default:
                return 0f;
        }
    }
}



public abstract class MLGameState : MonoBehaviour
{
    [SerializeField]
    protected MLGameStateSaver gameStateSaver;

    [SerializeField]
    private List<MLChainLink> chainLinks;

    public virtual void Awake() { }

    public virtual void Start() {
        gameStateSaver.reinstateFromPrefs(this);
    }

    private string _key;
    public string key {
        get {
            if (_key == null) { _key = HierarchyHelper.GenerateKey(transform); }
            return _key;
        }
    }

    public abstract MLNumericParam param {
        get; protected set;
    }

    public void enforce(MLNumericParam _mlnp) {
        param = _mlnp;
        gameStateSaver.writeToPrefs(this);
        foreach(MLChainLink link in chainLinks) {
            link.link(new ChainLinkData(this));
        }
    }
}



public static class HierarchyHelper
{
    public static IEnumerable<Transform> Parents(Transform t) {
        Transform result = t;
        while(result.parent) {
            result = result.parent;
            yield return result;
        }
    }

    public static T SearchUpAndDown<T>(Transform trans) where T : Component {
        T result = trans.GetComponentInChildren<T>();
        if (!result) {
            result = trans.GetComponentInParent<T>();
        }
        return result;
    }

    public static string GenerateKey(Transform t) {
        string result = t.name;
        foreach(Transform parent in HierarchyHelper.Parents(t.transform)) {
            result = string.Format("{0}.{1}", parent.name, result);
        }
        result = string.Format("{0}.{1}", t.gameObject.scene, result);
        return result;
    }
}

public static class ComponentHelper
{
    public static T AddIfNotPresent<T>(Transform trans) where T : Component {
        T thing = trans.GetComponent<T>();
        if (!thing) {
            thing = trans.gameObject.AddComponent<T>();
        }
        return thing;
    }
    public static T AddIfNotPresentInChildren<T>(Transform trans) where T : Component {
        T thing = trans.GetComponentInChildren<T>();
        if (!thing) {
            thing = trans.gameObject.AddComponent<T>();
        }
        return thing;
    }

    public static T CreateGameObjectWithComponent<T>() where T : Component {
        GameObject go = new GameObject();
        return go.AddComponent<T>();
    }

    public static T GetComponentOnlyInChildren<T>(Transform trans) where T : Component {
        T result = null;
        foreach(Transform child in trans) {
            result = child.GetComponentInChildren<T>();
            if(result) { break; }
        }
        return result;
    }
    internal static void EnforceTriggerCollider(Transform transform) {
        Collider coll = transform.GetComponent<Collider>();
        if(coll is MeshCollider) {
            ((MeshCollider)coll).convex = true;
        }
        try {
            coll.isTrigger = true;
        } catch(Exception e) {
            MonoBehaviour.print(transform.name + " has a convex collider?? " + e.ToString());
        } 
    }
}

