using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CController : MonoBehaviour {

	//Knoten reseten nachdem 2 ausgewählt wurden ohne Edge zu erstellen programmieren


    public GameObject edge; // Gameobjekt für Kante
    public GameObject node; // Gameobjekt für Knoten
    public float timeWindow = 0.25f; // Zeitinterval für doppelklicks
    public double timeBuffer = 0; // Zeit seit letzem Klick
    Vector3 mousePos = new Vector3(0, 0, 0); // Raumvektor für Mausposition

    Vector3 v1 = new Vector3(0, 0, 0); // Platzhalter für Knotenposition 1
    Vector3 v2 = new Vector3(0, 0, 0); // Platzhalter für Knotenposition 2
	
    void Update()
    {
        ClickTracker(); 
    }

    public void ClickTracker()
    {
        if (Input.GetMouseButtonDown(1)) // Wenn rechte Maustaste gedrückt dann...
        {
            CreateEdge();
        }

        if (Input.GetMouseButtonDown(0)) // Wenn linke Maustaste gedrückt dann...
        {
            if ((Time.time - timeBuffer) > timeWindow)// Wenn Zeit seit letztem Klick größer als Zeitinterval für Doppelklicks ist dann...
            {
                Debug.Log("single click");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Erzeugen von einem Strahl(Ray) an der Mausposition
                RaycastHit hit; //Zum erfassen der Position 

                if (Physics.Raycast(ray, out hit)) // Wenn der Strahl was trifft dann...
                {
                    if (hit.collider.tag == "Node")// Wenn der Tag übereinstimmt dann...
                    {
                        Debug.Log("This is a Node");
                        if (v1.Equals(new Vector3(0, 0, 0)))// Wenn Knotenposition 1 noch nicht gesetzt ist dann...
                        {
                            v1 = hit.collider.transform.position; // Knotenposition 1 wird auf Ortsvektor(transform) des colliders gesetzt
                            Debug.Log("v1 set");
                        }
                        else
                        {
                            if ((v2.Equals(new Vector3(0, 0, 0))))// Wenn Knotenposition 2 noch nicht gesetzt ist dann...
                            {
                                v2 = hit.collider.transform.position; // Knotenposition 2 wird auf Ortsvektor(transform) des colliders gesetzt
                                if (v1.Equals(v2)) // Wenn Knotenposition 1 und Knotenposition 2 gleich sind dann... 
                                {
                                    Debug.Log("nicht 2mal den selben pls");
                                    v2 = new Vector3(0, 0, 0); // Zurücksetzen von Knotenposition 2 auf Ursprung zum reset
                                }
                                else
                                {
                                    Debug.Log("v2 set");
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("This isn't a Node");
                    }
                }
            }
            else
            {
                Debug.Log("double click");
                CreateNode();
            }
            timeBuffer = Time.time; //Aktuallisieren der Zeit
        }
    }

    public void CreateNode()
    {
        mousePos = Input.mousePosition; // Vektor aus Auslesen der Mausposition 
//!!!
        mousePos.z = 11; // z Koordinate von Mauspos setzen
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos); // Übertragen auf Weltkamera 
        Quaternion spawnRotation = Quaternion.identity; // Standard rotation 
        Instantiate(node, objectPos, spawnRotation); // Erstellen von Knoten(node aus Prefab, Mausposition, Standardrotation)
        Debug.Log("erstellt");
    }

    public void CreateEdge()
    {
        Vector3 pos = Vector3.Lerp(v1, v2, 0.5f); // Punkt auf der Mitte von Knotenposition 1 und Knotenposition 2
        Vector3 aim = v2 - v1;

        if (v1.Equals(new Vector3(0, 0, 0)) || v2.Equals(new Vector3(0, 0, 0))) // Wenn Knotenposition 1 oder Knotenposition 2 auf Ursprung stehen
        {
            Debug.Log("Bitte 2 Knoten auswählen");
        }
        else
        {
            Quaternion aimRotation = Quaternion.LookRotation(aim); // Anpassen der Rotation damit sie in Richtung des Knotens zeigt
//!!!

            edge.transform.localScale = new Vector3(0.3f, 0.25f, (Vector3.Distance(v1, v2)) / 2);// Skalierung des Prefabs
//!!!
            Instantiate(edge, new Vector3(pos.x, pos.y, 1), aimRotation); // Erstellen der Kante (edge aus Prefab, von Mittelpunkt 
                                                                                      // zwischen den Knoten, Rotation in Richtung der Knoten)
           
            v1 = new Vector3(0, 0, 0); // Position zurücksetzen von Knotenposition 1
            v2 = new Vector3(0, 0, 0); // Position zurücksetzen von Knotenposition 2
            Debug.Log("reset");
            Debug.Log("erstellt");
        }
    }
}