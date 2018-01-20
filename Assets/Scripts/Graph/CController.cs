﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class CController : MonoBehaviour {

    //Row-Manager zum erstellen der Tabellendaten
    SimpleRowManager rowManager;


	//Knoten reseten nachdem 2 ausgewählt wurden ohne Edge zu erstellen programmieren


    [Header("Kanten Variablen")]
    public GameObject edge; // Gameobjekt für Kante
    // Kapazität der Kante
    [SerializeField]
    private float maxWidth = 2;
    private float widthMultiplier = 1;

    [Header("Knoten Variablen")]
    public GameObject node; // Gameobjekt für Knoten
    public int nodeNumber = 1;
    public float timeWindow = 0.25f; // Zeitinterval für doppelklicks
    public double timeBuffer = 0; // Zeit seit letzem Klick
    Vector3 mousePos = new Vector3(0, 0, 0); // Raumvektor für Mausposition
    public int vertexCount = 0; //Tatsächliche Anzahl der Knoten

    public List<Node> nodes = new List<Node>();
	public List<Edge> edges = new List<Edge>();
    Vector3 v1 = new Vector3(0, 0, 0); // Platzhalter für Knotenposition 1
    Vector3 v2 = new Vector3(0, 0, 0); // Platzhalter für Knotenposition 2

	public  void Start(){
        rowManager = GameObject.FindGameObjectWithTag("AddVertex").GetComponent<SimpleRowManager>();
	}

    void Update()
    {
        ClickTracker(); 
		if (Input.GetMouseButtonDown (2)) {
           //Debug.Log(ConnectedEdges(v1));
		}
    }

    public void ClickTracker()
    {
        if (Input.GetMouseButtonDown(1)) // Wenn rechte Maustaste gedrückt dann...
        {
            CreateEdge();
        }

        if (Input.GetMouseButtonDown(0)) // Wenn linke Maustaste gedrückt dann...
        {
            if ((Time.time - timeBuffer) > timeWindow)// Wenn Zeit seit letztem Klick größer als Zeitinterval für Doppelklicks ist dann...
            {
                Debug.Log("single click");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Erzeugen von einem Strahl(Ray) an der Mausposition
                RaycastHit hit; //Zum erfassen der Position 

                if (Physics.Raycast(ray, out hit)) // Wenn der Strahl was trifft dann...
                {
                    if (hit.collider.tag == "Node")// Wenn der Tag übereinstimmt dann...
                    {
                        Debug.Log("This is a Node");
                        if (v1.Equals(new Vector3(0, 0, 0)))// Wenn Knotenposition 1 noch nicht gesetzt ist dann...
                        {
                            v1 = hit.collider.transform.position; // Knotenposition 1 wird auf Ortsvektor(transform) des colliders gesetzt
                            Debug.Log("v1 set");
                        }
                        else
                        {
                            if ((v2.Equals(new Vector3(0, 0, 0))))// Wenn Knotenposition 2 noch nicht gesetzt ist dann...
                            {
                                v2 = hit.collider.transform.position; // Knotenposition 2 wird auf Ortsvektor(transform) des colliders gesetzt
                                if (v1.Equals(v2)) // Wenn Knotenposition 1 und Knotenposition 2 gleich sind dann... 
                                {
                                    Debug.Log("nicht 2mal den selben pls");
                                    v2 = new Vector3(0, 0, 0); // Zurücksetzen von Knotenposition 2 auf Ursprung zum reset
                                }
                                else
                                {
                                    Debug.Log("v2 set");
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("This isn't a Node");
                    }
                }
            }
            else
            {
                // Erzeugen von einem Strahl(Ray) an der Mausposition
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (!Physics.Raycast(ray)) // Wenn der Strahl was nichts trifft
                {
                    Debug.Log("double click");
                    CreateNode();
                }
            }
            timeBuffer = Time.time; //Aktuallisieren der Zeit
        }
    }

    public void CreateNode()
    {
        mousePos = Input.mousePosition; // Vektor aus Auslesen der Mausposition 
//!!!
        mousePos.z = 11; // z Koordinate von Mauspos setzen
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos); // Übertragen auf Weltkamera 
        Debug.Log(mousePos);
        Quaternion spawnRotation = Quaternion.identity; // Standard rotation 
        GameObject nodeObject = Instantiate(node, objectPos, spawnRotation); // Erstellen von Knoten(node aus Prefab, Mausposition, Standardrotation)
        nodeObject.name = nodeObject.name.Replace("(Clone)", "");
        nodeObject.name = "Knoten " + nodeNumber;
        Node n = new Node(objectPos, "Knoten " + nodeNumber, false, false);
        nodeNumber++;
        vertexCount++;
        nodes.Add(n);
        Debug.Log("erstellt");
        rowManager.InstantiateObject();
    }

    public void CreateEdge()
    {
        Vector3 pos = Vector3.Lerp(v1, v2, 0.5f); // Punkt auf der Mitte von Knotenposition 1 und Knotenposition 2
        Vector3 aim = v2 - v1;

        if (v1.Equals(new Vector3(0, 0, 0)) || v2.Equals(new Vector3(0, 0, 0))) // Wenn Knotenposition 1 oder Knotenposition 2 auf Ursprung stehen
        {
            Debug.Log("Bitte 2 Knoten auswählen");
        }
        else
        {
            GameObject edgeObject = Instantiate(edge, new Vector3(v1.x, v1.y, 0), new Quaternion()); // Erstellen der Kante (edge aus Prefab, von Mittelpunkt 
                                                                                                  // zwischen den Knoten, Rotation in Richtung der Knoten)
            // setze Werte für Kanten
            // TODO: übergebe maximale Kapazität (3. Parameter)
            constructEdge(edgeObject.GetComponent<LineRenderer>(), edgeObject.transform.GetChild(0).GetComponent<LineRenderer>(), 2);

            edgeObject.name = edgeObject.name.Replace("(Clone)", "");
            edgeObject.name = GetNode1(v1) + " und " + GetNode2(v2);
            Edge e = new Edge(v1, v2, edgeObject.name, 1, 1, false);
			edges.Add (e);
           

            v1 = new Vector3(0, 0, 0); // Position zurücksetzen von Knotenposition 1
            v2 = new Vector3(0, 0, 0); // Position zurücksetzen von Knotenposition 2
            Debug.Log("reset");
            Debug.Log("erstellt");
        }
    }


    private string GetNode1(Vector3 nodePosition1)
    {
        foreach (Node n in nodes)
        {
            if (n.nodePosition == nodePosition1)
                return n.nodeName;
        }
        return null;
    }

    private string GetNode2(Vector3 nodePosition2)
    {
        foreach (Node n in nodes)
        {
            if (n.nodePosition == nodePosition2)
                return n.nodeName;
        }
        return null;
    }

    /*private string ConnectedEdges(Vector3 nodePosition)
    {
        for (int es = 0; es <)
        {
            foreach (Edge e in edges)
            {
                if (e.vectorA == nodePosition)
                    return e.edgeName;
            }
        }
        return null;
    }*/

    // Werte der Edge einstellen
    private void constructEdge(LineRenderer edge, LineRenderer animationEdge, int capacity){
        // setze Start und Endpunkt der Kante
        edge.SetPosition(0, v1);
        edge.SetPosition(1, v2);
        // setze Start und Endpunkt der Animationskante
        animationEdge.SetPosition(0, v1 + Vector3.forward);
        animationEdge.SetPosition(1, v2 + Vector3.forward);
        
        //maximale Kapazität einstellen
        edge.widthMultiplier = Mathf.Min(capacity * widthMultiplier, maxWidth);
        animationEdge.widthMultiplier = Mathf.Min(capacity * widthMultiplier, maxWidth);
    }

}