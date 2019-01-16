using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class LevelInstantiator : MonoBehaviour {

    public string dataFileName;
    LevelInstanceData myLevel;
    //Example for 2x2 Array
    private int I00;
    private int I01;
    private int I10;
    private int I11;
    //private string[,] array;

    // Use this for initialization
    void Start () {
        //Filling the objects that becomes the json
        myLevel = new LevelInstanceData{};
        LoadData();
        Debug.Log("00: "+myLevel.I00+" 01: "+myLevel.I01);
    }

    public void LoadData()
    {
        if (File.Exists(getFilePath()))
        {
            string dataAsJson = File.ReadAllText(getFilePath());
            print("DataAsJson: "+dataAsJson);
            myLevel = JsonUtility.FromJson<LevelInstanceData>(dataAsJson);
        }
        else
        {
            Debug.Log("Please save correct " + dataFileName + " to " + Application.persistentDataPath);
        }
    }

    public string getFilePath()
    {
        return Application.persistentDataPath + dataFileName;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
