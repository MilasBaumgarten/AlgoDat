using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;


public class CController : MonoBehaviour
{
    //Row-Manager zum erstellen der Tabellendaten
    SimpleRowManager rowManager;

    [Header("Kanten Variablen")]
    // Gameobjekt für Kante
    public GameObject edge;

    [SerializeField]
    private float maxWidth = 2;
    private float widthMultiplier = 1;
    private int edgeCounter = 0;
    // Textmesh Variablen
    public TextMesh edgeName = new TextMesh();
    public Transform edgePos;
    public GameObject[] edgeHolder;
    private int edgezahl = 0;

    [Header("Knoten Variablen")]
    // Gameobjekt für Knoten
    public GameObject node;
    public int nodeNumber = 1;
    // Zeitinterval für doppelklicks
    public float timeWindow = 0.25f;
    // Zeit seit letzem Klick
    public double timeBuffer = 0;
    // Tatsächliche Anzahl der Knoten
    public int vertexCount = 0;
    public int nodeCounter = 0;
    // Textmesh Variablen
    public TextMesh nodeName = new TextMesh();
    public Transform nodePos;

    // Raumvektor für Mausposition
    Vector3 mousePos = new Vector3(0, 0, 0);
    // Klassen Objekte
    public Edge e;
    public Node n;
    // Listen für Nodes und Edges
    public List<Node> nodes = new List<Node>();
    public List<Edge> edges = new List<Edge>();
    // Platzhalter für Knotenposition 1 und 2
    Vector3 v1 = new Vector3(0, 0, 0);
    Vector3 v2 = new Vector3(0, 0, 0);

	// Materialien
	public Material sourceMaterial;
	public Material sinkMaterial;
	public Material nodeMaterial;

    public void Start()
    {
        rowManager = GameObject.FindGameObjectWithTag("AddVertex").GetComponent<SimpleRowManager>();
    }

    void Update()
    {
        ClickTracker();
    }



