using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace Model
{
	public class Edge : IDataObject
	{
        // Edgeanfang und ende zum setzen der Linie
		public Vector3 vectorA;
		public Vector3 vectorB;

		private string startNode;
        private string endNode;
		private string edgeName;
		private int capacity;
		private int flow;
		private bool visited;
		private GameObject edgeObject;

		public Edge(GameObject edgeObject, string startNode, string endNode, string edgeName, int capacity, int flow, bool visited)
		{
			this.startNode = startNode;
			this.endNode = endNode;
			this.edgeName = edgeName;
			this.capacity = capacity;
			this.flow = flow;
			this.visited = visited;
			this.edgeObject = edgeObject;
		}
        //Anfangsknoten der Edge zur�ckgeben
        public string getStart()
		{
			return startNode;
		}
        //EndKnoten der Edge zur�ckgeben
        public string getEnd()
		{
			return endNode;
		}
        //Kapazit�t zur�ckgeben
        public int getCapacity()
		{
			return capacity;
		}
        //Flow zur�ckgeben
        public int getFlow()
		{
			return flow;
		}
        //Ob der Knoten im durchgang schonmal durchlaufen wurde zur�ckgeben
        public bool getVisited()
		{
			return visited;
		}
        //Namen der Edge zur�ckgeben
        public string getEdgeName()
		{
			return edgeName;
		}
        //Passende Gameobject zur�ckgeben
        public GameObject getObject()
        {
			return edgeObject;
		}
        //Flow setzen
		public void setFlow(int wert)
		{
			this.flow = wert;
		}
        //Kapazit�t setzen
		public void setCapacity(int wert)
		{
			this.capacity = wert;
		}
        //Edges setzen die im durchgang schonmal durchlaufen sind
		public void setVisited(bool visited)
		{
			this.visited = visited;
		}
        //Anfangs Node setzen
		public void setStart(string start)
		{
			this.startNode = start;
		}
        //End Node setzen
		public void setEnd(string end)
		{
			this.endNode = end;
		}
	}
}

