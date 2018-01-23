using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AnimationQueue;
using Model;

public class FordFulkerson : MonoBehaviour { //Erstellt von Tim und Sebastian

	private static bool run; //gibt Auskunft darüber, ob Algorithmus ausgeführt werden kann
	private static int leftCapacity; //gibt Auskunft über die auf der Edge verbleibende Kapazität
	private static int minimalFlow; //gibt Auskunft über die im aktuellen Druchlauf kleinste benutzte Kapazität, die danach addiert/subtrahiert wird
	private static List<Edge> Wayforward = new List<Edge>(); //gibt Auskunft über die beim aktuellen Durchgang abgelaufenen Edges in Vorwärtige Richtung
	private static List<Edge> Waybackward = new List<Edge>(); //gibt Auskunft über die beim aktuellen Durchgang abgelaufenen Edges in Rückwärtige Richtung
	private static int MaxFlow =0; //gibt Auskunft über den maximalen Fluss des Graphen
	private static List<Edge> connectedEdges;
	public static Node actualN;
	CController Cc;

	void Start(){
		GameObject sd = GameObject.FindWithTag("GraphController");
		Cc = sd.GetComponent<CController>();
	}

	private void vorarbeit(){ // Methode, welche grundlegende Schritte ausführt, um FFA (Ford Fulkerson Algorithmus) funktionsfähig zu machen
		if (Cc.getSource() != null && Cc.getSink () != null && !Cc.getSource().getName().Equals(Cc.getSink().getName())) {   //prüfen, ob Quelle und Senke voranden und nicht gleich sind

			//Variablen der Klasse zurücksetzen, um eventuell zwischengespeicherte Werte bei Neustart zu löschen
			run = true;
			minimalFlow = 0;
			leftCapacity = 0;

			Waybackward.Clear();
			Wayforward.Clear();

			List<Edge> allEdges =Cc.getAllEdges();
			for (int i = 0; i < Cc.getAllEdges().Count; i++) {  //Flow aller Edges auf 0 setzen; wird standartmaessig mit 0 belegt
				
				allEdges [i].setVisited(false);
			}
		}
		else {
			Debug.Log("Quelle und Senke sind nicht bestimmt!");
			//Fehlermeldung im Dialogfenster ausgeben, dass Quelle und Senke nicht gewählt sind -> noch implementieren !!!
			kill();// Algorithmus beenden und maximalen Fluss ausgeben
		}
	}

