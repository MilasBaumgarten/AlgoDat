using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public class Animate : MonoBehaviour {

	public GameObject edge;
	private int flow = 0;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			if (edge != null){
				flow ++;
				//GetComponent<AnimationManager>().Add(new FillEdgeForward(edge, flow));
				AnimationManager.AM.addAnimation(new FillEdgeForward(edge, flow, 3));
				Debug.Log(flow);
			}
		}

		if (Input.GetKeyDown(KeyCode.Break)){
			if (edge != null){
				GetComponent<AnimationManager>().addAnimation(new AnimateVertex());
			}
		}
	}
}
