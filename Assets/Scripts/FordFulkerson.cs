/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FordFulkerson : MonoBehaviour {

	private static bool run;
	private static int leftCapacity;
	private static int minimalFlow;
	private static Edge[] Way;
	private static int MaxFlow;

	private void vorarbeit(){
		//ist Quelle und Senke voranden
		if (List.getSource () != null && List.getSink () != null && List.getSource() != List.getSink()) {
			run = true;
			minimalFlow = 0;
			leftCapacity = 0;
			MaxFlow = 0;
			Array.Clear (Way);
			//Flow auf 0 setzen
			//wird standartmaessig mit 0 belegt
			for (int i = 0; i < getAllEdges().Length; i++) {
				allEdges [i].setFlow () = 0;
			}
			
		} else {
			//Fehlermeldung
			kill();
		}
			
	}

	public void fordFulkerson(bool walkThrough){
		vorarbeit ();
		if (run) {
			Node actualN = List.getSource ();
			Edge[] connectedEdges = actualN.getConnectedEdges ();

			 

			for (int i = 0; i < connectedEdges.Lenght; i++) {
				if (connectedEdges [i].getFlow < connectedEdges [i].getCapacityMax) {
					//Blink gewählte edge
					Edge actualEdge = connectedEdges [i];
					minimalFlow = actualEdge.getCapacityMax ();
					Way.add (actualEdge);
					actualN = actualEdge.getEnd ();
					if (actualN == List.getSink ()) {
						terminate (walkThrough);
					}
					break;
				} else {
					kill ();
					break;
				}
			}

			while (actualN != List.getSink () && actualN != List.getSource ()) {

				connectedEdges = actualN.getConnectedEdges ();

				for (int i = 0; i < connectedEdges.Lenght; i++) {
					if (connectedEdges [i].getFlow < connectedEdges [i].getCapacityMax) {
						//Blink gewählte edge
						Edge actualEdge = connectedEdges [i];
						leftCapacity = actualEdge.getCapacityMax() - actualEdge.getFlow ();
						if (minimalFlow > leftCapacity) {
							minimalFlow = leftCapacity;
						}
						Way.add (actualEdge);
						actualN = actualEdge.getEnd ();
						if (actualN == List.getSink ()) {
							terminate (walkThrough);
						}
						break;
					}
					else {
						kill ();
						break;
					}
				}
			}



		
		}
	}

	private void terminate(bool walkThrough){
		MaxFlow += minimalFlow;
		for (int i = 0; i < Way.Length; i++) {
			if (Way [i].getIsForward ()) {
				Way [i].setFlow ((Way [i].getCapacityMax () - Way [i].getFlow ()) - minimalFlow);
			} else {
				Way [i].setFlow ((Way [i].getCapacityMax () - Way [i].getFlow ()) + minimalFlow);
			}
		}

		if(walkThrough){
			fordFulkerson ();
		}
	}

	private void kill(){
			//Ausgabe auf Ausgabelabelfeld anpassen!!!!!!
			Console.WriteLine(MaxFlow);			
	}


}

*/