	public void fordFulkerson(bool walkThrough){ // eigentlicher Algorithmus, Walkthrough Variable gibt Auskunft darüber, welche Darstellungsart Nutzer angecklickt hat -> Schrittweise oder am Stück
		vorarbeit (); // oben vorbereitet und erläuterte Vorarbeit einmalig ausführen vor Start 
		if (run) { // Test, ob Algorithmus noch ausgeführt werden soll bzw. darf
			actualN = Cc.getSource (); // aktuell betrachteter Knoten ist die gewählte Quelle
			connectedEdges = actualN.getConnectedEdges (); // mit Quellknoten verbundene Kanten werden ermittelt und zwischengespeichert
			Debug.Log("Algorithmus gestartet!");

			for (int i = 0; i < connectedEdges.Count; i++) { //iterieren über alle am Knoten anliegenden Kanten
				Debug.LogWarning("i: " + i + " " + connectedEdges[i].getEdgeName());
				if (!connectedEdges[i].getVisited() && !connectedEdges[i].getEnd().Equals(actualN.getName()) && connectedEdges [i].getFlow() < connectedEdges [i].getCapacity()) { // Check ob aktuell betrachtete Kante noch freie Kapazitäten für den Fluss hat, erste passende Kante wird gewählt
					//Blink gewählter passender Edge hier noch implementieren !!! Eigene Methode ???
					Edge actualEdge = connectedEdges [i]; // Auswahl erster passender zu betrachtender Edge

					minimalFlow = actualEdge.getCapacity() - actualEdge.getFlow(); // kleinster Fluss wird als Referenzwert mit der maximalen Kapazität der Quelle initialisiert, CapacityMax = Dicke des Lasers, statische Zahl aus tabelle wird dargestellt
					
					if(actualN.getName().Equals(actualEdge.getStart())){
						Debug.Log("Actual Edge: " + actualEdge.getEdgeName());	
						Wayforward.Add (actualEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
						actualEdge.setVisited(true);
						Debug.Log("Wayforward Länge: " + Wayforward.Count);
					}

					actualN = Cc.GetNodeAsObject(actualEdge.getEnd ());// zu betrachtender Knoten wird gewechselt auf den Knoten, der am Ende der Edge befindlich ist (Frage bei Rückläufigen Edges???)

					flowThroughEdge(actualN);
				}
			}
			kill();
		}
	}

	private void flowThroughEdge(Node currentNode){
		// mit Quellknoten verbundene Kanten werden ermittelt und zwischengespeichert, bereits besuchte Kanten von Betrachtung ausschliessen??? -> Edge[] connectedEdges = actualN.getConnectedEdges ()- Way[]; ???
		List<Edge> connectedEdges = new List<Edge>(currentNode.getConnectedEdges());
		Edge currentEdge;

		for(int i = connectedEdges.Count - 1; i >= 0; i--){
			// lösche alle eingehenden Kanten aus Auswahl (falsche Richtung & kann nicht Rückwärtskante werden)
			//if (connectedEdges[i].getEnd().Equals(currentNode.getName()) && connectedEdges[i].getCapacity() > connectedEdges[i].getFlow()){
			if (connectedEdges[i].getEnd().Equals(currentNode.getName())){
				Debug.LogWarning("Removed backwards Edge: " + connectedEdges[i].getEdgeName());
				connectedEdges.RemoveAt(i);
			}
			// lösche alle besuchten Kanten aus Auswahl
			else if (connectedEdges[i].getVisited()){
				Debug.LogWarning("Removed visited Edge: " + connectedEdges[i].getEdgeName());
				connectedEdges.RemoveAt(i);
			}
			// lösche alle vollen Kanten aus Auswahl (nur Vorwärtskanten)
			//else if (connectedEdges[i].getStart().Equals(currentNode.getName()) && connectedEdges[i].getFlow() >= connectedEdges[i].getCapacity()){
			else if (connectedEdges[i].getFlow() >= connectedEdges[i].getCapacity()){
				Debug.LogWarning("Removed full forward Edge: " + connectedEdges[i].getEdgeName());
				connectedEdges.RemoveAt(i);
			}
		}

		Debug.Log("connected Edges: " + connectedEdges.Count);

		// wenn noch nicht Senke/ Sackgasse gefunden
		if (connectedEdges.Count > 0){
			currentEdge = connectedEdges[0];

			// Rückwärtskante
			if (currentEdge.getEnd().Equals(currentNode.getName()) && currentEdge.getCapacity() <= currentEdge.getFlow()){
				Waybackward.Add (currentEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
				
				string tmp = currentEdge.getStart();
				currentEdge.setStart(currentEdge.getEnd());
				currentEdge.setEnd(tmp);
				Debug.LogError("backwards Edge");
			}
			// Vorwärtskante
			else if (currentEdge.getStart().Equals(currentNode.getName())){
				Wayforward.Add (currentEdge);
			}

			currentEdge.setVisited(true);

			leftCapacity = currentEdge.getCapacity() - currentEdge.getFlow ();

			if (minimalFlow > leftCapacity) { //Test ob dynamischer Wert des kleinsten im Durchlauf verwendeten Flusses erneuert werden muss
				minimalFlow = leftCapacity; //Wenn ja, wird dieser durch den aktuell verbleibenden Fluss auf der Edge ersetzt
			}

			// durchlaufe nächste Kante
			flowThroughEdge(Cc.GetNodeAsObject(currentEdge.getEnd()));
		}
		// Senke gefunden
		else if (currentNode.isSink){
			terminate(true);
		}
		else{
			vorarbeit();
			return;
		}
	}

/*	TESTE RÜCKWÄRTS
else if(actualN.getName().Equals(actualEdge.getEnd()) &&actualEdge.getFlow() != 0) {
	Waybackward.Add (actualEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
	actualEdge.setVisited(true);
}
*/

	private void terminate(bool walkThrough){ //Terminieren des Algorithmus nach einem Erfolgreichen Durchlauf (Schritt), Walkthrough gibt abspielart an
		MaxFlow += minimalFlow; // Inkrimentierung des Maximalen Flusses, um den, im aktuellen Durchgang, kleinsten benutzten Fluss
		Debug.Log("Wayforward Länge: " + Wayforward.Count);
		for (int i = 0; i < Wayforward.Count; i++) {//Iteration über alle im Weg zwischen Quelle und Senke befindlichen Kanten des aktuellen Durchlaufes
				Wayforward [i].setFlow(Wayforward [i].getFlow() + minimalFlow);
				AnimationManager.AM.addAnimation(new FillEdgeForward(Wayforward [i].getObject(), Wayforward [i].getFlow(), Wayforward [i].getCapacity()));
		}
		for (int i = 0; i < Waybackward.Count; i++){
				Waybackward [i].setFlow ((Waybackward [i].getCapacity () - Waybackward [i].getFlow ()) + minimalFlow);// Erhöhung verbleibender Kapazität der Kanten, um deren aktuellen rückläufigen Durchfluss
		}	

		Debug.Log("terminiert ");

		if(walkThrough){ //Test ob kompletter Durchlauf angewählt wurde
			fordFulkerson (true);//rekursiver Neuaufruf des FFA
		}
	}

	private void kill(){ //Beendungsmethode
			//Ausgabe auf Ausgabelabelfeld anpassen!!!!!!
			Debug.Log("MaxFlow: " + MaxFlow);	//Ausgabe des maximalen Flusses
			run = false;//Ausführvariabel auf Stopp setzen
	}
}