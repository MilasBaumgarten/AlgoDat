using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine.SceneManagement;

public class SimpleRowManager : MonoBehaviour {

    //Prefab, welches instanziert werden soll
    public GameObject vertexPrefab;
    public Text vertexName;
    public GameObject vertexSourceToggle;
    public GameObject vertexSinkToggle;

    public GameObject edgePrefab;
    public Text edgeName;
    public Text edgeStart;
    public Text edgeEnd;
    public InputField edgeCapacity;

    public ArrayList edgeStartList; //Liste aller Startpunkte im Graph
    public ArrayList edgeEndList; //Liste aller Endpunkte im Graph

    public bool isEdge;

    private VertexEdgeNameGen nameGen;
    private CController ccont;
    private List<Node> nodes;
    private List<Edge> edges;
    private int currentEdgeIndex;
    private int currentNodeIndex;

    //Container, in welchem das Prefab instanziert werden soll
    public GameObject vertexParent;
    public GameObject edgeParent;

    private bool createStandardGraph;

    void Start()
    {
        nameGen = GameObject.Find("EventSystem").GetComponent<VertexEdgeNameGen>();
        ccont = GameObject.Find("GraphController").GetComponent<CController>();

        edgeStartList = new ArrayList();
        edgeEndList = new ArrayList();

        currentEdgeIndex = 0;
        currentNodeIndex = 0;

        createStandardGraph = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            currentEdgeIndex = 0;
            currentNodeIndex = 0;
            createStandardGraph = true;
        }
    }

    public void InstantiateVertex()
    {
        //Node-Liste aktualisieren
        nodes = ccont.GetAllNodes();

        isEdge = false;
        if(isEdge)
            edgeName.text = nameGen.GenerateEdgeName();
        if (!isEdge)
            vertexName.text = nameGen.GenerateVertexName();
        //Instanzierung des Prefabs
        GameObject nodeInTable = Instantiate(vertexPrefab, vertexParent.transform);
        nodeInTable.name = nodeInTable.name.Replace("(Clone)", "");
        nodeInTable.name = "Knoten " + (ccont.nodeNumber).ToString();
    }

    public void readSourceSink()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            GameObject nodeInTable = GameObject.FindGameObjectWithTag("VertexContent").transform.GetChild(i + 1).gameObject;
            nodeInTable.transform.Find("SourceToggle").GetComponent<Toggle>().isOn = nodes[i].getSource();
            nodeInTable.transform.Find("SinkToggle").GetComponent<Toggle>().isOn = nodes[i].getSink();
        }
    }

    public void InstantiateEdge()
    {
        if (createStandardGraph)
        {
            currentEdgeIndex = 0;
            createStandardGraph = false;
        }

        if(GameObject.FindGameObjectWithTag("EdgeContent").transform.childCount == 1)
        {
            currentEdgeIndex = 0;
        }

        //Edge-Liste aktualisieren
        edges = ccont.GetAllEdges();

        edgeStart.text = ccont.GetV1();
        edgeEnd.text = ccont.GetV2();

        edgeStart.name = edgeStart.text + "(Edge)";
        edgeEnd.name = edgeEnd.text + "(Edge)";

        edgeStartList.Add(ccont.GetV1());
        edgeEndList.Add(ccont.GetV2());
        Debug.Log("CurrentEdgeIndex: " + currentEdgeIndex);
        edgeCapacity.text = edges[currentEdgeIndex].getCapacity().ToString();

        isEdge = true;
        if (isEdge)
        {
            edgeName.text = nameGen.GenerateEdgeName();
        }
        if (!isEdge)
        {
            vertexName.text = nameGen.GenerateVertexName();
        }

        //Instanzierung des Prefabs
        GameObject edgeInTable = Instantiate(edgePrefab, edgeParent.transform);
        currentEdgeIndex = edgeInTable.transform.GetSiblingIndex();

        edgeInTable.name = edgeInTable.name.Replace("(Clone)", "");
        edgeInTable.name = ccont.GetV1() + " zu " + ccont.GetV2();
        
    }

    public ArrayList GetEdgeStartList()
    {
        return edgeStartList;
    }

    public ArrayList GetEdgeEndList()
    {
        return edgeEndList;
    }
}
