using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public class AnimationManager : MonoBehaviour {

	public float delay = 0.5f;

	private Queue<IAction> animationQueue = new Queue<IAction>();

	public void Add(IAction action){
		animationQueue.Enqueue(action);
	}

	void Start () {
		StartCoroutine(animateQueue());
	}
	
	IEnumerator animateQueue(){
		while(true){
			if (animationQueue.Count != 0){
				IAction action = animationQueue.Dequeue();
				yield return action.Run();
			}
			yield return new WaitForSeconds(delay);
		}
	}
}
