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
    //Example for 2x2 Array
    private int I00;
    private int I01;
    private int I10;
    private int I11;
    //private string[,] array;

    // Use this for initialization
    void Start () {
        myLevel = new LevelInstanceData{};
        LoadData();
        //print("Split data: " + myLevel.rows[0]);
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
        string[] temp = rows;
        int count = 0;

        //Split the jsonData into an array containing strings with a row each.
        for(int s = 0; s < rows.Length; s++) {
            string row = rows[s];
            if (row.Contains(",") && row.Any(char.IsDigit))
            {
                temp[count] = row;
                count++;
                print("[" + count + "] " + temp[s]);
            }
        }

        Array.Resize(ref temp, rows.Length - count + 1);
        rows = temp;

        string[] colLength = rows[0].Split(',');
        string[,] data2d = new string[rows.Length, colLength.Length];

        for(int i = 0; i < rows.Length; i++)
        {
            string[] split = rows[i].Split(',');

            for(int j = 0; j < colLength.Length; j++)
            {
                data2d[i, j] = split[j];
                print("["+i+"]["+j+"] " + data2d[i, j]);
            }
        }
        
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
