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
        public bool isSenke;

		public Node (Vector3 nodePosition, string nodeName, bool isSource, bool isSenke)
		{
            this.nodePosition = nodePosition;
            this.nodeName = nodeName;
            this.isSource = isSource;
            this.isSenke = isSenke;
		}
	}
}

