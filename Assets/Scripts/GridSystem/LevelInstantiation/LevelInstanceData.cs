using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class instantiates the given block objects.
[System.Serializable]
public class LevelInstanceData {
    
    public string data;
    public string[] rows;

    // Use this for initialization
    void Start () {
        SplitData();
	}

    private void SplitData()
    {
        rows = data.Split('e');
    }

    // Update is called once per frame
    void Update () {
		
	}
}
