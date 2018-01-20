using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public class AnimationManager : MonoBehaviour {

	public float delay = 0.5f;
	public float speed = 2;
	private bool speedChanged = true;

	private Queue<IAction> animationQueue = new Queue<IAction>();
     
    // mach ein Singleton -> kann von überall angesprochen werden
    public static AnimationManager AM { get; private set; }
     
    void Awake(){
        // teste ob es bereits eine Instanz gibt
        if(AM != null && AM != this)
        {
            // lösche diese Instanz
            Destroy(gameObject);
        }
 
        // wenn keine Instanz vorhanden -> das hier ist Instanz
        AM = this;
    }

	public void addAnimation(IAction action){
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

	public void changeAnimationSpeed(float speed){
		this.speed = speed;
	}
}