    public void ClickTracker()
    {
        // Wenn rechte Maustaste gedrückt dann...
        if (Input.GetMouseButtonDown(1))
        {
            createStandardEdge(v1, v2, 1, 0);
            // Position zurücksetzen von Knotenposition 1
            v1 = new Vector3(0, 0, 0);
            // Position zurücksetzen von Knotenposition 2
            v2 = new Vector3(0, 0, 0);
        }
        //Wenn r gedrückt dann setze v1 und v2 zurück
        if (Input.GetKeyDown("r"))
        {
            // Position zurücksetzen von Knotenposition 1 und 2
            v1 = new Vector3(0, 0, 0);
            v2 = new Vector3(0, 0, 0);
            Debug.Log("Werte zurück gesetzt");
        }
        //Wenn g gedrück erstelle Standard Graphen
        if (Input.GetKeyDown("g"))
        {
            createStandardGraph();
        }
        // Wenn linke Maustaste gedrückt dann...
        if (Input.GetMouseButtonDown(0))
        {
            // Wenn Zeit seit letztem Klick größer als Zeitinterval für Doppelklicks ist dann...
            if ((Time.time - timeBuffer) > timeWindow)
            {
                Debug.Log("single click");
                // Erzeugen von einem Strahl(Ray) an der Mausposition
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Zum erfassen der Position 
                RaycastHit hit;
                // Wenn der Strahl was trifft dann...
                if (Physics.Raycast(ray, out hit))
                {
                    // Wenn der Tag übereinstimmt dann...
                    if (hit.collider.tag == "Node")
                    {
                        Debug.Log("This is a Node");
                        // Wenn Knotenposition 1 noch nicht gesetzt ist dann...
                        if (v1.Equals(new Vector3(0, 0, 0)))
                        {
                            // Knotenposition 1 wird auf Ortsvektor(transform) des colliders gesetzt
                            v1 = hit.collider.transform.position;
                            Debug.Log("v1 set");
                        }
                        else
                        {
                            // Wenn Knotenposition 2 noch nicht gesetzt ist dann...
                            if ((v2.Equals(new Vector3(0, 0, 0))))
                            {
                                // Knotenposition 2 wird auf Ortsvektor(transform) des colliders gesetzt
                                v2 = hit.collider.transform.position;
                                // Wenn Knotenposition 1 und Knotenposition 2 gleich sind dann... 
                                if (v1.Equals(v2))
                                {
                                    Debug.Log("nicht 2mal den selben pls");
                                    // Zurücksetzen von Knotenposition 2 auf Ursprung zum reset
                                    v2 = new Vector3(0, 0, 0);
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
                //Zum erfassen der Position
                RaycastHit hit;
                // Wenn der Strahl was trifft dann...
                if (Physics.Raycast(ray, out hit))
                {
                    // Wenn der Tag übereinstimmt dann...
                    if (hit.collider.tag == "Playfield")
                    {

                        Debug.Log("double click");
                        // Vektor aus Auslesen der Mausposition 
                        mousePos = Input.mousePosition;
                        // z Koordinate von Mauspos setzen
                        mousePos.z = 11;
                        // Übertragen auf Weltkamera
                        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
                        objectPos.z = -1;
                        createStandardNode(objectPos, false, false);
                    }
                }
            }
            //Aktuallisieren der Zeit
            timeBuffer = Time.time;
        }
    }

    //Bestimmte Nodes aus Liste entfernen
    public void removeNode(string name)
    {
        foreach (Node n in nodes)
        {
            if (n.getName() == name)
            {
                nodes.Remove(n);
            }
        }
    }
    //Bestimmte Edges aus Liste entfernen
    public void removeEdge(string name)
    {
        foreach (Edge e in edges)
        {
            if (e.getEdgeName() == name)
            {
                edges.Remove(e);
            }
        }
    }
    //Standard Graphen erstellen
    public void createStandardGraph()
    {
        //Positionzuweisung einzelner Nodes
        Vector3 vs1 = new Vector3(-1, 0, 1);
        Vector3 vs2 = new Vector3(1, 2, 1);
        Vector3 vs3 = new Vector3(1, -2, 1);
        Vector3 vs4 = new Vector3(4, 2, 1);
        Vector3 vs5 = new Vector3(4, -2, 1);
        Vector3 vs6 = new Vector3(6, 0, 1);
        //Jeden Node vom Standard Graphen erstellen
        createStandardNode(vs1, true, false);
        createStandardNode(vs2, false, false);
        createStandardNode(vs3, false, false);
        createStandardNode(vs4, false, false);
        createStandardNode(vs5, false, false);
        createStandardNode(vs6, false, true);
        //Jede Edge vom Standard Graphen erstellen
        createStandardEdge(vs1, vs2, 10, 0);
        createStandardEdge(vs1, vs3, 10, 0);
        createStandardEdge(vs2, vs3, 2, 0);
        createStandardEdge(vs2, vs4, 4, 0);
        createStandardEdge(vs3, vs5, 9, 0);
        createStandardEdge(vs2, vs5, 8, 0);
        createStandardEdge(vs5, vs4, 6, 0);
        createStandardEdge(vs5, vs6, 10, 0);
        createStandardEdge(vs4, vs6, 10, 0);

        // Position zurücksetzen von Knotenposition 1
        v1 = new Vector3(0, 0, 0);
        // Position zurücksetzen von Knotenposition 2
        v2 = new Vector3(0, 0, 0);
    }

    //Normale Node erstellen
    public void createStandardNode(Vector3 position, bool source, bool sink)
    {
        // Übertragen auf Weltkamera
        Vector3 objectPos = position;
        // Setze Tiefe von Node auf -1, um immer vor allem zu sein
        objectPos.z = -1;
        // Standard rotation 
        Quaternion spawnRotation = Quaternion.identity;
        // Erstellen von Node(node aus Prefab, Mausposition, Standardrotation)
        GameObject nodeObject = Instantiate(node, objectPos, spawnRotation);
        // Clone im Namen entfernen
        nodeObject.name = nodeObject.name.Replace("(Clone)", "");
        // Node einen einzigartigen Namen zuweisen 
        nodeObject.name = "Knoten " + nodeNumber;
        // Objekt mit Parametern erstellen
        n = new Node(objectPos, "Knoten " + nodeNumber, source, sink);

        //TextMesh Änderungen
        GameObject[] nodeHolder = GameObject.FindGameObjectsWithTag("TextNode");
        nodeName = nodeHolder[nodeCounter].GetComponent<TextMesh>();
        nodePos = nodeHolder[nodeCounter].GetComponent<RectTransform>();
        nodeCounter++;
        //Node mit Textmesh einen Namen geben
        nodeName.text = "v" + nodeCounter;
        nodePos.position = position;
        //Objekt in Liste hinzufügen
        nodes.Add(n);
        //Tabelleneintrag erzeugen
        rowManager.InstantiateVertex();
        nodeNumber++;
        vertexCount++;

		if (source){
			nodeObject.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = sourceMaterial;
		}

		if (sink){
			nodeObject.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = sinkMaterial;
		}
    }

    //Normale Edge für Standard Graphen erstellen
    public void createStandardEdge(Vector3 v1, Vector3 v2, int capacity, int flow)
    {
        this.v1 = v1;
        this.v2 = v2;
        // Spawne Edge
        GameObject edgeObject = Instantiate(edge, new Vector3(v1.x, v1.y, 0), new Quaternion());

        // Setze Werte für Edge
        constructEdge(edgeObject.GetComponent<LineRenderer>(), edgeObject.transform.GetChild(0).GetComponent<LineRenderer>(), 2, v1, v2);

        //HACK
        if (v1 == new Vector3(1, -2, 1) && v2 == new Vector3(1, 2, 1))
        {
            edgeObject.GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(v1.x, v1.y, 2), new Vector3(v2.x, v2.y, 2) });
        }
        //Neues Edge Objekt erstellen mit passenden Werten
		edgeObject.name = GetNode(v1) + " zu " + GetNode(v2);
        e = new Edge(edgeObject, GetNode(v1), GetNode(v2), edgeObject.name, capacity, flow, false);
        //Edge Objekt in Liste eintragen
        edges.Add(e);
        //Edge auch in Liste in Nodes eintragen, damit Nodes wissen welche Kanten an ihnen sitzt
        AddCEdges(e);

        // TextMesh Änderungen
        edgeHolder = GameObject.FindGameObjectsWithTag("EdgeText");
        edgeName = edgeHolder[edgeCounter].GetComponent<TextMesh>();
        edgePos = edgeHolder[edgeCounter].GetComponent<RectTransform>();
        edgeCounter++;
        edgeName.text = "0 / " + e.getCapacity();
        edgePos.position = Vector3.Lerp(v1, v2, 0.5f);
        Quaternion aimRotation = Quaternion.LookRotation(v2 - v1);
        aimRotation.x = 0;
        aimRotation.y = 0;
        edgePos.rotation = aimRotation;

        //Tabelleneintrag erstellen
        rowManager.InstantiateEdge();
    }
    //Fügt Listeneintrag in Klasse Nodes in Edge Liste damit Nodes wissen welche Edges an ihnen anliegt
    public void AddCEdges(Edge edge)
    {
        foreach (Node n in nodes)
        {
            if (n.getName().Equals(edge.getStart()) || n.getName().Equals(edge.getEnd()))
            {
                n.cEdges.Add(edge);
            }
        }
    }

    // Werte der Edge einstellen
    private void constructEdge(LineRenderer edge, LineRenderer animationEdge, int capacity)
    {
        constructEdge(edge, animationEdge, capacity, this.v1, this.v2);
    }

    private void constructEdge(LineRenderer edge, LineRenderer animationEdge, int capacity, Vector3 v1, Vector3 v2)
    {
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

        // maximale Kapazität einstellen
        edge.widthMultiplier = Mathf.Min(capacity * widthMultiplier, maxWidth);
        animationEdge.widthMultiplier = Mathf.Min(capacity * widthMultiplier, maxWidth);
    }

    // Getter für die Knoten eines Edges
    public string GetV1()
    {
        return GetNode(v1);
    }

    public string GetV2()
    {
        return GetNode(v2);
    }
    // Alle Edges als Liste zurückgeben
    public List<Edge> GetAllEdges()
    {
        return edges;
    }
    // Alle Edges als Liste setzen 
    public void SetAllEdges(List<Edge> edges)
    {
        this.edges = edges;
    }
    // Alle Nodes als Liste zurückgeben
    public List<Node> GetAllNodes()
    {
        return nodes;
    }
    // Alle Nodes als Liste setzen
    public void SetAllNodes(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public SimpleRowManager GetRowManager()
    {
        return rowManager;
    }
    //Nodeposition durch Listenindex zurückgeben
    public Vector3 GetNodePosition(int i)
    {
        return nodes[i].nodePosition;
    }
    //Nodename durch Node Position zurückgeben
    public string GetNode(Vector3 nodePosition)
    {
        foreach (Node n in nodes)
        {
            if (n.nodePosition.x == nodePosition.x && n.nodePosition.y == nodePosition.y)
            {
                return n.nodeName;
            }
        }
        return null;
    }
    //Node Objekt durch Node Namen zurückgeben
    public Node GetNodeAsObject(string NodeName)
    {
        foreach (Node n in nodes)
        {
            if (n.nodeName == NodeName)
            {
                return n;
            }
        }
        return null;
    }

	public GameObject GetNodeAsGameObject(string NodeName){
		return GameObject.Find(NodeName);
	}

    public List<Edge> getAllEdges()
    {
        return edges;
    }

    //Quelle zurückgeben
    public Node getSource()
    {
        foreach (Node n in nodes)
        {
            if (n.getSource())
            {
                return n;
            }
        }
        return null;
    }
    //Senke zurückgeben
    public Node getSink()
    {
        foreach (Node n in nodes)
        {
            if (n.getSink())
            {
                return n;
            }
        }
        return null;
    }
}