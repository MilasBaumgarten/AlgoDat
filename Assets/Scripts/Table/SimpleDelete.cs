using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDelete : MonoBehaviour {

    //Parent-Objekt des Buttons = RowItem
    private GameObject parent;

    private string objectName;
    private GameObject deleteThis;

    private CController ccont;

    private SimpleRowManager rowManager; //Benötigt, um die Liste aller Start- und Endpunkte im Graph auszulesen
    private ArrayList edgeStartList;
    private ArrayList edgeEndList;

    void Start()
    {
        ccont = GameObject.Find("GraphController").GetComponent<CController>();
        rowManager = ccont.GetRowManager();
    }
    
    public void DeleteVertex()
    {
        //Auslesen der im Knoten anliegenden Kanten
        edgeStartList = rowManager.GetEdgeStartList();
        edgeEndList = rowManager.GetEdgeEndList();
        Debug.Log(gameObject.transform.parent.name);
        Debug.Log(edgeStartList.Count);
        /*
        for (int i = 0; i < edgeStartList.Count; i++)
        {
            GameObject deleteThis = null;
            //Wenn ein Knoten aus der Startliste der Edges mit dem gelöschten Knoten übereinstimmt, wird die Entsprechende Kante gelöscht

            //Gameobject, dessen parent dann gelöscht wird
            Debug.Log(edgeStartList[i].ToString());
            if (gameObject.transform.parent.name.Equals(edgeStartList[i].ToString()) || gameObject.transform.parent.name.Equals(edgeEndList[i].ToString()))
            {
               deleteThis = GameObject.Find(gameObject.transform.parent.name + "(Edge)").transform.parent.gameObject;
            }
            Debug.Log("Delete: " + deleteThis.name);
            Destroy(deleteThis);

        }
        */
        /* Löschen der Zeile in der Tabelle */

        //Parent-Objekt initialisieren
        parent = gameObject.transform.parent.gameObject;
        //Parent-Objekt löschen
        Destroy(parent);


        /* Löschen des Knotens oder der Kante */
        objectName = parent.gameObject.transform.Find("VertexName").GetComponent<Text>().text;
        deleteThis = GameObject.Find(objectName);
        Destroy(deleteThis);

        //Knotennummer verringern
        ccont.vertexCount--;
    }

    public void DeleteEdge()
    {
        parent = gameObject.transform.parent.gameObject;
        
        Destroy(parent);
        Debug.Log("Parent name: " + parent.name);
        
        //Da die erstellten Objekte gleich heißen, können sie m.H. ihres Namens gelöscht werden
        Destroy(GameObject.Find(parent.name));
    }

}
