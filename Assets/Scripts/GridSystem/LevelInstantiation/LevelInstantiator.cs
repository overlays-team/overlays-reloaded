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
    public GameObject grid;
    GridPositioner gridPositioner;
    //BlockObjects
    public GameObject wall;
    public GameObject mirror;
    [Tooltip("ImageInput")]
    public GameObject source;
    [Tooltip("ImageOutputMulti")]
    public GameObject targetMulti;

    // Use this for initialization
    void Start () {
        gridPositioner = grid.GetComponent<GridPositioner>();
        LoadData();
        //print("Split data: " + myLevel.rows[0]);
        ApplyLevelData();
    }

    private void ApplyLevelData()
    {
        int rowLength = levelData.GetLength(0); //first index
        int colLength = levelData.GetLength(1); //second index
        gridPositioner.UpdatePlanes(rowLength, colLength, gridPositioner.getPadding());
        //Get 2d array of gridPlanes to use transoforms for intantiation
        GameObject[,] gridPlaneArray = gridPositioner.getGridArray();
        //Instantiation
        int counter = 0; //To get index of children
        for(int row = 0; row < rowLength; row++)
        {
            for(int col = 0; col < colLength; col++)
            {
                //print("[" + row + "][" + col + "] " + levelData[row, col]);
                Transform transform = gridPlaneArray[row, col].transform;
                if (levelData[row, col] == "-1")               //-1 is a hole in the grid
                {
                    gridPlaneArray[row, col].SetActive(false);
                } else if(levelData[row, col] == "1")        //1 is a wall
                {
                    Instantiate(wall);
                    wall.GetComponent<BlockObject>().BlockPositionSetUp(gridPlaneArray[row, col].GetComponent<GridPlane>());
                } else if(levelData[row, col] == "57")   //57 is a mirror variant (/)
                {
                    Instantiate(mirror, new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z), Quaternion.Euler(0, 45, 0));
                } else if(levelData[row, col] == "59")      //59 is another mirror variant (\)
                {
                    Instantiate(mirror, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, -45, 0));
                } else if(levelData[row, col].Contains("00"))     //Multiples of 100 are different targets
                {
                    Instantiate(targetMulti);
                } else if(levelData[row, col].Contains("02"))   //Multiples of 100 ending on 2 means a source with a down output 
                {
                    Instantiate(source);
                }

                counter++;
            }
        }
        
    }

    public void LoadData()
    {
        if (File.Exists(getFilePath()))
        {
            string jsonAsString = File.ReadAllText(getFilePath());
            //print("DataAsJson: " + jsonAsString);
            SplitData(jsonAsString);
        }
        else
        {
            Debug.Log("Please save the .json to " + Application.streamingAssetsPath + " and write '/NAME.json' into script inspector field.");
        }
    }

    public int getIdx0()
    {
        return levelData.GetLength(0);
    }

    public int getIdx1()
    {
        return levelData.GetLength(1);
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
                data2d[i, j] = split[j].Trim();
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
