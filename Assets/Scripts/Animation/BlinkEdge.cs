using System.Collections;
using UnityEngine;
using Model;

namespace Event{
	public class BlinkEdge : BasicAction {

		// wie lange die Animation geht
		private float duration;
		// wie oft die Edge blinkt
		private int blinkAmount;

		// dieses GameObject, muss wegen AnimationQueue übergeben werden
		private GameObject thisEdge;

		private Color blinkColor;

		// Konstruktor, um Parameter zu übergeben
		public BlinkEdge(GameObject edge, Color blinkColor, float duration, int blinkAmount){
			this.thisEdge = edge;
			this.blinkColor = blinkColor;
			this.duration = duration;
			this.blinkAmount = blinkAmount;
		}


		public override IEnumerator Run() {
			// vereinfacht nachfolgenden Code
			Color originalColor = thisEdge.GetComponent<LineRenderer>().material.color;

			float speed = AnimationManager.AM.speed;

			for (int i = 0; i < blinkAmount; i++){
				yield return (duration / (speed * blinkAmount));
				blink(originalColor);
				yield return (duration / (speed * blinkAmount));
				blink(blinkColor);
			}
		}

		void blink(Color color){
			Material mat = thisEdge.GetComponent<LineRenderer>().material;

			// färbe Animationskante ein
			mat.EnableKeyword("_EMISSION");
			mat.SetColor("_EmissionColor", color);
		}
	}
}