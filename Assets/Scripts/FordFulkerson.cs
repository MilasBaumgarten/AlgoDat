using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AnimationQueue;
using Model;

public class FordFulkerson : MonoBehaviour { //Erstellt von Tim und Sebastian

	private static bool run; //gibt Auskunft darüber, ob Algorithmus ausgeführt werden kann
	private static int leftCapacity; //gibt Auskunft über die auf der Edge verbleibende Kapazität
	private List<int> minimalFlow = new List<int>(); //gibt Auskunft über die im aktuellen Druchlauf kleinste benutzte Kapazität, die danach addiert/subtrahiert wird
	private static List<Edge> Wayforward = new List<Edge>(); //gibt Auskunft über die beim aktuellen Durchgang abgelaufenen Edges in Vorwärtige Richtung
	private static List<Edge> Waybackward = new List<Edge>(); //gibt Auskunft über die beim aktuellen Durchgang abgelaufenen Edges in Rückwärtige Richtung
	private static int MaxFlow =0; //gibt Auskunft über den maximalen Fluss des Graphen
	private static List<Edge> connectedEdges;
	public static Node actualN;
	CController Cc;
	private List<bool> wentForward = new List<bool>(); // speichert in welche Richtung sich bewegt wurde
	private int killswitch = 50;

	void Start(){
		GameObject sd = GameObject.FindWithTag("GraphController");
		Cc = sd.GetComponent<CController>();
	}

	private void vorarbeit(){ // Methode, welche grundlegende Schritte ausführt, um FFA (Ford Fulkerson Algorithmus) funktionsfähig zu machen

		if (Cc.getSource() != null && Cc.getSink () != null && !Cc.getSource().getName().Equals(Cc.getSink().getName())) {   //prüfen, ob Quelle und Senke voranden und nicht gleich sind

			//Variablen der Klasse zurücksetzen, um eventuell zwischengespeicherte Werte bei Neustart zu löschen
			run = true;
			minimalFlow.Clear();
			leftCapacity = 0;

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
		killswitch--;
		if (killswitch < 0){
			kill();
			return;
		}
		
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
					}

					actualN = Cc.GetNodeAsObject(actualEdge.getEnd ());// zu betrachtender Knoten wird gewechselt auf den Knoten, der am Ende der Edge befindlich ist (Frage bei Rückläufigen Edges???)

					flowThroughEdge(actualN);
				}
			}
			//kill();
		}
	}

	private void flowThroughEdge(Node currentNode){
		killswitch--;
		if (killswitch < 0){
			kill();
			return;
		}
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

				string tmp = currentEdge.getStart();
				currentEdge.setStart(currentEdge.getEnd());
				currentEdge.setEnd(tmp);

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
		// TODO: fixe Animationsreihenfolge ()
		for (int i = 0; i < Wayforward.Count; i++) {//Iteration über alle im Weg zwischen Quelle und Senke befindlichen Kanten des aktuellen Durchlaufes
				Wayforward [i].setFlow(Wayforward [i].getFlow() + minimalFlow[0]);
				AnimationManager.AM.addAnimation(new FillEdgeForward(Wayforward [i].getObject(), Wayforward [i].getFlow(), Wayforward [i].getCapacity()));
		}
		for (int i = 0; i < Waybackward.Count; i++){
				Waybackward [i].setFlow (Waybackward [i].getFlow () - minimalFlow[0]);// Erhöhung verbleibender Kapazität der Kanten, um deren aktuellen rückläufigen Durchfluss
				// WIP Animation
				//Vector3 lineStart = Waybackward [i].getObject().GetComponent<LineRenderer>().GetPosition(0);
				//Vector3 lineEnd = Waybackward [i].getObject().GetComponent<LineRenderer>().GetPosition(1);
				//Waybackward [i].getObject().GetComponent<LineRenderer>().SetPositions(new Vector3[] {lineEnd, lineStart});
				AnimationManager.AM.addAnimation(new FillEdgeForward(Waybackward [i].getObject(), Waybackward [i].getFlow(), Waybackward [i].getCapacity()));
		}

		if(walkThrough){ //Test ob kompletter Durchlauf angewählt wurde
			fordFulkerson (true);//rekursiver Neuaufruf des FFA
		}
	}

	private void kill(){ //Beendungsmethode
			//Ausgabe auf Ausgabelabelfeld anpassen!!!!!!
			Debug.Log("MaxFlow: " + MaxFlow);	//Ausgabe des maximalen Flusses
			run = false;//Ausführvariabel auf Stopp setzen
	}

	// gehe zu letztem besuchten Knoten zurück, mache Änderungen auf dem Weg rückgängig
	private void goBack(){
		if (minimalFlow.Count > 1){
			minimalFlow.RemoveAt(minimalFlow.Count - 1);
		}

		// lösche letzten besuchten Knoten aus Wegliste
		if(wentForward[wentForward.Count - 1]){
			Wayforward.RemoveAt(Wayforward.Count - 1);
		}
		else{
			// mache Richtungsänderung rückgängig
			string tmp = Waybackward[Waybackward.Count - 1].getStart();
			Waybackward[Waybackward.Count - 1].setStart(Waybackward[Waybackward.Count - 1].getEnd());
			Waybackward[Waybackward.Count - 1].setEnd(tmp);

			Waybackward.RemoveAt(Waybackward.Count - 1);
		}

		wentForward.RemoveAt(wentForward.Count - 1);

		// gehe zu vorletzetem besuchten Knoten zurück
		if (wentForward[wentForward.Count - 1]){
			wentForward.RemoveAt(wentForward.Count - 1);

			if (Wayforward.Count >= 1){
				flowThroughEdge(Cc.GetNodeAsObject(Wayforward[Wayforward.Count - 1].getEnd()));
			}
			else{
				terminate(false);
				kill();
			}
		}
		else{
			wentForward.RemoveAt(wentForward.Count - 1);
			if (Waybackward.Count >= 1){
				flowThroughEdge(Cc.GetNodeAsObject(Waybackward[Waybackward.Count - 1].getStart()));
			}
			else{
				terminate(false);
				kill();
			}
		}
	}
}