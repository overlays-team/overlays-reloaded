using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class LevelInstantiator : MonoBehaviour {

    public string levelDataPath;

    //Example for 2x2 Array
    private int I00;
    private int I01;
    private int I10;
    private int I11;
    //private string[,] array;

    // Use this for initialization
    void Start () {
        //Filling the objects that becomes the json
        LevelInstantiator myLevel = new LevelInstantiator
        {
            I00 = 1,
            I01 = 2,
            I10 = 3,
            I11 = 4,
            //array = new string[,] { { "00 ", "01 "}, { "10 ", "11 "} },
        };

        
        
        string json = JsonUtility.ToJson(myLevel);
        print("json: "+json);

        LevelInstanceData fromJson = JsonUtility.FromJson<LevelInstanceData>(levelDataPath);
        print("fromJson I00: " + fromJson.I00);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
