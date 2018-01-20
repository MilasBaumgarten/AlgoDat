﻿using System.Collections;
using UnityEngine;
using Model;

namespace Event{
	public class FillEdgeForward : BasicAction {
		private float currentTime = 0f;
		// ob Kante gerade animiert wird
		private bool animating = true;

		// dieses GameObject, muss wegen AnimationQueue übergeben werden
		private GameObject thisEdge;
		private int maxCapacity = -1;
		private int flow = 0;

		// verkürze Kommunikation mit LineRenderer Komponente über Variable
		private LineRenderer main_line;
		private LineRenderer animated_line;

		// Konstruktor, um Parameter zu übergeben
		public FillEdgeForward(GameObject edge, int flow, int maxCapacity){
			this.thisEdge = edge;
			this.flow = flow;
			this.maxCapacity = maxCapacity;
		}


		public override IEnumerator Run() {
			// vereinfacht nachfolgenden Code
			main_line = thisEdge.GetComponent<LineRenderer>();
			animated_line = thisEdge.transform.GetChild(0).GetComponent<LineRenderer>();

			// bewege Animierungskant vor Graphenkante
			animated_line.SetPosition(0, main_line.GetPosition(0) + Vector3.back);
			animated_line.SetPosition(1, main_line.GetPosition(0) + Vector3.back);

			// färbe Animationskante ein
			animated_line.material.EnableKeyword("_EMISSION");
			animated_line.material.SetColor("_EmissionColor", new Color((float)flow/maxCapacity, 1.0f - (float)flow/maxCapacity, 0));

			while (animating){
				animate();
				yield return new WaitForEndOfFrame();
			}
			clean();
		}

		void animate(){
			float speed = AnimationManager.AM.speed;
			// errechne Richtungsvektor (normalisiert) -> Änderung des Endpunktes pro Frame
			Vector3 direction = (main_line.GetPosition(1) - main_line.GetPosition(0)).normalized * speed * currentTime;
			// Richtungsvektor ausgehend von Startpunkt
			direction += main_line.GetPosition(0);
			// setze Tiefe (z) auf Animationsebene (0)
			direction.z = 0;
			// ändere Endposition in Richtung Zielposition
			animated_line.SetPosition(1, direction);

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

		void clean(){
			// färbe Graphenkante ein
			main_line.material.EnableKeyword("_EMISSION");
			main_line.material.SetColor("_EmissionColor", new Color((float)flow/maxCapacity, 1.0f - (float)flow/maxCapacity, 0));

			animated_line.SetPosition(0, main_line.GetPosition(0) + Vector3.forward);
			animated_line.SetPosition(1, main_line.GetPosition(1) + Vector3.forward);
		}
	}
}