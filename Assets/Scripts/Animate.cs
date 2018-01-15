﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour {

	public GameObject edge;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			if (edge != null){
				AnimateEdge edgeAnim = edge.GetComponent<AnimateEdge>();
				if (edgeAnim != null){
					if (edgeAnim.getIsFinished()){
						edgeAnim.setDestination(Random.Range(-5,5), Random.Range(-5,5));
						//edgeAnim.setDestination(-2, 0);
						edgeAnim.startAnimation();
					}
				}
			}
		}
	}
}
