using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finds the first parameter in an Animator.
/// Auto-detects the type of the parameter and sets it using <paramref name="value_"/>
/// Set value_, offValue to greater than zero for boolean true, zero or less for false.
/// Toggles between value_ and offValue, if <paramref name="toggleOnOff"/> is set to true
/// </summary>
public class MLAnimatorState : MLGameState {

    public override MLNumericParam param {
        get {
            return paramValue;
        }
        protected set {
            command.Invoke(value);
        }
    }

    [SerializeField, Header("If blank defaults to first param")]
    private string _paramName;
    private string paramName {
        get {
            if (string.IsNullOrEmpty(_paramName)) {
                _paramName = anim.GetParameter(0).name;
            }
            return _paramName;
        }
    }

    private AnimatorControllerParameterType type {
        get {
            foreach(AnimatorControllerParameter t in anim.parameters) {
                if (t.name.Equals(paramName)) {
                    return t.type;
                }
            }
            return AnimatorControllerParameterType.Trigger;
        }
    }

    private bool toggleState {
        get {
            return Mathf.Abs(paramValue) > .001f;
        }
    }

    private float paramValue {
        get {
            switch(type) {
                case AnimatorControllerParameterType.Bool:
                    return anim.GetBool(paramName) ? 1f : 0f;
                case AnimatorControllerParameterType.Float:
                    return anim.GetFloat(paramName);
                case AnimatorControllerParameterType.Int:
                    return anim.GetInteger(paramName);
                default:
                case AnimatorControllerParameterType.Trigger:
                    return 0f;
            }
        }
    }

    private Action<float> command {
        get {
            switch (type) {
                case AnimatorControllerParameterType.Bool:
                    return (float val) => {
                        anim.SetBool(paramName, val > 0);
                    };
                case AnimatorControllerParameterType.Float:
                    return (float val) => {
                        anim.SetFloat(paramName, val);
                    };
                case AnimatorControllerParameterType.Int:
                    return (float val) => {
                        anim.SetInteger(paramName, (int)val);
                    };
                case AnimatorControllerParameterType.Trigger:
                default:
                    return (float val) => {
                        anim.SetTrigger(paramName);
                    };
            }
        }
    }

    private Animator _anim;
    private Animator anim {
        get {
            if(!_anim) { _anim = GetComponent<Animator>(); }
            return _anim;
        }
    }


   
}
