using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class SourceBoolean : MonoBehaviour {

    string parentName;
    public bool isSource;

    private CController ccont;
    private List<Node> nodes;
    Transform thisToggle;

    void Start()
    {
        ccont = GameObject.Find("GraphController").GetComponent<CController>();
        thisToggle = gameObject.transform;
    }

	public void TurnOffAllOtherToggles()
    {
        isSource = thisToggle.GetComponent<Toggle>().isOn;

        int vertexCount = GameObject.Find("GraphController").GetComponent<CController>().vertexCount;
        GameObject parent = GameObject.FindGameObjectWithTag("VertexContent");

        //Zugewiesener Knotenname, welcher in der Tabelle angezeigt wird
        parentName = thisToggle.parent.transform.Find("VertexName").GetComponent<Text>().text;

        //Wenn toggle angeschalten wird, werden andere ausgeschalten
        if (thisToggle.GetComponent<Toggle>().isOn)
        {
            if(vertexCount > 1)
            {
                //Ein Knoten darf nur Quelle ODER Senke sein
                thisToggle.parent.Find("SinkToggle").GetComponent<Toggle>().isOn = false;
            } else
            {
                return;
            }

            for (int i = 1; i <= vertexCount; i++)
            {
                Transform other = parent.transform.GetChild(i);
                string otherName = other.Find("VertexName").GetComponent<Text>().text;

                Debug.Log(otherName);

                if(otherName != parentName)
                    other.Find("SourceToggle").GetComponent<Toggle>().isOn = false;
            }
        }
    }

    public void SetSource()
    {
        int currentIndex = gameObject.transform.parent.GetSiblingIndex() - 1;
        Debug.Log("Source Index: " + currentIndex);
        nodes = ccont.GetAllNodes();
        nodes[currentIndex].setSource(thisToggle.GetComponent<Toggle>().isOn);
        ccont.SetAllNodes(nodes);
        nodes = ccont.GetAllNodes();

        Debug.Log("List of nodes:");
        foreach(Node n in nodes)
        {
            Debug.Log(n.nodeName + ": isSource: " + n.getSource());
        }
    }
}
