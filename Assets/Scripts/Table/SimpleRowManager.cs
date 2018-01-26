using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Model;

public class SimpleRowManager : MonoBehaviour {

    //Prefab, welches instanziert werden soll
    public GameObject vertexPrefab;
    public Text vertexName;
    public Toggle vertexSourceToggle;
    public Toggle vertexSinkToggle;

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
    int currentEdgeIndex;

    //Container, in welchem das Prefab instanziert werden soll
    public GameObject vertexParent;
    public GameObject edgeParent;

    void Start()
    {
        nameGen = GameObject.Find("EventSystem").GetComponent<VertexEdgeNameGen>();
        ccont = GameObject.Find("GraphController").GetComponent<CController>();

        edgeStartList = new ArrayList();
        edgeEndList = new ArrayList();

        currentEdgeIndex = 0;
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

    public void InstantiateEdge()
    {
        //Edge-Liste aktualisieren
        edges = ccont.GetAllEdges();

        edgeStart.text = ccont.GetV1();
        edgeEnd.text = ccont.GetV2();

        edgeStart.name = edgeStart.text + "(Edge)";
        edgeEnd.name = edgeEnd.text + "(Edge)";

        edgeStartList.Add(ccont.GetV1());
        edgeEndList.Add(ccont.GetV2());

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
