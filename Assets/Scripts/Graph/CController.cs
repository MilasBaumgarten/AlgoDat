﻿using System.Collections;
 
using System.Collections.Generic;
 
using UnityEngine;
 
using Model;
 

 
public class CController : MonoBehaviour {
  //Row-Manager zum erstellen der Tabellendaten
  SimpleRowManager rowManager;
 
  [Header("Kanten Variablen")]
  public GameObject edge; // Gameobjekt für Kante
 
  // Kapazität der Kante
 
  [SerializeField]
  private float maxWidth = 2;
  private float widthMultiplier = 1;
    private int edgeCounter = 0;
    public TextMesh edgeName = new TextMesh();
    public Transform edgePos;
    public GameObject[] edgeHolder;
 
    private int edgezahl = 0;
 
   [Header("Knoten Variablen")]
    public GameObject node; // Gameobjekt für Knoten
    public int nodeNumber = 1;
    public float timeWindow = 0.25f; // Zeitinterval für doppelklicks
    public double timeBuffer = 0; // Zeit seit letzem Klick
    public int vertexCount = 0; //Tatsächliche Anzahl der Knoten
    public int nodeCounter = 0;
    public TextMesh nodeName = new TextMesh();
    public Transform nodePos;

    Vector3 mousePos = new Vector3(0, 0, 0); // Raumvektor für Mausposition
 
    public Edge e;
    public Node n;
 
    public List<Node> nodes = new List<Node>();
    public List<Edge> edges = new List<Edge>();
    
    Vector3 v1 = new Vector3(0, 0, 0); // Platzhalter für Knotenposition 1
    Vector3 v2 = new Vector3(0, 0, 0); // Platzhalter für Knotenposition 2
 
  public void Start(){
    rowManager = GameObject.FindGameObjectWithTag("AddVertex").GetComponent<SimpleRowManager>();
  }
 

 
    void Update()
    {
        ClickTracker(); 
        if (Input.GetKeyDown("l")) {
                removeGraph();
        }
        edgezahl = 0;

        foreach (Edge e in edges){
            edgeHolder = GameObject.FindGameObjectsWithTag("Edge");
            edgeName = edgeHolder[edgezahl].GetComponent<TextMesh>();
            edgeName.text = e.getFlow() + " / " + e.getCapacity();
            edgezahl++;
            Debug.Log("Flow: " + e.getFlow());
        }
    }
 

 
  public void ClickTracker()
 
