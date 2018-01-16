using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteRow : MonoBehaviour {

    public Transform contentPanel;
    public GameObject currentRow;
    public SimpleObjectPool rowObjectPool;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void removeRow()
    {
        if (contentPanel.childCount > 0)
        {
            GameObject toRemove = currentRow;
                rowObjectPool.ReturnObject(toRemove);
        }
    }
}
