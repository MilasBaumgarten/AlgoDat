using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Model;
using Event;

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

	int killCount = 5;


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
			
			if (Waybackward.Count > 0)
			{
				for (int i = 1; i < Waybackward.Count; i++)
				{
        			Waybackward.RemoveAt(i);
				}
			}
			if (Wayforward.Count > 0)
			{
				for (int i = 1; i < Waybackward.Count; i++)
				{
        			Waybackward.RemoveAt(i);
				}
			}

			List<Edge> allEdges =Cc.getAllEdges();
			for (int i = 1; i < Cc.getAllEdges().Count; i++) {  //Flow aller Edges auf 0 setzen; wird standartmaessig mit 0 belegt
				
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
		if (killCount >= 0){
			killCount--;
		}
		else{
			kill();
			return;
		}

		vorarbeit (); // oben vorbereitet und erläuterte Vorarbeit einmalig ausführen vor Start 
		if (run) { // Test, ob Algorithmus noch ausgeführt werden soll bzw. darf
			actualN = Cc.getSource (); // aktuell betrachteter Knoten ist die gewählte Quelle
			connectedEdges = actualN.getConnectedEdges (); // mit Quellknoten verbundene Kanten werden ermittelt und zwischengespeichert
			Debug.Log("Algorithmus gestartet!");

			for (int i = 1; i <= connectedEdges.Count - 1; i++) { //iterieren über alle am Knoten anliegenden Kanten
				Debug.Log("i: " + i + " " + connectedEdges[i].getEdgeName());
				if (!connectedEdges[i].getVisited() && !connectedEdges[i].getEnd().Equals(actualN.getName()) && connectedEdges [i].getFlow() < connectedEdges [i].getCapacity()) { // Check ob aktuell betrachtete Kante noch freie Kapazitäten für den Fluss hat, erste passende Kante wird gewählt
					//Blink gewählter passender Edge hier noch implementieren !!! Eigene Methode ???
					Edge actualEdge = connectedEdges [i]; // Auswahl erster passender zu betrachtender Edge
					Debug.LogWarning(connectedEdges [i].getFlow());

					minimalFlow = actualEdge.getCapacity (); // kleinster Fluss wird als Referenzwert mit der maximalen Kapazität der Quelle initialisiert, CapacityMax = Dicke des Lasers, statische Zahl aus tabelle wird dargestellt
					if(actualN.getName().Equals(actualEdge.getStart())){
						Debug.Log("Actual Edge: " + actualEdge.getEdgeName());	
						Wayforward.Add (actualEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
						actualEdge.setVisited(true);
						Debug.Log("Wayforward Länge: " + Wayforward.Count);
					}

					actualN = Cc.GetNodeAsObject(actualEdge.getEnd ());// zu betrachtender Knoten wird gewechselt auf den Knoten, der am Ende der Edge befindlich ist (Frage bei Rückläufigen Edges???)

					flowThroughEdge(actualN);

					/*if (actualN == Cc.getSink ()) { // Test ob neuer zu betrachtender Knoten die Senke ist
						terminate (walkThrough); // Wenn ja, terminiere den Algorithmus und starte neuen Durchlauf, da kompletter Weg von Quelle zu Senke gefunden wurde
						break; // Abbruch übergeordneter for Schleife
					}*/

					break; // Abbruch übergeordneter for Schleife
				}
				else if(i == connectedEdges.Count){
					kill ();// Algorithmus beenden und maximalen Fluss ausgeben
					break;// Abbruch übergeordneter for Schleife
				}
				Debug.Log("Ich bin hier!");
			}
		}
	}

	private void flowThroughEdge(Node currentNode){
		connectedEdges = currentNode.getConnectedEdges (); // mit Quellknoten verbundene Kanten werden ermittelt und zwischengespeichert, bereits besuchte Kanten von Betrachtung ausschliessen??? -> Edge[] connectedEdges = actualN.getConnectedEdges ()- Way[]; ???
		Edge currentEdge;

		for(int i = connectedEdges.Count - 1; i >= 0; i--){
			// lösche alle eingehenden Kanten aus Auswahl (flasche Richtung)
			if (connectedEdges[i].getEnd().Equals(currentNode.getName())){
				connectedEdges.RemoveAt(i);
			}
			// lösche alle besuchten Kanten aus Auswahl
			else if (connectedEdges[i].getVisited()){
				connectedEdges.RemoveAt(i);
			}
			// lösche alle vollen Kanten aus Auswahl
			else if (connectedEdges[i].getFlow() >= connectedEdges[i].getCapacity()){
				connectedEdges.RemoveAt(i);
			}
		}

		// wenn noch nicht Senke/ Sackgasse gefunden
		if (connectedEdges.Count > 0){
			currentEdge = connectedEdges[0];

			Debug.LogError(currentNode.getName() + ": " + currentEdge.getEdgeName());

			Wayforward.Add (currentEdge);
			currentEdge.setVisited(true);

			leftCapacity = currentEdge.getCapacity() - currentEdge.getFlow ();

			if (minimalFlow > leftCapacity) { //Test ob dynamischer Wert des kleinsten im Durchlauf verwendeten Flusses erneuert werden muss
				minimalFlow = leftCapacity; //Wenn ja, wird dieser durch den aktuell verbleibenden Fluss auf der Edge ersetzt
			}

			// durchlaufe nächste Kante
			flowThroughEdge(Cc.GetNodeAsObject(currentEdge.getEnd()));
		}
		// Senke gefunden
		else{
			terminate(true);
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