using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateEdge : MonoBehaviour {

	// Dauer der Animation
	public float animationDuration = 1f;
	private float currentTime = 0f;
	public float speed = 3f;

	// Zielposition bzw. Ende der Kante ab
	[SerializeField]
	Vector2 destinationPos = new Vector2();

	// ob Kante gerade animiert wird
	bool animating = false;

	// verkürze Kommunikation mit LineRenderer Komponente über Variable
	LineRenderer line;

	void Start(){
		line = GetComponent<LineRenderer>();
	}

	void LateUpdate () {
		if (animating){
			animate();
		}
	}

	void animate(){
		// speichere Startpunkt um Zielpunktberechnung übersichtlicher zu machen
			Vector2 start = line.GetPosition(0);
			// setze und berechne Zielpunkt
			//	- errechne Richtungsvektor (normalisiert)
			//	- multipliziere Richtungsvektor mit Geschwindigkeit und vergangener Zeit
			line.SetPosition(1, start + (destinationPos - start).normalized * speed * currentTime);

			// erhöhe "Zeitmessung"
			currentTime += Time.deltaTime;

			// Animation ist beendet, sobald wirkliche Länge der Kante > gewollte Länge
			if (Vector2.Distance(start, destinationPos) < Vector2.Distance(start, line.GetPosition(1))){
				line.SetPosition(1, destinationPos);
				animating = false;
				currentTime = 0;
			}
	}

	// rufe auf um Animation der Kante zu starten
	public void startAnimation(){
		animating = true;
	}

//#####################################################################
// GET			SET

	// übergebe Zielposition bzw. Ende der Kante
	// Startposition = transform.position (schon bei der Initialisierung gesetzt)
	public void setDestination(float x, float y){
		// Teste ob Start == Ziel
		// sollte NIE gewollt sein, sonst ist keine Animation möglich
		if (line.GetPosition(0) != new Vector3(x,y,0)){
			destinationPos = new Vector2(x,y);

			line.SetPosition(1, destinationPos);
		}
		else{
			Debug.LogError( "Zielposition darf NICHT gleich Startposition sein!! Wurde auf (0,0,0) gesetzt.\n" +
							"- LG Milas");
		}
		
	}

	// gibt zurück ob Animation abgeschlossen ist
	// -> gibt Negation von animating zurück
	public bool getIsFinished(){
		return !animating;
	}
}
//23:30