using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

public class MLTransformLerpGameState : MLGameState
{
    [SerializeField]
    private float zeroToOneAnimationDuration = .8f;
    [SerializeField]
    private int resolution = 10;

    [SerializeField]
    private string reachedEndAudio;
    [SerializeField]
    private string reachedStartAudio;

    private bool startMethodHappened;
    protected override void _Start() {
        base._Start();
        startMethodHappened = true;
    }

    [SerializeField, Header("If none, search this and children")]
    private LerpDelimiter _lerpDelimiter;
    private LerpDelimiter lerpDelimiter {
        get {
            if(!_lerpDelimiter) {
                _lerpDelimiter = GetComponentInChildren<LerpDelimiter>();
            }
            return _lerpDelimiter;
        }
    }

    public override MLNumericParam param {
        get {
            return lerpDelimiter.cursor;
        }

        protected set {
            if (startMethodHappened) {
                if(MLMath.SomewhatCloseValues(value.value_, lerpDelimiter.cursor)) {
                    return;
                }
                StartCoroutine(animateTo(value, () => {
                    notifyChainLinks();
                }));
            } else {
                lerpDelimiter.interp(value);
            }
        }
    }


    private IEnumerator animateTo(MLNumericParam value, Action callback) {
        float start = lerpDelimiter.cursor;
        float end = value;
        float totalDuration = Mathf.Abs(start - end) * zeroToOneAnimationDuration;
        int frames = Mathf.CeilToInt(Mathf.Abs(start - end) * resolution);
        float tick = totalDuration / resolution;
        float nudge = (end - start) / resolution;
        
        for(int i = 1; i <= frames; ++i) {
            lerpDelimiter.interp(start + nudge * i);
            yield return new WaitForSeconds(tick);
        }

        lerpDelimiter.interp(value);
        playAudioFor(end);
        callback.Invoke();
    }

    private void playAudioFor(float destination) {
        if(MLMath.SomewhatCloseValues(0f, destination)) {
            if (!string.IsNullOrEmpty(reachedEndAudio)) {
                AudioManager.Instance.play(reachedStartAudio);
            }
        }
        else if(MLMath.SomewhatCloseValues(1f, destination)) {
            if (!string.IsNullOrEmpty(reachedStartAudio)) {
                AudioManager.Instance.play(reachedEndAudio);
            }
        }
    }
}
