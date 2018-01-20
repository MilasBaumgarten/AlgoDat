using UnityEngine;
using UnityEngine.UI;

public class SimpleRowManager : MonoBehaviour {

    //Prefab, welches instanziert werden soll
    public GameObject rowPrefab;
    public Text name;

    public bool isEdge;

    private VertexEdgeNameGen nameGen;
    private CController ccont;

    void Start()
    {
        nameGen = GameObject.Find("EventSystem").GetComponent<VertexEdgeNameGen>();
        ccont = GameObject.Find("GraphController").GetComponent<CController>();
    }

    //Container, in welchem das Prefab instanziert werden soll
    public GameObject parent;

    public void InstantiateObject()
    {
        if(isEdge)
            name.text = nameGen.GenerateEdgeName();
        if (!isEdge)
            name.text = nameGen.GenerateVertexName();
        //Instanzierung des Prefabs
        GameObject nodeInTable = Instantiate(rowPrefab, parent.transform);
        nodeInTable.name = nodeInTable.name.Replace("(Clone)", "");
        nodeInTable.name = "KnotenInTabelle " + (ccont.nodeNumber-1).ToString();
    }
}
