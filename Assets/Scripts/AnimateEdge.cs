using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateEdge : MonoBehaviour {

	[Header("Animation")]
	// Animationsgeschwindigkeit und Zeit
	[SerializeField]
	private float speed = 3f;
	private float currentTime = 0f;
	// ob Kante gerade animiert wird
	private bool animating = false;

	[Header("Capacity")]
	// Kapazität der Kante
	[SerializeField]
	private float widthMultiplier = 1;
	[SerializeField]
	private float maxWidth = 2;
	private int maxCapacity = -1;
	private int usedCapacity = 0;

	// Zielposition bzw. Ende der Kante ab
	private Vector2 destinationPos = new Vector2();

	// verkürze Kommunikation mit LineRenderer Komponente über Variable
	private LineRenderer main_line;
	private LineRenderer animated_line;

	void Start(){
		main_line = GetComponent<LineRenderer>();
		animated_line = transform.GetChild(0).GetComponent<LineRenderer>();
	}

	void LateUpdate () {
		if (animating){
			animate();
		}
	}

	void animate(){
		// speichere Startpunkt um Zielpunktberechnung übersichtlicher zu machen
			Vector2 start = main_line.GetPosition(0);
			// setze und berechne Zielpunkt
			//	- errechne Richtungsvektor (normalisiert)
			//	- multipliziere Richtungsvektor mit Geschwindigkeit und vergangener Zeit
			// -1 bei z, damit farbige Kante vor grauer Kante gezeichnet wird
			animated_line.SetPosition(1, new Vector3(start.x, start.y, -1) + (new Vector3(destinationPos.x - start.x, destinationPos.y - start.y, -1)).normalized * speed * currentTime);

			// erhöhe "Zeitmessung"
			currentTime += Time.deltaTime;

			// Animation ist beendet, sobald wirkliche Länge der Kante > gewollte Länge
			if (Vector3.Distance(start, new Vector3(destinationPos.x, destinationPos.y, -1)) < Vector3.Distance(start, animated_line.GetPosition(1))){
				animated_line.SetPosition(1, new Vector3(destinationPos.x, destinationPos.y, -1));
				animating = false;
				currentTime = 0;
			}
	}

	// rufe auf um Animation der Kante zu starten
	public void startAnimation(){
		animating = true;
		animated_line.SetPosition(0, transform.position + Vector3.back);

		// Ändere Farbe abhängig von Kapazität
		usedCapacity ++;
		animated_line.material.EnableKeyword("_EMISSION");
		Debug.Log((float)1-usedCapacity/maxCapacity + " _ " + (float)usedCapacity/maxCapacity);
		animated_line.material.SetColor("_EmissionColor", new Color((float)usedCapacity/maxCapacity, (float)1-usedCapacity/maxCapacity, 0));
	}

//#####################################################################
// GET			SET

	// setze Kapazität bis Maximalwert
	public void setMaxCapacity(int capacity){
		maxCapacity = capacity;
		main_line.widthMultiplier = Mathf.Min(capacity * widthMultiplier, maxWidth);
		animated_line.widthMultiplier = Mathf.Min(capacity * widthMultiplier, maxWidth);
	}

	// übergebe Zielposition bzw. Ende der Kante
	public void setDestination(float x, float y){
		// Startposition = transform.position
		main_line.SetPosition(0, transform.position);
		// verstecke Kante hinter grauer Kante, wenn nicht animiert wird
		animated_line.SetPosition(0, transform.position + Vector3.forward);

		// Teste ob Start == Ziel
		// sollte NIE gewollt sein, sonst ist keine Animation möglich
		if (main_line.GetPosition(0) != new Vector3(x,y,0)){
			destinationPos = new Vector2(x,y);

			main_line.SetPosition(1, destinationPos);
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