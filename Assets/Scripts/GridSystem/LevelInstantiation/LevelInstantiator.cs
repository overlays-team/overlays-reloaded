using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
//This class is used to pull the json/string apart and convert the number indicators into block objets.

[System.Serializable]
public class LevelInstantiator : MonoBehaviour {

    public string dataFileName;
    LevelInstanceData myLevel;
    public string[,] levelData;

    //Input for managers
    public GameObject Grid;

    // Use this for initialization
    void Start () {
        myLevel = new LevelInstanceData{};
        LoadData();
        //print("Split data: " + myLevel.rows[0]);
        ApplyLevelData();
    }

    private void ApplyLevelData()
    {

    }

    public void LoadData()
    {
        if (File.Exists(getFilePath()))
        {
            string jsonAsString = File.ReadAllText(getFilePath());
            //print("DataAsJson: " + jsonAsString);
            SplitData(jsonAsString);
            //myLevel = JsonUtility.FromJson<LevelInstanceData>(dataAsJson);
            myLevel.data = jsonAsString;
        }
        else
        {
            Debug.Log("Please save the .json to " + Application.streamingAssetsPath + " and write '/NAME.json' into script inspector field.");
        }
    }

    private void SplitData(string dataAsJson)
    {
        string[] rows = dataAsJson.Split('"');
        //foreach (string s in rows) { print("s in rows(direkt nach Initialisierung): " + s); }
        int count = 0;

        //Split the jsonData into an array containing strings with a row each.
        for(int s = 0; s < rows.Length; s++) {
            string row = rows[s];
            if (row.Contains(",") && row.Any(char.IsDigit))
            {
                rows[count] = row;
                count++;
                //print("[" + count + "] " + temp[s]);
            }
        }
        //foreach (string s in rows) { print("s in rows(nach erster Sortierung): " + s); }
        //print("count nach Sortierung: " + count);
        Array.Resize(ref rows, count);
        //foreach (string s in rows){print("s in rows(nach resizing): " + s);}

        //print("rowsLength: " + rows.Length);

        string[] colLength = rows[0].Split(',');
        string[,] data2d = new string[rows.Length, colLength.Length];

        for(int i = 0; i < rows.Length; i++)
        {
            string[] split = rows[i].Split(',');

            for(int j = 0; j < colLength.Length; j++)
            {
                data2d[i, j] = split[j];
                //print("["+i+"]["+j+"] " + data2d[i, j]);
            }
        }

        this.levelData = data2d;
    }
    
    /*
     * Example Data
     {
        "level1": [
	    "102, 1, -1, 300, 59",
	    "59, 0, -1, 57, 0",
	    "-1, 0, -1, 0, 0",
	    "206, 0, -1, -1, 57" ]
     }
     */

    public string getFilePath()
    {
        return Application.streamingAssetsPath + dataFileName;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
