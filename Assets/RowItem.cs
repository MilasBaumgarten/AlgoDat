using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RowItem : MonoBehaviour {

	public bool GetIsSink()
    {
        return gameObject.transform.Find("SinkToggle").GetComponent<Toggle>().isOn;
    }

    public void SetIsSink(bool isSink)
    {
        gameObject.transform.Find("SinkToggle").GetComponent<Toggle>().isOn = isSink;
    }


    public bool GetIsSource()
    {
        return gameObject.transform.Find("SourceToggle").GetComponent<Toggle>().isOn;
    }

    public void SetIsSource(bool isSource)
    {
        gameObject.transform.Find("SourceToggle").GetComponent<Toggle>().isOn = isSource;
    }
}
