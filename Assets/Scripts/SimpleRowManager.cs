using UnityEngine;
using UnityEngine.UI;

public class SimpleRowManager : MonoBehaviour {

    //Prefab, welches instanziert werden soll
    public GameObject rowPrefab;
    public Text name;

    public bool isEdge;

    private VertexEdgeNameGen nameGen;

    void Start()
    {
        nameGen = GameObject.Find("EventSystem").GetComponent<VertexEdgeNameGen>();
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
        Instantiate(rowPrefab, parent.transform);
    }
}
