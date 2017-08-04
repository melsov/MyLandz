using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUseAnimtionComp : MonoBehaviour {

    public Animation anim;


	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
        foreach(AnimationState state in anim) {
            print(state.normalizedTime);
        }
        anim.Play();
	}
	
	// Update is called once per frame
	void Update () {
      foreach(AnimationState state in anim) {
            print(state.normalizedTime);
        }
	}
}
