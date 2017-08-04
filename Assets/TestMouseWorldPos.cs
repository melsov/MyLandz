using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouseWorldPos : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = CursorInput.Instance.mouseWorldPosition;
        pos.z = 0f;
        transform.position = pos;
	}
}
