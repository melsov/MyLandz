﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class AnimtorNoodler : MonoBehaviour {

    //Must have an Animator sibling component
    private Animator _animtor;
    protected Animator animtor {
        get {
            if(!_animtor) {
                _animtor = GetComponent<Animator>();
                Assert.IsTrue(_animtor, "AnimatorNoodler must have an Animator sibling component");
            }
            return _animtor;
        }
    }

    protected AnimationClip[] clips {
        get {
            return animtor.runtimeAnimatorController.animationClips;
        }
    }

    protected AnimatorStateInfo stateInfo {
        get {
            return animtor.GetCurrentAnimatorStateInfo(0);
        }
    }

    protected float normalizedTime {
        get { return stateInfo.normalizedTime % 1; }
    }

    protected AnimationClip clipNamed(string name) {
        foreach(AnimationClip ac in clips) {
            if(ac.name.Equals(name)) {
                return ac;
            }
        }
        return null;
    }

    protected AnimatorControllerParameter getParameter(int index) {
        if(index < animtor.parameterCount)
            return animtor.GetParameter(index);
        return null;
    }

    protected AnimatorControllerParameter firstParameter {
        get { return getParameter(0); }
    }

}
