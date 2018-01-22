using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleRowManager : MonoBehaviour {

    //Prefab, welches instanziert werden soll
    public GameObject vertexPrefab;
    public Text vertexName;

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

    //Container, in welchem das Prefab instanziert werden soll
    public GameObject vertexParent;
    public GameObject edgeParent;

    void Start()
    {
        nameGen = GameObject.Find("EventSystem").GetComponent<VertexEdgeNameGen>();
        ccont = GameObject.Find("GraphController").GetComponent<CController>();

        edgeStartList = new ArrayList();
        edgeEndList = new ArrayList();
    }

    public void InstantiateVertex()
    {
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
        edgeStart.text = ccont.GetV1();
        edgeEnd.text = ccont.GetV2();

        edgeStart.name = edgeStart.text + "(Edge)";
        edgeEnd.name = edgeEnd.text + "(Edge)";

        edgeStartList.Add(ccont.GetV1());
        edgeEndList.Add(ccont.GetV2());

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
