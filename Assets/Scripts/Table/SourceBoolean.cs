using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class SourceBoolean : MonoBehaviour {

    //Name des GameObjects, auf welchem der Toggle sitzt
    string parentName;
    //boolean, welche festlegt, ob es sich bei der Entsprechenden Node um eine Quelle handel
    public bool isSource;

    //CController, von welchem die Node-Liste geladen wird
    private CController ccont;
    //Node Liste, welche in dieser Methode bearbeitet wird
    private List<Node> nodes;
    //Der Toggle, welcher angeklickt wurde
    Transform thisToggle;

    //Methode zum Initialisieren von Variablen und Objekten
    void Start()
    {
        //CController: Muss aus dem GraphController geladen werden, da dieser vom gesammten Programm verwendet wird
        ccont = GameObject.Find("GraphController").GetComponent<CController>();
        //Der Toggle, welcher angeklickt wurde
        thisToggle = gameObject.transform;
    }

    //Methode, welche beim anklicken des Toggles aufgerufen wird
	public void TurnOffAllOtherToggles()
    {
        //boolean bekommt den aus dem Toggle ausgelesenen Wert zugewiesen
        isSource = thisToggle.GetComponent<Toggle>().isOn;

        //Anzahl der erstellten Nodes -> wird verwendet, um später die isSource-boolean bei allen anderen Nodes auf false zu setzen
        int vertexCount = GameObject.Find("GraphController").GetComponent<CController>().vertexCount;
        //Das GameObject, auf welchem alle Tabelleneinträge sitzen
        GameObject parent = GameObject.FindGameObjectWithTag("VertexContent");

        //Zugewiesener Knotenname, welcher in der Tabelle angezeigt wird
        parentName = thisToggle.parent.transform.Find("VertexName").GetComponent<Text>().text;

        //Wenn toggle angeschalten wird, werden andere ausgeschalten
        if (thisToggle.GetComponent<Toggle>().isOn)
        {
            //Wenn mehr als ein Knoten existiert, darf ein Knoten nur Quelle oder Senke sein
            if(vertexCount > 1)
            {
                //Ein Knoten darf nur Quelle ODER Senke sein
                thisToggle.parent.Find("SinkToggle").GetComponent<Toggle>().isOn = false;
            }
            //Falls nur ein Knoten existiert, darf dieser Quelle und Senke gleichzeitig sein
            else
            {
                return;
            }

            //Jeden Knoten in der Tabelle durchlaufen
            for (int i = 1; i <= vertexCount; i++)
            {
                //Ein "anderer" Knoten in der Tabelle
                Transform other = parent.transform.GetChild(i);
                //Der Name des anderen Knoten
                string otherName = other.Find("VertexName").GetComponent<Text>().text;

                //Test-Ausgabe des anderen Knotennamens
                Debug.Log(otherName);
                //Wenn der andere Knotenname nicht mit dem angeklickten Knoten übereinstimmt, wird dessen Toggle-Wert auf false gesetzt
                if(otherName != parentName)
                    other.Find("SourceToggle").GetComponent<Toggle>().isOn = false;
            }
        }
    }

    //Weitere Methode, welche beim Anklicken des Toggles aufgerufen wird
    public void SetSource()
    {
        //Der aktuelle Index des Knotens in der Tabelle
        //Dieser Stimmt mit dem Index des korrepsondierenden Knotens in der Node-Liste überein
        int currentIndex = gameObject.transform.parent.GetSiblingIndex() - 1;
        //Test-Ausgabe um den Index zu überprüfen
        Debug.Log("Source Index: " + currentIndex);

        //Die Node-Liste aus der CController auslesen
        nodes = ccont.GetAllNodes();
		// Quelle einfärben
		

        //Quelle updaten
		foreach(Node n in nodes){
			if (n != nodes[currentIndex]){
				n.setSource(false);
				if (!n.isSink)
					ccont.GetNodeAsGameObject(n.getName()).transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = ccont.nodeMaterial;
			}
			else{
				n.setSource(true);
				ccont.GetNodeAsGameObject(nodes[currentIndex].getName()).transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = ccont.sourceMaterial;
			}
		}

        //Die Node-Liste aus dem CController wird aktualisiert
        ccont.SetAllNodes(nodes);
    }
}
