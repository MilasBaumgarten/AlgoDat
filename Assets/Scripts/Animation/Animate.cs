using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationQueue;

public class Animate : MonoBehaviour {

	public GameObject edge;
	private int flow = 0;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			if (edge != null){
				flow ++;
				//AnimationManager.AM.addAnimation(new FillEdgeForward(edge, flow, 10));
				AnimationManager.AM.addAnimation(new BlinkEdge(edge, Color.yellow, 5, 5));
			}
		}

		if (Input.GetKeyDown(KeyCode.Break)){
			if (edge != null){
				GetComponent<AnimationManager>().addAnimation(new AnimateVertex());
			}
		}
	}
}
