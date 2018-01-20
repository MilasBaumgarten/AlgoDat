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

		public string startNode;
        public string endNode;
        public string edgeName;
		public int capacity;
		public int flow;
		public Boolean visited;


		/*public Edge (Vector3 vectorA, Vector3 vectorB, string edgeName, double capacity, double flow, Boolean visited)
		{
			this.vectorA = vectorA;
			this.vectorB = vectorB;
            this.edgeName = edgeName;
			this.capacity = capacity;
			this.flow = flow;
			this.visited = visited;
		}*/

		public Edge(string startNode, string endNode, string edgeName, int capacity, int flow, Boolean visited)
        {
            this.startNode = startNode;
            this.endNode = endNode;
            this.edgeName = edgeName;
            this.capacity = capacity;
            this.flow = flow;
            this.visited = visited;
        }

//		public string GetName()
//		{
//			return a.Label + "_" + b.Label;
//		}

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
	}
}

