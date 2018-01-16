using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDelete : MonoBehaviour {

    //Parent-Objekt des Buttons = RowItem
    private GameObject parent;
    
    public void DeleteObject()
    {
        //Parent-Objekt initialisieren
        parent = gameObject.transform.parent.gameObject;
        //Parent-Objekt löschen
        Destroy(parent);
    }

}
