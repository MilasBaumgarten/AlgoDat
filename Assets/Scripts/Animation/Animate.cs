using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public class Animate : MonoBehaviour {

	public GameObject edge;
	private int usedCapacity = 0;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			if (edge != null){
				usedCapacity ++;
				GetComponent<AnimationManager>().Add(new FillEdgeForward(edge, usedCapacity));
			}
		}

		if (Input.GetKeyDown(KeyCode.Break)){
			if (edge != null){
				GetComponent<AnimationManager>().Add(new AnimateVertex());
			}
		}
	}
}
