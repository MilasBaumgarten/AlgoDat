using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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


		

			for (int i = 0; i < Cc.getAllEdges().Count; i++) {  //Flow aller Edges auf 0 setzen; wird standartmaessig mit 0 belegt
				List<Edge> allEdges =Cc.getAllEdges();
			
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

			for (int i = 1; i <= connectedEdges.Count; i++) { //iterieren über alle am Knoten anliegenden Kanten
				Debug.Log("i: " + i + " " + connectedEdges[i].getEdgeName());
				if (!connectedEdges[i].getVisited() && !connectedEdges[i].getEnd().Equals(actualN.getName()) && connectedEdges [i].getFlow() < connectedEdges [i].getCapacity()) { // Check ob aktuell betrachtete Kante noch freie Kapazitäten für den Fluss hat, erste passende Kante wird gewählt
					//Blink gewählter passender Edge hier noch implementieren !!! Eigene Methode ???
					Edge actualEdge = connectedEdges [i]; // Auswahl erster passender zu betrachtender Edge
					minimalFlow = actualEdge.getCapacity (); // kleinster Fluss wird als Referenzwert mit der maximalen Kapazität der Quelle initialisiert, CapacityMax = Dicke des Lasers, statische Zahl aus tabelle wird dargestellt
						if(actualN.getName().Equals(actualEdge.getStart())){
							Debug.Log("Actual Edge: " + actualEdge.getEdgeName());	
							Wayforward.Add (actualEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
							actualEdge.setVisited(true);
							Debug.Log("Wayforward Länge: " + Wayforward.Count);					
						}
						
						actualN = Cc.GetNodeAsObject(actualEdge.getEnd ());// zu betrachtender Knoten wird gewechselt auf den Knoten, der am Ende der Edge befindlich ist  (Frage bei Rückläufigen Edges???)				
					
					if (actualN == Cc.getSink ()) { // Test ob neuer zu betrachtender Knoten die Senke ist
						terminate (walkThrough); // Wenn ja, terminiere den Algorithmus und starte neuen Durchlauf, da kompletter Weg von Quelle zu Senke gefunden wurde
						break; // Abbruch übergeordneter for Schleife
					}

					break; // Abbruch übergeordneter for Schleife
				}
				else if(i == connectedEdges.Count){
					kill ();// Algorithmus beenden und maximalen Fluss ausgeben
					break;// Abbruch übergeordneter for Schleife
				}
				Debug.Log("Ich bin hier!");
			}

			while (actualN != Cc.getSource ()) { // Betrachten restlicher im  Grafen verbliebener Knoten, solange diese nicht Senke oder Quelle sind
				connectedEdges = actualN.getConnectedEdges (); // mit Quellknoten verbundene Kanten werden ermittelt und zwischengespeichert, bereits besuchte Kanten von Betrachtung ausschliessen??? -> Edge[] connectedEdges = actualN.getConnectedEdges ()- Way[]; ???
				Node newNode = Cc.getSource();
				for (int i = 1; i < connectedEdges.Count; i++) {//iterieren über alle am Knoten anliegenden Kanten
					
				
					
					Debug.Log("Drin0 ");
					Debug.Log("i: " +i);
					Debug.Log("visited: " +connectedEdges[i].getVisited());
					Debug.Log("Flow: " +connectedEdges [i].getFlow());
					Debug.Log("Cap: " +connectedEdges [i].getCapacity());
					if (connectedEdges[i].getVisited() ==false && connectedEdges [i].getFlow() < connectedEdges [i].getCapacity()) {// Check ob aktuell betrachtete Kante noch freie Kapazitäten für den Fluss hat, erste passende Kante wird gewählt
						//Blink gewählter passender Edge hier noch implementieren !!! Eigene Methode ???
						Debug.Log("Drin1 ");
						Edge actualEdge = connectedEdges [i];// Auswahl erster passender zu betrachtender Edge
						leftCapacity = actualEdge.getCapacity() - actualEdge.getFlow (); // auf Edge verbleibende Kapazität wird aus der Subtraktion des aktuellen Durchflusses von der maximalen Kapazitzät errechnet

						if (minimalFlow > leftCapacity) { //Test ob dynamischer Wert des kleinsten im Durchlauf verwendeten Flusses erneuert werden muss
							minimalFlow = leftCapacity; //Wenn ja, wird dieser durch den aktuell verbleibenden Fluss auf der Edge ersetzt
						}
						if(actualN.getName().Equals(actualEdge.getStart())){
							Debug.Log("Drin2 ");
							Wayforward.Add (actualEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
							actualEdge.setVisited(true);
							Debug.Log("Wayforward Länge: " + Wayforward.Count);
						}else if(actualN.getName().Equals(actualEdge.getEnd()) &&actualEdge.getFlow() != 0) {
							Waybackward.Add (actualEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
							actualEdge.setVisited(true);
						}
				
						actualN = Cc.GetNodeAsObject(actualEdge.getEnd ());// zu betrachtender Knoten wird gewechselt auf den Knoten, der am Ende der Edge befindlich ist  (Frage bei Rückläufigen Edges???)
						Debug.Log("Aktuelles n: " + actualN.getName());
						
						if (actualN == Cc.getSink ()) {// Test ob neuer zu betrachtender Knoten die Senke ist
							terminate (walkThrough);// Wenn ja, terminiere den Algorithmus und starte neuen Durchlauf, da kompletter Weg von Quelle zu Senke gefunden wurde
							
						}
						//break;// Abbruch übergeordneter for Schleife
						

					}
					else if(i > connectedEdges.Count){
						kill ();// Algorithmus beenden und maximalen Fluss ausgeben
						break;// Abbruch übergeordneter for Schleife
					}
					
						Debug.Log(i);
				}
				break;
			}
		}
	}

	private void terminate(bool walkThrough){ //Terminieren des Algorithmus nach einem Erfolgreichen Durchlauf (Schritt), Walkthrough gibt abspielart an
		MaxFlow += minimalFlow; // Inkrimentierung des Maximalen Flusses, um den, im aktuellen Durchgang, kleinsten benutzten Fluss
		Debug.Log("Wayforward Länge: " + Wayforward.Count);
		for (int i = 1; i < Wayforward.Count; i++) {//Iteration über alle im Weg zwischen Quelle und Senke befindlichen Kanten des aktuellen Durchlaufes
				Wayforward [i].setFlow ((Wayforward [i].getCapacity () - Wayforward [i].getFlow ()) - minimalFlow); // Verringerung verbleibender Kapazität der Kanten, um deren aktuellen Füllstand
		}
		for (int i = 1; i < Waybackward.Count; i++){
				Waybackward [i].setFlow ((Waybackward [i].getCapacity () - Waybackward [i].getFlow ()) + minimalFlow);// Erhöhung verbleibender Kapazität der Kanten, um deren aktuellen rückläufigen Durchfluss
		}	

		if(walkThrough){ //Test ob kompletter Durchlauf angewählt wurde
			fordFulkerson (true);//rekursiver Neuaufruf des FFA
		}
		Debug.Log("terminiert ");
	}

	private void kill(){ //Beendungsmethode
			//Ausgabe auf Ausgabelabelfeld anpassen!!!!!!
			Debug.Log("MaxFlow: " + MaxFlow);	//Ausgabe des maximalen Flusses
			run = false;//Ausführvariabel auf Stopp setzen
	}
}