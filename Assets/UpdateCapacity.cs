using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class UpdateCapacity : MonoBehaviour {

    int capacity;
    private CController ccont;
    private List<Edge> edges;

    void Start()
    {
        ccont = GameObject.Find("GraphController").GetComponent<CController>();
        edges = ccont.GetAllEdges();
    }

    public void LoadCapacity()
    {
        capacity = int.Parse(gameObject.GetComponent<InputField>().text);
        Debug.Log("Kapazität: " + capacity);
        
        for(int i = 0; i < edges.Count; i++)
        {
            Debug.Log(edges[i].edgeName);
        }
    }

}
