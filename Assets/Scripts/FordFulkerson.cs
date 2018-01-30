using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AnimationQueue;
using Model;
using TMPro;

public class FordFulkerson : MonoBehaviour { //Erstellt von Tim und Sebastian

	private static bool run; //gibt Auskunft darüber, ob Algorithmus ausgeführt werden kann
	private List<int> minimalFlow = new List<int>(); //gibt Auskunft über die im aktuellen Druchlauf kleinste benutzte Kapazität, die danach addiert/subtrahiert wird
	private static List<Edge> Wayforward = new List<Edge>(); //gibt Auskunft über die beim aktuellen Durchgang abgelaufenen Edges in Vorwärtige Richtung
	private static List<Edge> Waybackward = new List<Edge>(); //gibt Auskunft über die beim aktuellen Durchgang abgelaufenen Edges in Rückwärtige Richtung
	private static int MaxFlow =0; //gibt Auskunft über den maximalen Fluss des Graphen
	private static List<Edge> connectedEdges;
	public static Node actualN;
	CController Cc;
	private List<bool> wentForward = new List<bool>(); // speichert in welche Richtung sich bewegt wurde

	public TextMeshProUGUI debugText; // Ausgabe in GUI

	void Start(){
		Cc = GameObject.FindWithTag("GraphController").GetComponent<CController>();
	}

