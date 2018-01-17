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
        public string edgeName;
		public double capacity;
		public double flow;
		public Boolean visited;


		public Edge (Vector3 vectorA, Vector3 vectorB, string edgeName, double capacity, double flow, Boolean visited)
		{
			this.vectorA = vectorA;
			this.vectorB = vectorB;
            this.edgeName = edgeName;
			this.capacity = capacity;
			this.flow = flow;
			this.visited = visited;
		}

//		public string GetName()
//		{
//			return a.Label + "_" + b.Label;
//		}
	}
}