  {
 
    if (Input.GetMouseButtonDown(1)) // Wenn rechte Maustaste gedrückt dann...
 
    {
 
      CreateEdge();
 
    }
 

 
    if (Input.GetKeyDown("r"))
 
    {
 
      v1 = new Vector3(0, 0, 0); // Position zurücksetzen von Knotenposition 1
 
      v2 = new Vector3(0, 0, 0); // Position zurücksetzen von Knotenposition 2
 
      Debug.Log("Werte zurück gesetzt");
 
    }
 

 
    if (Input.GetKeyDown("s"))
 
    {
 
      for (int i = 0; i < 3; i++)
 
      {
 
        Debug.Log(edges[i].getStart());
 
        Debug.Log(edges[i].getEnd());
 
      }
 
    }
 

 
    if (Input.GetKeyDown("g"))
 
    {
 
      createStandardGraph ();
 
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
                RaycastHit hit; //Zum erfassen der Position
                if (Physics.Raycast(ray, out hit)) // Wenn der Strahl was trifft dann...
                {
                    if (hit.collider.tag == "Playfield")// Wenn der Tag übereinstimmt dann...
                    {
                        
                            Debug.Log("double click");
                            CreateNode();
                        

                    }
                }
            }
                         timeBuffer = Time.time; //Aktuallisieren der Zeit
 
    }
 
  }
 

 
  public void removeGraph()
 
  {
 
        vertexCount = 0; 
 
        nodeCounter = 0;
 

 
        edgeCounter = 0;
 
        nodes.Clear();
 
        edges.Clear();
 

 
    nodes = new List<Node>();
 
    edges = new List<Edge>();
 
  }
 
	public void removeNode(string name)
	{
		foreach (Node n in nodes) 
		{
			if(n.getName() == name)
			{
				nodes.Remove(n);
			}
		}
	}

	public void removeEdge(string name)
	{
		foreach (Edge e in edges) 
		{
			if(e.getEdgeName() == name)
			{
				edges.Remove(e);
			}
		}
	}


	public void createStandardGraph()
	{
		removeGraph ();
		Vector3 vs1 = new Vector3(-1, 0, 1);
		Vector3 vs2 = new Vector3(1, 2, 1);
		Vector3 vs3 = new Vector3(1, -2, 1);
		Vector3 vs4 = new Vector3(4, 2, 1);
		Vector3 vs5 = new Vector3(4, -2, 1);
		Vector3 vs6 = new Vector3(6, 0, 1);
		
		createStandardNode (vs1, true, false);
		createStandardNode (vs2, false, false);
		createStandardNode (vs3, false, false);
		createStandardNode (vs4, false, false);
		createStandardNode (vs5, false, false);
		createStandardNode (vs6, false, true);

		createStandardEdge (vs1, vs2, 10, 0);
		createStandardEdge (vs1, vs3, 10, 0);
		createStandardEdge (vs2, vs3, 2, 0);
		createStandardEdge (vs2, vs4, 4, 0);
		createStandardEdge (vs3, vs5, 9, 0);
		createStandardEdge (vs2, vs5, 8, 0);
		createStandardEdge (vs5, vs4, 6, 0);
		createStandardEdge (vs5, vs6, 10, 0);
		createStandardEdge (vs4, vs6, 10, 0);

        // CHEESE
        createStandardEdge (vs3, vs2, 2, 0);

        v1 = new Vector3(0, 0, 0); // Position zurücksetzen von Knotenposition 1
        v2 = new Vector3(0, 0, 0); // Position zurücksetzen von Knotenposition 2
    }

	public void createStandardNode(Vector3 position, bool source,bool sink)
	{
		Vector3 objectPos = position; // Übertragen auf Weltkamera
		objectPos.z = -1;    // setze Tiefe von Knoten auf -1, um immer vor allem zu sein
		Quaternion spawnRotation = Quaternion.identity; // Standard rotation 
		GameObject nodeObject = Instantiate(node, objectPos, spawnRotation); // Erstellen von Knoten(node aus Prefab, Mausposition, Standardrotation)

		nodeObject.name = nodeObject.name.Replace("(Clone)", ""); // Clone im Namen entfernen
		nodeObject.name = "Knoten " + nodeNumber; // Knoten einen einzigartigen Namen zuweisen 

		n = new Node(objectPos, "Knoten " + nodeNumber, source, sink); // Objekt mit Parametern erstellen

        //TextMesh Änderungen
        GameObject[] nodeHolder = GameObject.FindGameObjectsWithTag("TextNode");
        nodeName = nodeHolder[nodeCounter].GetComponent<TextMesh>();
        nodePos = nodeHolder[nodeCounter].GetComponent<RectTransform>();
        nodeCounter++;
        nodeName.text = "v" + nodeCounter;
        nodePos.position = position;


        nodes.Add(n); //Objekt in Liste hinzufügen
        //Tabelleneintrag erzeugen
        rowManager.InstantiateVertex();
        nodeNumber++;
        
    }

	public void createStandardEdge(Vector3 v1, Vector3 v2, int capacity, int flow)
	{
        this.v1 = v1;
        this.v2 = v2;
		// Spawne Kante
		GameObject edgeObject = Instantiate(edge, new Vector3(v1.x, v1.y, 0), new Quaternion());

		// setze Werte für Kanten
		constructEdge(edgeObject.GetComponent<LineRenderer>(), edgeObject.transform.GetChild(0).GetComponent<LineRenderer>(), 2, v1, v2);

        //HACK
        if (v1 == new Vector3(1, -2, 1) && v2 == new Vector3(1, 2, 1)){
            edgeObject.GetComponent<LineRenderer>().SetPositions(new Vector3[]{new Vector3(v1.x, v1.y, 2), new Vector3(v2.x, v2.y, 2)});
        }
        
        e = new Edge(edgeObject, GetNode(v1), GetNode(v2), edgeObject.name, capacity, flow, false);
        edgeObject.name = GetNode(v1) + " zu " + GetNode(v2);
        edges.Add(e);
        AddCEdges(e);

        // TextMesh Änderungen
        edgeHolder = GameObject.FindGameObjectsWithTag("Edge");
        edgeName = edgeHolder[edgeCounter].GetComponent<TextMesh>();
        edgePos = edgeHolder[edgeCounter].GetComponent<RectTransform>();
        edgeCounter++;
        edgeName.text = e.getFlow()+ " / " + e.getCapacity();
        edgePos.position =  Vector3.Lerp(v1, v2, 0.5f);
        Quaternion aimRotation = Quaternion.LookRotation(v2 - v1);
        aimRotation.x = 0;
        aimRotation.y = 0;
        edgePos.rotation = aimRotation;

        rowManager.InstantiateEdge();
       
    }

	public void CreateNode()
	{
		mousePos = Input.mousePosition; // Vektor aus Auslesen der Mausposition 
//!!!
        mousePos.z = 11; // z Koordinate von Mauspos setzen
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos); // Übertragen auf Weltkamera
		objectPos.z = -1;
        Quaternion spawnRotation = Quaternion.identity; // Standard rotation 
        GameObject nodeObject = Instantiate(node, objectPos, spawnRotation); // Erstellen von Knoten(node aus Prefab, Mausposition, Standardrotation)
        nodeObject.name = nodeObject.name.Replace("(Clone)", "");
        nodeObject.name = "Knoten " + nodeNumber;
        Node n = new Node(objectPos, "Knoten " + nodeNumber, false, false);
        nodes.Add(n);
        Debug.Log(nodes.IndexOf(n));


        //TextMesh Änderungen
        GameObject[] nodeHolder = GameObject.FindGameObjectsWithTag("TextNode");
        nodeName = nodeHolder[nodeCounter].GetComponent<TextMesh>();
        nodePos = nodeHolder[nodeCounter].GetComponent<RectTransform>();
        nodeCounter++;
        nodeName.text = "v" + nodeCounter;
        nodePos.position = objectPos;

        //Tabelleneintrag erstellen
        rowManager.InstantiateVertex();
        nodeNumber++;
        vertexCount++;
        
        Debug.Log("erstellt");

        
    }

	public void CreateEdge()
	{
		if (v1.Equals(new Vector3(0, 0, 0)) || v2.Equals(new Vector3(0, 0, 0))) // Wenn Knotenposition 1 oder Knotenposition 2 auf Ursprung stehen
		{
			Debug.Log("Bitte 2 Knoten auswählen");
		}
		else
		{
			GameObject edgeObject = Instantiate(edge, new Vector3(v1.x, v1.y, 0), new Quaternion()); // Erstellen der Kante (edge aus Prefab, von Mittelpunkt 
																								// zwischen den Knoten, Rotation in Richtung der Knoten)
			constructEdge(edgeObject.GetComponent<LineRenderer>(), edgeObject.transform.GetChild(0).GetComponent<LineRenderer>(), 2);
			edgeObject.name = edgeObject.name.Replace("(Clone)", "");
			edgeObject.name = GetNode(v1) + " zu " + GetNode(v2);

			e = new Edge(edgeObject, GetNode(v1), GetNode(v2), edgeObject.name, 0, 0, false);
			edges.Add(e);
			AddCEdges(e);

            // TextMesh Änderungen
            edgeHolder = GameObject.FindGameObjectsWithTag("Edge");
            edgeName = edgeHolder[edgeCounter].GetComponent<TextMesh>();
            edgePos = edgeHolder[edgeCounter].GetComponent<RectTransform>();
            edgeCounter++;
            edgeName.text = e.getFlow() + " / " + e.getCapacity();
            edgePos.position = Vector3.Lerp(v1, v2, 0.5f);
            Quaternion aimRotation = Quaternion.LookRotation(v2 - v1);
            aimRotation.x = 0;
            aimRotation.y = 0;
            edgePos.rotation = aimRotation;

            //Tabelleneintrag erstellen
            rowManager.InstantiateEdge();

            v1 = new Vector3(0, 0, 0); // Position zurücksetzen von Knotenposition 1
            v2 = new Vector3(0, 0, 0); // Position zurücksetzen von Knotenposition 2
            Debug.Log("reset");
            Debug.Log("erstellt");
        }
    }

    public void AddCEdges(Edge edge)
    {
		foreach (Node n in nodes) 
		{
			//if(n.getName() == edge.getStart() || n.getName() == edge.getEnd())
			if (n.getName().Equals(edge.getStart()) || n.getName().Equals(edge.getEnd()))
			{
				n.cEdges.Add (edge);
			}
		}
	}

	// Werte der Edge einstellen
	private void constructEdge(LineRenderer edge, LineRenderer animationEdge, int capacity){
		constructEdge(edge, animationEdge, capacity, this.v1, this.v2);
	}

	private void constructEdge(LineRenderer edge, LineRenderer animationEdge, int capacity, Vector3 v1, Vector3 v2){
		v1.z = 0;
		v2.z = 0;

		// setze Start und Endpunkt der Kante
		edge.SetPosition(0, v1);
		edge.SetPosition(1, v2);
		// setze Start und Endpunkt der Animationskante
		animationEdge.SetPosition(0, v1 + Vector3.forward);
		animationEdge.SetPosition(1, v2 + Vector3.forward);

		// setze Pfeil
		edge.transform.GetChild(1).transform.position = new Vector3(v1.x, v1.y, -0.5f);
		// rotiere Pfeil in Richtung des Endpunktes
		Vector2 arrowDirection = (v2 - edge.transform.GetChild(1).transform.position).normalized;
		float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * -Mathf.Rad2Deg + 90;
		edge.transform.GetChild(1).transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);

		// verschiebe Pfeile in Richtung des Endpunktes
		float offset = 0.162f;
		edge.transform.GetChild(1).transform.position += new Vector3(arrowDirection.x * offset, arrowDirection.y * offset, 0);
		edge.transform.GetChild(1).transform.Rotate(0, 180, 0);
		
		//maximale Kapazität einstellen
		edge.widthMultiplier = Mathf.Min(capacity * widthMultiplier, maxWidth);
		animationEdge.widthMultiplier = Mathf.Min(capacity * widthMultiplier, maxWidth);
	}

	//Getter für die Knoten eines Edges
    public string GetV1()
    {
        Debug.Log("V1: " + GetNode(v1));
        return GetNode(v1);
    }

    public string GetV2()
    {
        Debug.Log("V2: " + GetNode(v2));
        return GetNode(v2);
    }

    public List<Edge> GetAllEdges() // !!!
    { 
		return edges;
    }

    public void SetAllEdges(List<Edge> edges)
    {
        this.edges = edges;
    }

    public List<Node> GetAllNodes()
    {
        return nodes;
    }

    public void SetAllNodes(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public SimpleRowManager GetRowManager()
    {
        return rowManager;
    }

	//Getter

	public Vector3 GetNodePosition(int i)
	{
		return nodes[i].nodePosition;
	}

	public string GetNode(Vector3 nodePosition)
	{
		foreach (Node n in nodes)
		{
			if (n.nodePosition.x == nodePosition.x && n.nodePosition.y == nodePosition.y){
				return n.nodeName;
			} 
		}
		return null;
	}

	public Node GetNodeAsObject(string NodeName){
		foreach (Node n in nodes)
		{
			if (n.nodeName == NodeName)
			{
				return n;
			}
			
			
		}
		return null;
	}

	public List<Edge> getAllEdges() // !!!
	{ 
		return edges;
	}

	public Node getSource(){
		foreach(Node n in nodes){
			if(n.getSource()){
				return n;
			}
			
		}
		return null;
	}

		public Node getSink(){
		foreach(Node n in nodes){
			if(n.getSink()){
				return n;
			}
			
		}
		return null;
	}
}