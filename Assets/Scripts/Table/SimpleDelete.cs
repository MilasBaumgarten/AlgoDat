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

    void Start()
    {
        ccont = GameObject.Find("GraphController").GetComponent<CController>();
    }
    
    public void DeleteObject()
    {
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

}
