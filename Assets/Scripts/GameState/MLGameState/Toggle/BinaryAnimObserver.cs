using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BinaryAnimObserver : AnimtorNoodler {

    [SerializeField]
    private string idleClip = "Idle";
    [SerializeField]
    private string openClip = "Open";
    [SerializeField]
    private string closeClip = "Close";

    [SerializeField, Header("If not assigned, searches children.")]
    private BinaryToggle _binaryToggle;
    private BinaryToggle binaryToggle {
        get {
            if(!_binaryToggle) {
                _binaryToggle = ComponentHelper.GetComponentOnlyInChildren<BinaryToggle>(transform);
            }
            return _binaryToggle;
        }
    }

	public void Awake () {
        Assert.IsTrue(clipNamed(openClip) != null);
        Assert.IsTrue(clipNamed(closeClip) != null);
        setupEvent(openClip, 1);
        setupEvent(closeClip, 0);
	}

    private void setupEvent(string clipName, int param) {
        AnimationClip clip = clipNamed(clipName);
        AnimationEvent ae = new AnimationEvent();
        ae.functionName = "handleSwitchEvent";
        ae.intParameter = param;
        ae.time = clip.length;
        clip.AddEvent(ae);
    }

    public void handleSwitchEvent(int isOn) {
        binaryToggle.state = isOn > 0;
    }


}
