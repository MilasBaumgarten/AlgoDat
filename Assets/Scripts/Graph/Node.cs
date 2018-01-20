using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace Model
{
	public class Node : IDataObject
	{
        public Vector3 nodePosition;
        public string nodeName;
        public bool isSource;
        public bool isSink;
        public List<Edge> cEdges = new List<Edge>();

		public Node (Vector3 nodePosition, string nodeName, bool isSource, bool isSink)
		{
            this.nodePosition = nodePosition;
            this.nodeName = nodeName;
            this.isSource = isSource;
            this.isSink = isSink;
		}

            public bool getSource()
		{
			return isSource;
		}

		public bool getSink()
		{
			return isSink;
		}
		public string getName()
		{
			return nodeName;
		}

		public List<Edge> getconnectedEdges(){
			return cEdges;
		}
	}
}

