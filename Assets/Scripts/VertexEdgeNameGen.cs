using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klasse zum Generieren der Default-Namen von Edges und Vertices
public class VertexEdgeNameGen : MonoBehaviour {

    //Buchstaben, welche den Anfang der generierten Namen bilden
    private string edgeChar = "E";
    private string vertexChar = "V";

    //Nummern, welche das Ende der generierten Namen bilden
    private int edgeNum = 1;
    private int vertexNum = 1;

    //Namen, welche am Ende der Methode zurückgegeben werden
    private string edgeName = "";
    private string vertexName = "";

    //Update-Methode nur zum Testen
    void Update()
    {
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
        //Testausgabe in der Konsole
        Debug.Log("Edge: " + edgeName);
        //String zurückgeben
        return edgeName;
    }

    public string GenerateVertexName()
    {
        //Zusammensetzen des Strings
        vertexName = vertexChar + vertexNum.ToString();
        //Zahlenwert inkrementieren
        vertexNum++;
        //Testausgabe in der Konsole
        Debug.Log("Vertex: " + vertexName);
        //String zurückgeben
        return vertexName;
    }
}
