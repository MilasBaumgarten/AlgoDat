using System.Collections;
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
				//Debug.Log(nodes[i].getconnectedEdges());
				/*Debug.Log(GetCapacity(i));
				Debug.Log(GetFlow(i));
				Debug.Log(GetSource(i));
				Debug.Log(GetSource(i));
				Debug.Log(GetVisited(i));*/
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

				if (!Physics.Raycast(ray)) // Wenn der Strahl was nichts trifft
				{
					Debug.Log("double click");
					CreateNode();
				}
			}
			timeBuffer = Time.time; //Aktuallisieren der Zeit
		}
	}

	public void removeGraph()
	{
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
		Vector3 v1 = new Vector3(-1, 0, 1);
		Vector3 v2 = new Vector3(1, 2, 1);
		Vector3 v3 = new Vector3(1, -2, 1);
		Vector3 v4 = new Vector3(4, 2, 1);
		Vector3 v5 = new Vector3(4, -2, 1);
		Vector3 v6 = new Vector3(6, 0, 1);
		
		createStandardNode (v1, true, false);
		createStandardNode (v2, false, false);
		createStandardNode (v3, false, false);
		createStandardNode (v4, false, false);
		createStandardNode (v5, false, false);
		createStandardNode (v6, false, true);

		createStandardEdge (v1, v2, 10, 0);
		createStandardEdge (v1, v3, 10, 0);
		createStandardEdge (v2, v3, 2, 0);
		createStandardEdge (v2, v4, 4, 0);
		createStandardEdge (v3, v5, 9, 0);
		createStandardEdge (v2, v5, 8, 0);
		createStandardEdge (v5, v4, 6, 0);
		createStandardEdge (v5, v6, 10, 0);
		createStandardEdge (v4, v6, 10, 0);


	}

	public void createStandardNode(Vector3 position, bool source,bool sink)
	{
		Vector3 objectPos = position; // Übertragen auf Weltkamera
		objectPos.z = -1;    // setze Tiefe von Knoten auf -1, um immer vor allem zu sein
		Quaternion spawnRotation = Quaternion.identity; // Standard rotation 
		GameObject nodeObject = Instantiate(node, objectPos, spawnRotation); // Erstellen von Knoten(node aus Prefab, Mausposition, Standardrotation)

		nodeObject.name = nodeObject.name.Replace("(Clone)", ""); // Clone im Namen entfernen
		nodeObject.name = "v" + nodeNumber; // Knoten einen einzigartigen Namen zuweisen 

		n = new Node(objectPos, "v" + nodeNumber, source, sink); // Objekt mit Parametern erstellen
		nodeNumber++;
		nodes.Add(n); //Objekt in Liste hinzufügen
	}

	public void createStandardEdge(Vector3 v1, Vector3 v2, int capacity, int flow)
	{
		// Spawne Kante
		GameObject edgeObject = Instantiate(edge, new Vector3(v1.x, v1.y, 0), new Quaternion()); 

		// setze Werte für Kanten
		constructEdge(edgeObject.GetComponent<LineRenderer>(), edgeObject.transform.GetChild(0).GetComponent<LineRenderer>(), 2, v1, v2);
		edgeObject.name = edgeObject.name.Replace("(Clone)", "");
		edgeObject.name = GetNode(v1) + " zu " + GetNode(v2);

		e = new Edge(edgeObject, GetNode(v1), GetNode(v2), edgeObject.name, capacity, flow, false);
		edges.Add(e);
		AddCEdges(e);
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

			e = new Edge(edgeObject, GetNode(v1), GetNode(v2), edgeObject.name, 1, 0, false);
			edges.Add(e);
			AddCEdges(e);
		

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
		// setze Start und Endpunkt der Kante
		edge.SetPosition(0, v1);
		edge.SetPosition(1, v2);
		// setze Start und Endpunkt der Animationskante
		animationEdge.SetPosition(0, v1 + Vector3.forward);
		animationEdge.SetPosition(1, v2 + Vector3.forward);

		// setze Pfeil
		edge.transform.GetChild(1).transform.position = new Vector3(v1.x, v1.y, 0);
		// rotiere Pfeil in Richtung des Endpunktes
		Vector2 arrowDirection = (v2 - edge.transform.GetChild(1).transform.position).normalized;
		float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * -Mathf.Rad2Deg + 90;
		edge.transform.GetChild(1).transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);

		// verschiebe Pfeile in Richtung des Endpunktes
		float offset = 0.162f;
		edge.transform.GetChild(1).transform.position += new Vector3(arrowDirection.x * offset, arrowDirection.y * offset, 0);

		
		//maximale Kapazität einstellen
		edge.widthMultiplier = Mathf.Min(capacity * widthMultiplier, maxWidth);
		animationEdge.widthMultiplier = Mathf.Min(capacity * widthMultiplier, maxWidth);
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
			if(n.getisSource()){
				return n;
			}
			
		}
		return null;
	}

		public Node getSink(){
		foreach(Node n in nodes){
			if(n.getisSink()){
				return n;
			}
			
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
}