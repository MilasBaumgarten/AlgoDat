using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddNewRow : MonoBehaviour {

    public Transform contentPanel;
    public SimpleObjectPool rowObjectPool;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddRow()
    {
        rowObjectPool.GetObject(contentPanel);
    }
}
