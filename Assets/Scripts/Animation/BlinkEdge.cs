/*using System.Collections;
using UnityEngine;
using Model;

namespace Event{
	public class BlinkEdge : BasicAction {

		// wie lange die Animation geht
		private float duration;
		// wie oft die Edge blinkt
		private int blinkAmount;
		private float currentTime = 0f;

		// ob Kante gerade animiert wird
		private bool animating = true;

		// dieses GameObject, muss wegen AnimationQueue übergeben werden
		private GameObject thisEdge;

		// verkürze Kommunikation mit LineRenderer Komponente über Variable
		private LineRenderer main_line;

		private Color originalColor;
		private Color blinkColor;

		// Konstruktor, um Parameter zu übergeben
		public BlinkEdge(GameObject edge, Color blinkColor, float duration, int blinkAmount){
			this.thisEdge = edge;
			this.blinkColor = blinkColor;
			this.duration = duration;
			this.blinkAmount = blinkAmount;

			this.originalColor = edge.GetComponent<LineRenderer>().material.color;
		}


		public override IEnumerator Run() {
			// vereinfacht nachfolgenden Code
			Material mat = thisEdge.GetComponent<LineRenderer>().material;



			// färbe Animationskante ein
			animated_line.material.EnableKeyword("_EMISSION");
			animated_line.material.SetColor("_EmissionColor", new Color((float)flow/maxCapacity, 1.0f - (float)flow/maxCapacity, 0));

			while (animating){
				animate();
				yield return new WaitForEndOfFrame();
			}
		}

		void animate(){
			float speed = AnimationManager.AM.speed;
			

			// erhöhe "Zeitmessung"
			currentTime += Time.deltaTime;

			// Animation ist beendet, sobald wirkliche Länge der Kante > gewollte Länge
			if (Vector3.Distance(main_line.GetPosition(0), main_line.GetPosition(1)) <
				Vector3.Distance(animated_line.GetPosition(0), animated_line.GetPosition(1))){
				// beende Animation
				animating = false;
				currentTime = 0;
			}
		}
	}
}
*/