/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FordFulkerson : MonoBehaviour { //Erstellt von Tim und Sebastian

	private static bool run; //gibt Auskunft darüber, ob Algorithmus ausgeführt werden kann
	private static int leftCapacity; //gibt Auskunft über die auf der Edge verbleibende Kapazität
	private static int minimalFlow; //gibt Auskunft über die im aktuellen Druchlauf kleinste benutzte Kapazität, die danach addiert/subtrahiert wird
	//Anstatt eines Arrays eine Liste verwenden da Array.add() wahrscheinlich nicht funktioniert
	private static Edge[] Wayforward; //gibt Auskunft über die beim aktuellen Durchgang abgelaufenen Edges in Vorwärtige Richtung
	private static Edge[] Waybackward; //gibt Auskunft über die beim aktuellen Durchgang abgelaufenen Edges in Rückwärtige Richtung
	private static int MaxFlow; //gibt Auskunft über den maximalen Fluss des Graphen

	private void vorarbeit(){ // Methode, welche grundlegende Schritte ausführt, um FFA (Ford Fulkerson Algorithmus) funktionsfähig zu machen
		if (List.getSource () != null && List.getSink () != null && List.getSource() != List.getSink()) {   //prüfen, ob Quelle und Senke voranden und nicht gleich sind

			//Variablen der Klasse zurücksetzen, um eventuell zwischengespeicherte Werte bei Neustart zu löschen
			run = true;
			minimalFlow = 0;
			leftCapacity = 0;
			MaxFlow = 0;
			Array.Clear (Waybackward);
			Array.Clear	(Wayforward);
			

			for (int i = 0; i < getAllEdges().Length; i++) {  //Flow aller Edges auf 0 setzen; wird standartmaessig mit 0 belegt
				allEdges [i].setFlow () = 0;
			}
		} 
		else {
			//Fehlermeldung im Dialogfenster ausgeben, dass Quelle und Senke nicht gewählt sind -> noch implementieren !!!
			kill();// Algorithmus beenden und maximalen Fluss ausgeben
		}
	}

	public void fordFulkerson(bool walkThrough){ // eigentlicher Algorithmus, Walkthrough Variable gibt Auskunft darüber, welche Darstellungsart Nutzer angecklickt hat -> Schrittweise oder am Stück
		vorarbeit (); // oben vorbereitet und erläuterte Vorarbeit einmalig ausführen vor Start 
		if (run) { // Test, ob Algorithmus noch ausgeführt werden soll bzw. darf
					Node actualN = List.getSource (); // aktuell betrachteter Knoten ist die gewählte Quelle
			Edge[] connectedEdges = actualN.getConnectedEdges (); // mit Quellknoten verbundene Kanten werden ermittelt und zwischengespeichert
			

			for (int i = 0; i < connectedEdges.Lenght; i++) { //iterieren über alle am Knoten anliegenden Kanten

				if (connectedEdges[i].getisVisited ==false && connectedEdges[i].getEnd() != actualN && connectedEdges [i].getFlow < connectedEdges [i].getCapacityMax) { // Check ob aktuell betrachtete Kante noch freie Kapazitäten für den Fluss hat, erste passende Kante wird gewählt
					//Blink gewählter passender Edge hier noch implementieren !!! Eigene Methode ???
					
					Edge actualEdge = connectedEdges [i]; // Auswahl erster passender zu betrachtender Edge
					minimalFlow = actualEdge.getCapacityMax (); // kleinster Fluss wird als Referenzwert mit der maximalen Kapazität der Quelle initialisiert, CapacityMax = Dicke des Lasers, statische Zahl aus tabelle wird dargestellt
						if(actualN == actualEdge.getStart){
							Wayforward.add (actualEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
							actualEdge.setisVisited(true);					
						}
						actualN = actualEdge.getEnd ();// zu betrachtender Knoten wird gewechselt auf den Knoten, der am Ende der Edge befindlich ist  (Frage bei Rückläufigen Edges???)
					
					if (actualN == List.getSink ()) { // Test ob neuer zu betrachtender Knoten die Senke ist
						terminate (walkThrough); // Wenn ja, terminiere den Algorithmus und starte neuen Durchlauf, da kompletter Weg von Quelle zu Senke gefunden wurde
					}

					break; // Abbruch übergeordneter for Schleife
				}
				else if(i == connectedEdges.Lenght){
					kill ();// Algorithmus beenden und maximalen Fluss ausgeben
					break;// Abbruch übergeordneter for Schleife
				}
			}

			while (actualN != List.getSink () && actualN != List.getSource ()) { // Betrachten restlicher im  Grafen verbliebener Knoten, solange diese nicht Senke oder Quelle sind

				Edge[] connectedEdges = actualN.getConnectedEdges (); // mit Quellknoten verbundene Kanten werden ermittelt und zwischengespeichert, bereits besuchte Kanten von Betrachtung ausschliessen??? -> Edge[] connectedEdges = actualN.getConnectedEdges ()- Way[]; ???

				for (int i = 0; i < connectedEdges.Lenght; i++) {//iterieren über alle am Knoten anliegenden Kanten
					if (connectedEdges[i].getisVisited ==false && connectedEdges [i].getFlow < connectedEdges [i].getCapacityMax()) {// Check ob aktuell betrachtete Kante noch freie Kapazitäten für den Fluss hat, erste passende Kante wird gewählt
						//Blink gewählter passender Edge hier noch implementieren !!! Eigene Methode ???
						
						Edge actualEdge = connectedEdges [i];// Auswahl erster passender zu betrachtender Edge
						leftCapacity = actualEdge.getCapacityMax() - actualEdge.getFlow (); // auf Edge verbleibende Kapazität wird aus der Subtraktion des aktuellen Durchflusses von der maximalen Kapazitzät errechnet

						if (minimalFlow > leftCapacity) { //Test ob dynamischer Wert des kleinsten im Durchlauf verwendeten Flusses erneuert werden muss
							minimalFlow = leftCapacity; //Wenn ja, wird dieser durch den aktuell verbleibenden Fluss auf der Edge ersetzt
						}
						if(actualN == actualEdge.getStart()){
							Wayforward.add (actualEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
							actualEdge.setisVisited(true);
						}else if(actualN == actualEdge.getEnd() &&actualEdge.getFlow() != 0) {
							Waybackward.add (actualEdge);// ausgewählte Kante wird dem Wegearray hinzugefügt und sich gemerkt, um später den Weg von Queelle zu Senke rekonstruieren zu können
							actualEdge.setisVisited(true);
						}
						
						actualN = actualEdge.getEnd ();// zu betrachtender Knoten wird gewechselt auf den Knoten, der am Ende der Edge befindlich ist  (Frage bei Rückläufigen Edges???)

						if (actualN == List.getSink ()) {// Test ob neuer zu betrachtender Knoten die Senke ist
							terminate (walkThrough);// Wenn ja, terminiere den Algorithmus und starte neuen Durchlauf, da kompletter Weg von Quelle zu Senke gefunden wurde
						}
						break;// Abbruch übergeordneter for Schleife
					}
					else if(i == connectedEdges.Lenght){
						kill ();// Algorithmus beenden und maximalen Fluss ausgeben
						break;// Abbruch übergeordneter for Schleife
					}
				}
			}
		}
	}

	private void terminate(bool walkThrough){ //Terminieren des Algorithmus nach einem Erfolgreichen Durchlauf (Schritt), Walkthrough gibt abspielart an
		MaxFlow += minimalFlow; // Inkrimentierung des Maximalen Flusses, um den, im aktuellen Durchgang, kleinsten benutzten Fluss
		
		for (int i = 0; i < Wayforward.Length; i++) {//Iteration über alle im Weg zwischen Quelle und Senke befindlichen Kanten des aktuellen Durchlaufes
				Wayforward [i].setFlow ((Wayforward [i].getCapacityMax () - Wayforward [i].getFlow ()) - minimalFlow); // Verringerung verbleibender Kapazität der Kanten, um deren aktuellen Füllstand
		}
		for (int i = 0; i < Waybackward.Length; i++){
				Waybackward [i].setFlow ((Waybackward [i].getCapacityMax () - Waybackward [i].getFlow ()) + minimalFlow);// Erhöhung verbleibender Kapazität der Kanten, um deren aktuellen rückläufigen Durchfluss
		}	

		if(walkThrough){ //Test ob kompletter Durchlauf angewählt wurde
			fordFulkerson ();//rekursiver Neuaufruf des FFA
		}
	}

	private void kill(){ //Beendungsmethode
			//Ausgabe auf Ausgabelabelfeld anpassen!!!!!!
			Console.WriteLine(MaxFlow);	//Ausgabe des maximalen Flusses
			run = false;//Ausführvariabel auf Stopp setzen
	}
}

*/