	private void vorarbeit(){ // Methode, welche grundlegende Schritte ausführt, um FFA (Ford Fulkerson Algorithmus) funktionsfähig zu machen

		if (Cc.getSource() != null && Cc.getSink () != null && !Cc.getSource().getName().Equals(Cc.getSink().getName())) {   //prüfen, ob Quelle und Senke voranden und nicht gleich sind

			//Variablen der Klasse zurücksetzen, um eventuell zwischengespeicherte Werte bei Neustart zu löschen
			run = true;
			minimalFlow.Clear();

			Waybackward.Clear();
			Wayforward.Clear();

			List<Edge> allEdges =Cc.getAllEdges();
			for (int i = 0; i < Cc.getAllEdges().Count; i++) { //Flow aller Edges auf 0 setzen; wird standartmaessig mit 0 belegt
				
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

			for (int i = 0; i < connectedEdges.Count; i++) { //iterieren über alle am Knoten anliegenden Kanten
				if (!connectedEdges[i].getVisited() && !connectedEdges[i].getEnd().Equals(actualN.getName()) && connectedEdges [i].getFlow() < connectedEdges [i].getCapacity()) { // Check ob aktuell betrachtete Kante noch freie Kapazitäten für den Fluss hat, erste passende Kante wird gewählt
					//Blink gewählter passender Edge hier noch implementieren !!! Eigene Methode ???
					Edge actualEdge = connectedEdges [i]; // Auswahl erster passender zu betrachtender Edge

					minimalFlow.Add(actualEdge.getCapacity() - actualEdge.getFlow()); // kleinster Fluss wird als Referenzwert mit der maximalen Kapazität der Quelle initialisiert, CapacityMax = Dicke des Lasers, statische Zahl aus tabelle wird dargestellt
					
					if(actualN.getName().Equals(actualEdge.getStart())){
						Wayforward.Add (actualEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
						actualEdge.setVisited(true);
						wentForward.Add(true);
					}

					actualN = Cc.GetNodeAsObject(actualEdge.getEnd ());// zu betrachtender Knoten wird gewechselt auf den Knoten, der am Ende der Edge befindlich ist (Frage bei Rückläufigen Edges???)

					flowThroughEdge(actualN);
				}
			}
		}
	}

	private void flowThroughEdge(Node currentNode){
		// mit Quellknoten verbundene Kanten werden ermittelt und zwischengespeichert
		List<Edge> connectedEdges = new List<Edge>(currentNode.getConnectedEdges());
		Edge currentEdge;

		// säubere Liste angrenzender Kanten
		for(int i = connectedEdges.Count - 1; i >= 0; i--){
			// lösche alle eingehenden Kanten aus Auswahl (falsche Richtung & kann nicht Rückwärtskante werden)
			if (connectedEdges[i].getEnd().Equals(currentNode.getName()) && connectedEdges[i].getCapacity() > connectedEdges[i].getFlow()){
				connectedEdges.RemoveAt(i);
			}
			// lösche alle besuchten Kanten aus Auswahl
			else if (connectedEdges[i].getVisited()){
				connectedEdges.RemoveAt(i);
			}
			// lösche alle vollen Kanten aus Auswahl (nur Vorwärtskanten)
			else if (connectedEdges[i].getStart().Equals(currentNode.getName()) && connectedEdges[i].getFlow() >= connectedEdges[i].getCapacity()){
				connectedEdges.RemoveAt(i);
			}
		}

		// wenn die Senke erreicht wurde
		if (currentNode.isSink){
			terminate(true);
		}
		else if (connectedEdges.Count > 0){
			currentEdge = connectedEdges[0];
			currentEdge.setVisited(true);

			// Rückwärtskante
			if (currentEdge.getEnd().Equals(currentNode.getName()) && currentEdge.getCapacity() <= currentEdge.getFlow()){
				Waybackward.Add (currentEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
				wentForward.Add(false);

				minimalFlow.Add(currentEdge.getCapacity());

				// durchlaufe nächste Kante
				flowThroughEdge(Cc.GetNodeAsObject(currentEdge.getStart()));
			}
			// Vorwärtskante
			else if (currentEdge.getStart().Equals(currentNode.getName())){
				Wayforward.Add (currentEdge);
				wentForward.Add(true);

				minimalFlow.Add(currentEdge.getCapacity() - currentEdge.getFlow ());

				// durchlaufe nächste Kante
				flowThroughEdge(Cc.GetNodeAsObject(currentEdge.getEnd()));
			}
		}
		// wenn Sackgasse gefunden
		else{
			goBack();
		}
	}

	private void terminate(bool walkThrough){ //Terminieren des Algorithmus nach einem Erfolgreichen Durchlauf (Schritt), Walkthrough gibt abspielart an
		minimalFlow.Sort();
		
		MaxFlow += minimalFlow[0]; // Inkrimentierung des Maximalen Flusses, um den, im aktuellen Durchgang, kleinsten benutzten Fluss

		int indexForward = 0;
		int indexBackward = 0;
		foreach (bool wf in wentForward){
			if (wf){
				Wayforward [indexForward].setFlow(Wayforward [indexForward].getFlow() + minimalFlow[0]);
				AnimationManager.AM.addAnimation(new FillEdgeForward(Wayforward [indexForward].getObject(), Wayforward [indexForward].getFlow(), Wayforward [indexForward].getCapacity()));
				indexForward++;
			}
			else{
				Waybackward [indexBackward].setFlow (Waybackward [indexBackward].getFlow () - minimalFlow[0]);// Erhöhung verbleibender Kapazität der Kanten, um deren aktuellen rückläufigen Durchfluss
				// WIP Animation
				//Vector3 lineStart = Waybackward [indexBackward].getObject().GetComponent<LineRenderer>().GetPosition(0);
				//Vector3 lineEnd = Waybackward [indexBackward].getObject().GetComponent<LineRenderer>().GetPosition(1);
				//Waybackward [indexBackward].getObject().GetComponent<LineRenderer>().SetPositions(new Vector3[] {lineEnd, lineStart});
				AnimationManager.AM.addAnimation(new FillEdgeForward(Waybackward [indexBackward].getObject(), Waybackward [indexBackward].getFlow(), Waybackward [indexBackward].getCapacity()));
				indexBackward ++;
			}
		}

		wentForward.Clear();

		if(walkThrough){ //Test ob kompletter Durchlauf angewählt wurde
			fordFulkerson (true);//rekursiver Neuaufruf des FFA
		}
	}

	// gehe zu letztem besuchten Knoten zurück, mache Änderungen auf dem Weg rückgängig
	private void goBack(){
		if (minimalFlow.Count >= 1){
			minimalFlow.RemoveAt(minimalFlow.Count - 1);
		}

		// lösche letzten besuchten Knoten aus Wegliste
		if (wentForward.Count > 1){
			if(wentForward[wentForward.Count - 1]){
				Wayforward.RemoveAt(Wayforward.Count - 1);
			}
			else{
				Waybackward.RemoveAt(Waybackward.Count - 1);
			}

			wentForward.RemoveAt(wentForward.Count - 1);

			// gehe zu vorletzetem besuchten Knoten zurück
			if (wentForward[wentForward.Count - 1]){
				if (Wayforward.Count >= 1){
					flowThroughEdge(Cc.GetNodeAsObject(Wayforward[Wayforward.Count - 1].getEnd()));
				}
			}
			else{
				if (Waybackward.Count >= 1){
					flowThroughEdge(Cc.GetNodeAsObject(Waybackward[Waybackward.Count - 1].getStart()));
				}
			}
		}
		else{
			kill();
		}
	}

	private void kill(){ //Beendungsmethode
			//Ausgabe auf Ausgabelabelfeld anpassen!!!!!!
			Debug.Log("MaxFlow: " + MaxFlow);	//Ausgabe des maximalen Flusses
			debugText.text = "MaxFlow: " + MaxFlow;
			run = false;//Ausführvariabel auf Stopp setzen
	}
}