using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klasse zum Generieren der Default-Namen von Edges und Vertices
public class VertexEdgeNameGen : MonoBehaviour {

    //Buchstaben, welche den Anfang der generierten Namen bilden
    private string edgeChar = "Kante ";
    private string vertexChar = "Knoten ";

    //CController, um nodeNumber auszulesen
    private CController ccont;

    //Nummern, welche das Ende der generierten Namen bilden
    private int edgeNum;
    private int vertexNum;

    //Namen, welche am Ende der Methode zurückgegeben werden
    private string edgeName = "";
    private string vertexName = "";

    void Start()
    {
        ccont = GameObject.Find("GraphController").GetComponent<CController>();
        edgeNum = 1;
    }

    //Update-Methode nur zum Testen
    void Update()
    {
        if(vertexNum != ccont.nodeNumber)
        {
            vertexNum = ccont.nodeNumber;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GenerateEdgeName();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            GenerateVertexName();
        }
    }

    public string GenerateEdgeName()
    {
        //Zusammensetzen des Strings
        edgeName = edgeChar + edgeNum.ToString();
        //Zahlenwert inkrementieren
        edgeNum++;
        //String zurückgeben
        return edgeName;
    }

    public string GenerateVertexName()
    {
        vertexNum = ccont.nodeNumber;
        //Zusammensetzen des Strings
        vertexName = vertexChar + vertexNum.ToString();
        //String zurückgeben
        return vertexName;
    }
}
