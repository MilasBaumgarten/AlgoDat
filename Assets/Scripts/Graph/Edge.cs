using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace Model
{
	public class Edge : IDataObject
	{
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

		public Edge(){
			//null
		}

		public string getStart()
		{
			return startNode;
		}

		public string getEnd()
		{
			return endNode;
		}
		public int getCapacity()
		{
			return capacity;
		}

		public int getFlow()
		{
			return flow;
		}

		public bool getVisited()
		{
			return visited;
		}
		public string getEdgeName()
		{
			return edgeName;
		}

		public GameObject getObject(){
			return edgeObject;
		}


		public void setFlow(int wert)
		{
			this.flow = wert;
		}

		public void setCapacity(int wert)
		{
			this.capacity = wert;
		}

		public void setVisited(bool visited)
		{
			this.visited = visited;
		}

		public void setStart(string start)
		{
			this.startNode = start;
		}

		public void setEnd(string end)
		{
			this.endNode = end;
		}
	}
}

