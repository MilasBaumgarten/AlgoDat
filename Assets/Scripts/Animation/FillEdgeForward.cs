﻿using System.Collections;
using UnityEngine;

namespace AnimationQueue{
	public class FillEdgeForward : BasicAction {

		// momentane Position des Kantenendes während der Animation
		private Vector3 currentPos;
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
			TextMesh textComponent = thisEdge.transform.GetComponentInChildren<TextMesh>();

			textComponent.text = flow + " / " + maxCapacity;

			// bewege Animierungskant vor Graphenkante
			animated_line.SetPosition(0, main_line.GetPosition(0) + Vector3.back);
			animated_line.SetPosition(1, main_line.GetPosition(0) + Vector3.back);

			currentPos = animated_line.GetPosition(0);

			setMaterialColor(animated_line.material);

			while (animating){
				animate();
				yield return new WaitForEndOfFrame();
			}
			clean();
		}

		void animate(){
			float speed = AnimationManager.AM.speed;
			// errechne Richtungsvektor (normalisiert) -> Änderung des Endpunktes pro Frame
			Vector3 direction = (main_line.GetPosition(1) - animated_line.GetPosition(0)).normalized;

			// berechne Zielpunkt der Bewegung für diesen Frame
			currentPos += direction * speed * Time.deltaTime;
			// ändere Endposition in Richtung Zielposition
			animated_line.SetPosition(1, currentPos);

			// Animation ist beendet, sobald wirkliche Länge der Kante > gewollte Länge
			if (Vector3.Distance(main_line.GetPosition(0), main_line.GetPosition(1)) <
				Vector3.Distance(animated_line.GetPosition(0), animated_line.GetPosition(1))){
				// beende Animation
				animating = false;
			}
		}

		void clean(){
			setMaterialColor(main_line.material);

			animated_line.SetPosition(0, main_line.GetPosition(0) + Vector3.forward);
			animated_line.SetPosition(1, main_line.GetPosition(1) + Vector3.forward);
		}

		void setMaterialColor(Material mat){
			// ermögliche Einfärbung der Emission
			mat.EnableKeyword("_EMISSION");

			// bestimme farbe
			Color fillColor = new Color(0,0,0);
			float capacityPercentage = (float) flow / maxCapacity;
			fillColor.r = (capacityPercentage) * capacityPercentage;
			fillColor.g = (1.0f - capacityPercentage) * capacityPercentage;

			// färbe ein
			mat.SetColor("_EmissionColor", fillColor);
		}
	}
}