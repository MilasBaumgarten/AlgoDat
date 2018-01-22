using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class UpdateCapacity : MonoBehaviour {
    //Kapaziät der Edges
    int capacity;
    //CController, von welchem die Edge-Liste geholt wird
    private CController ccont;
    //Die Edge-Liste, welche in dieser Klasse bearbeitet wird
    private List<Edge> edges;

    //Methode zum inititalisieren von Variablen und Objekten
    void Start()
    {
        //Initialisierung der CControllers: Muss der CController sein, welcher auf dem GraphController sitzt
        ccont = GameObject.Find("GraphController").GetComponent<CController>();
    }

    //Methode, welche beim ändern des InputField-Values aufgerufen wird
    public void LoadCapacity()
    {
        //Eingegebene Kapazität aus dem InputField auslesen
        capacity = int.Parse(gameObject.GetComponent<InputField>().text);
        //Test DebugLog
        Debug.Log("Kapazität: " + capacity);

        //Index der Tabellenzeile in welchem das InputField sitzt
        //Stimmt mit dem Index des entsprechenden Edge in der Edge-Liste überein
        int currentIndex = gameObject.transform.parent.GetSiblingIndex() - 1;

        //Zur Zeit geladenen Edges aus dem CController laden
        edges = ccont.GetAllEdges();
        //Die Kapazität des Edge mit dem ausgelesenen Index festlegen
        edges[currentIndex].setCapacity(capacity);

        //Edge-Liste in dem CController updaten
        ccont.SetAllEdges(edges);

        //Testweise wird die lokale Edge-Liste nochmal aktualisiert
        edges = ccont.GetAllEdges();
        //Die Kapazität von jedem gespeicherten Edge ausgeben
        foreach(Edge e in edges)
        {
            //Debug-Ausgabe
            Debug.Log("Kapazität: " + e.getCapacity());
        }
    }

}
