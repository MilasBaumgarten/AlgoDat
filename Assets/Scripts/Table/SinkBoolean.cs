using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class SinkBoolean : MonoBehaviour
{
    //Name des GameObjects, auf welchem der SinkToggle sitzt
    string parentName;
    //Boolean, welche festlegt, ob es sich bei dem Vertex um eine Senke handelt
    public bool isSink;
    //Der betätigte Toggle
    Transform thisToggle;
    
    //CController, über welchen die Liste aller Ecken ausgelesen und bearbeitet wird
    private CController ccont;
    //Die Liste aller Ecken (Nodes)
    private List<Node> nodes;

    //Methode zum Initialisieren von Variablen und Objekten
    void Start()
    {
        //Der ausgewählte Toggle ist der, von dem dieses Skript aufgerufen wird
        thisToggle = gameObject.transform;
        //CController: muss der CController sein, welcher auf GraphController liegt, da dieser vom gesammten Programm verwendet wird
        ccont = GameObject.Find("GraphController").GetComponent<CController>();
    }
    
    //Methode, welche beim anklicken des Toggles aufgerufen wird
    public void TurnOffAllOtherToggles()
    {
        //Den Wert aus dem Toggle auslesen und in entsprechende boolean zwischenspeichern
        isSink = thisToggle.GetComponent<Toggle>().isOn;

        //Wird verwendet, um die isSink-boolean bei allen anderen Ecken auf false zu setzen
        int vertexCount = GameObject.Find("GraphController").GetComponent<CController>().vertexCount;
        //GameObject, auf welchem alle Tabellen-Einträge für die Ecken stehen
        GameObject parent = GameObject.FindGameObjectWithTag("VertexContent");

        //Zugewiesener Knotenname, welcher in der Tabelle angezeigt wird
        parentName = thisToggle.parent.transform.Find("VertexName").GetComponent<Text>().text;

        //Wenn toggle angeschalten wird, werden andere ausgeschalten
        if (thisToggle.gameObject.transform.GetComponent<Toggle>().isOn)
        {
            //Sobald mehr als ein Knoten existiert, darf ein Knoten nur Quelle oder Senke sein
            if (vertexCount > 1)
            {
                //Ein Knoten darf nur Quelle ODER Senke sein
                thisToggle.parent.Find("SourceToggle").GetComponent<Toggle>().isOn = false;
            }
            //Falls nur ein Knoten existiertm darf dieser Quelle und Senke sein
            else
            {
                return;
            }

            //Für jeden Knoten (vertexCount) wird die isSink ausgeschaltet
            for (int i = 1; i <= vertexCount; i++)
            {
                //Tabellen-Eintrag einer anderen Ecke
                Transform other = parent.transform.GetChild(i);
                //Der Name dieser anderen Ecke
                string otherName = other.Find("VertexName").GetComponent<Text>().text;
                //Test-Ausgabe
                Debug.Log(otherName);
                //Wenn der Name der anderen Ecke nicht mit dem Namen der angeklickten Ecke übereinstimmt, wird hier der Toggle auf "aus" gestellt
                if (otherName != parentName)
                    //Da auch durch das ändern des Toggle-Wertes über ein Skript die "OnValueChanged"-Methode aufgerufen wird, muss man nicht nochmal extra die isSink-Variable ändern
                    //--> siehe SetSink
                    other.Find("SinkToggle").GetComponent<Toggle>().isOn = false;
            }
        }
    }

    //Weitere Methode, welche beim anklicken des Toggles aufgerufen wird
    //Diese Methode aktualisiert den isSink-Wert der Node in dem entsprechenden Node-Objekt
    public void SetSink()
    {
        //Der aktuelle Tabellenindex der angeklickten Node
        int currentIndex = gameObject.transform.parent.GetSiblingIndex() - 1;
        //Test-Ausgabe
        Debug.Log("Sink Index: " + currentIndex);

        //Node-Liste wird aus dem CController geladen und in lokale Liste gespeichert
        nodes = ccont.GetAllNodes();
        //Senke updaten
		foreach(Node n in nodes){
			if (n != nodes[currentIndex]){
				n.setSink(false);
				if (!n.isSource)
					ccont.GetNodeAsGameObject(n.getName()).transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = ccont.nodeMaterial;
			}
			else{
				n.setSink(true);
				ccont.GetNodeAsGameObject(n.getName()).transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = ccont.sinkMaterial;
			}
		}

        //Node-Liste im CController aktualisieren
        ccont.SetAllNodes(nodes);
    }
}
