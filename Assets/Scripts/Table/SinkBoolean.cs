using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class SinkBoolean : MonoBehaviour
{
    string parentName;
    public bool isSink;
    Transform thisToggle;

    private CController ccont;
    private List<Node> nodes;

    void Start()
    {
        thisToggle = gameObject.transform;
        ccont = GameObject.Find("GraphController").GetComponent<CController>();
    }

    public void TurnOffAllOtherToggles()
    {
        isSink = thisToggle.GetComponent<Toggle>().isOn;

        int vertexCount = GameObject.Find("GraphController").GetComponent<CController>().vertexCount;
        GameObject parent = GameObject.FindGameObjectWithTag("VertexContent");

        //Zugewiesener Knotenname, welcher in der Tabelle angezeigt wird
        parentName = thisToggle.parent.transform.Find("VertexName").GetComponent<Text>().text;

        //Wenn toggle angeschalten wird, werden andere ausgeschalten
        if (thisToggle.gameObject.transform.GetComponent<Toggle>().isOn)
        {
            if (vertexCount > 1)
            {
                //Ein Knoten darf nur Quelle ODER Senke sein
                thisToggle.parent.Find("SourceToggle").GetComponent<Toggle>().isOn = false;
            }
            else
            {
                return;
            }

            for (int i = 1; i <= vertexCount; i++)
            {
                Transform other = parent.transform.GetChild(i);
                string otherName = other.Find("VertexName").GetComponent<Text>().text;

                Debug.Log(otherName);

                if (otherName != parentName)
                    other.Find("SinkToggle").GetComponent<Toggle>().isOn = false;
            }
        }
    }

    public void SetSink()
    {
        int currentIndex = gameObject.transform.parent.GetSiblingIndex() - 1;
        Debug.Log("Sink Index: " + currentIndex);
        nodes = ccont.GetAllNodes();
        nodes[currentIndex].SetSink(thisToggle.GetComponent<Toggle>().isOn);
        ccont.SetAllNodes(nodes);
        nodes = ccont.GetAllNodes();

        Debug.Log("List of nodes:");
        foreach (Node n in nodes)
        {
            Debug.Log(n.nodeName + ": isSink: " + n.getSink());
        }
    }
}
