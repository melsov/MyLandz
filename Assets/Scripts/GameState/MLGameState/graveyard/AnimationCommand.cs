using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationCommandBehaviour
{
    FORWARD_REVERSE_TOGGLE,
    FORWARD_ONLY_ONCE_EVER
}

public enum ClipPosition
{
    START, END
}
/// <summary>
/// Use to activate/control an animator with a single clip
/// </summary>
public class AnimationCommand : MonoBehaviour {
    
    public AnimationCommandBehaviour behaviour = AnimationCommandBehaviour.FORWARD_REVERSE_TOGGLE;

    private delegate void ReachedStartCallback();
    private ReachedStartCallback reachedStartCallback;
    private delegate void ReachedEndCallback();
    private ReachedEndCallback reachedEndCallback;

    private readonly string PlaybackSpeedParam = "PlaybackSpeed";
    private float speed = 1f;

    public delegate void OnReachedAnimLimit(ClipPosition clipPosition);
    public OnReachedAnimLimit onReachedAnimLimit;

    private Animator _animtor;
    private Animator animtor {
        get {
            if(!_animtor) {
                _animtor = GetComponent<Animator>();
            }
            return _animtor;
        }
    }

    private AnimationClip _clip;
    private AnimationClip clip {
        get {
            if(!_clip) {
                _clip = animtor.runtimeAnimatorController.animationClips[0];
            }
            return _clip;
        }
    }

    private AnimatorStateInfo state {
        get { return animtor.GetCurrentAnimatorStateInfo(0); }
    }

    public void Awake() {
        throw new Exception("Don't use this class please");
        setupClip();
    }

    private void setupClip() {
        switch(behaviour) {
            case AnimationCommandBehaviour.FORWARD_REVERSE_TOGGLE:
                setupForwardReverseClip();
                break;
            case AnimationCommandBehaviour.FORWARD_ONLY_ONCE_EVER:
                setupOnceEverClip();
                break;
        }
    }

    private void setupOnceEverClip() {
        addEndCallback();
        reachedEndCallback = reachedEndOnlyOnceMode;
    }

    
    private void setupForwardReverseClip() {
        addStartCallback();
        addEndCallback();
        reachedStartCallback = reachedStartToggleMode;
        reachedEndCallback = reachedEndToggleMode;
    }

    private void addEndCallback() {
        AnimationEvent ae = new AnimationEvent();
        ae.time = clip.length;
        ae.functionName = "endCallback";
        clip.AddEvent(ae);
    }

    private void addStartCallback() {
        AnimationEvent ae = new AnimationEvent();
        ae.time = 0f;
        ae.functionName = "startCallback";
        clip.AddEvent(ae);
    }

    public void endCallback() {
        reachedEndCallback();
    }
    
    public void startCallback() {
        reachedStartCallback();
    }

    public void reachedEndToggleMode() {
        print("end cb toggle mode");
        animtor.SetFloat(PlaybackSpeedParam, 0f);
        if(onReachedAnimLimit != null)
            onReachedAnimLimit(ClipPosition.END);
    }

    public void reachedStartToggleMode() {
        print("start cb toggle mode");
        animtor.SetFloat(PlaybackSpeedParam, 0f);
        if(onReachedAnimLimit != null)
            onReachedAnimLimit(ClipPosition.START);
    }

    private void reachedEndOnlyOnceMode() {
        animtor.SetFloat(PlaybackSpeedParam, 0f);
        if (onReachedAnimLimit != null)
            onReachedAnimLimit(ClipPosition.END);
    }

    //public override void invoke(MLNumericParam param) {
    //    animtor.SetFloat(PlaybackSpeedParam, speed);
    //    if (behaviour == AnimationCommandBehaviour.FORWARD_REVERSE_TOGGLE) {
    //        speed *= -1;
    //    }
    //}

}
