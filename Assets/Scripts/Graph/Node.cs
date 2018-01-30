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
        // Quelle zur�ckgeben
        public bool getSource()
		{
			return isSource;
		}
        // Senke zur�ckgeben
        public bool getSink()
		{
			return isSink;
		}
        // Node Namen zur�ckgeben
		public string getName()
		{
			return nodeName;
		}

		public List<Edge> getConnectedEdges()
        {
			return cEdges;
		}
        // Quelle setzen
        public void setSource(bool isSource)
        {
            this.isSource = isSource;
        }
        // Senke setzen
        public void setSink(bool isSink)
        {
            this.isSink = isSink;
        }
    }
}

