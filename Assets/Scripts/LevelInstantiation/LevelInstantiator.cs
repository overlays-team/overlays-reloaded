
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;
//This class is used to pull the json/string apart and convert the number indicators into block objets.

[System.Serializable]
public class LevelInstantiator : MonoBehaviour
{
    //For loading of data
    [Tooltip("Please write: '/Levels/NAME.json' here")]
    public string dataFileName;
    [Tooltip("Please write 'FOLDERNAME/abc' here")]
    public string picFolderName;
    private int randomFolder;
    public string[,] levelData;
    public GameObject gridObject;

    private int levelIndex;
    private string jsonAsString;
    private int mirrorCount;

    //Input for managers
    public GameObject grid;
    public Inventory inventory;
    public GameObject ingameManagerObject;
    GridPositioner gridPositioner;
    TimeAttackManager timeAttackManager;
    List<ImageOutput> imageOutputs = new List<ImageOutput>();

    //BlockObjects
    public GameObject wall;
    public GameObject mirror;
    [Tooltip("ImageInput")]
    public GameObject source;
    [Tooltip("ImageOutputMulti")]
    public GameObject targetMulti;
    private Texture2D _goalImage3;
    private Texture2D _goalImage9;
    private Texture2D _sourceImage1;
    private Texture2D _sourceImage2;
    private Texture2D _sourceImage4;
    private Texture2D _sourceImage5;

    // Use this for initialization
    void Start()
    {
        gridPositioner = grid.GetComponent<GridPositioner>();
        timeAttackManager = TimeAttackManager.Instance;
        assignPics();
        LoadData();
        levelIndex = UnityEngine.Random.Range(0, jsonAsString.Split(']').Length - 1);
        InstantiateLevel();
    }

    private void assignPics()
    {
        int numOfFolders = 22; 
        randomFolder = UnityEngine.Random.Range(1, numOfFolders);
        int temp = randomFolder;
        _sourceImage1 = getPicA();
        _sourceImage2 = getPicB();
        _goalImage3 = getPicC();
        while (temp == randomFolder)
        {
            randomFolder = UnityEngine.Random.Range(1, numOfFolders);
            //print("RandomNum: " + randomFolder + " temp: " + temp);
        }
        _sourceImage4 = getPicA();
        _sourceImage5 = getPicB();
        _goalImage9 = getPicC();
    }

    public void InstantiateLevel()
    {
        SplitData(jsonAsString);
        ApplyLevelData();
        levelIndex++;
    }

    public void InstantiateRandomLevel()
    {
        SplitData(jsonAsString);
        levelIndex = UnityEngine.Random.Range(0, jsonAsString.Split(']').Length - 1);
        int temp = levelIndex;
        while(temp == levelIndex)
        {
            levelIndex = UnityEngine.Random.Range(0, jsonAsString.Split(']').Length - 1);
            //print("LvlIndex: " + levelIndex + "Temp: " + temp);
        }
        ApplyLevelData();

    }

    private void ApplyLevelData()
    {
        int rowLength = levelData.GetLength(0); //first index
        int colLength = levelData.GetLength(1); //second index
        gridPositioner.UpdatePlanes(rowLength, colLength, gridPositioner.GetPadding());

        //Get 2d array of gridPlanes to use transoforms for intantiation
        GameObject[,] gridPlaneArray = gridPositioner.GetGridArray();

        //Deletion of earlier Blockobjects
        GameObject[] blockObjects = GameObject.FindGameObjectsWithTag("blockObject");
        //GameObject[] lasers = GameObject.FindGameObjectsWithTag("Laser");
        for (int i = 0; i < blockObjects.Length; i++)
        {
            Destroy(blockObjects[i]);
        }

        //Instantiation
        GameObject instantiatedSource = null;
        GameObject instantiatedMulti = null;
        imageOutputs.Clear();
        int counter = 0; //To get index of children
        mirrorCount = 0; //Counts mirrors in Json top put into inventory
        for (int row = 0; row < rowLength; row++)
        {
            for (int col = 0; col < colLength; col++)
            {
                //GameObject objectToInstantiate;
                //print("[" + row + "][" + col + "] " + levelData[row, col]);

                //Sources ´beginning with a 1 and 2 belong to targets with 3, and sources with 4 and 5 belong to 9
                bool isSource = false;
                Transform transform = gridPlaneArray[row, col].transform;
                if (levelData[row, col] == "-1")               //-1 is a hole in the grid
                {
                    gridPlaneArray[row, col].SetActive(false);
                }
                else if (levelData[row, col] == "1")        //1 is a wall
                {
                    Instantiate(wall, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                    //objectToInstantiate.transform.parent = gridObject.transform;
                    wall.GetComponent<BlockObject>().BlockPositionSetUp(gridPlaneArray[row, col].GetComponent<GridPlane>());
                }
                else if (levelData[row, col] == "57")   //57 is a mirror variant (/)
                {
                    //Instantiate(mirror, new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z), Quaternion.Euler(0, 45, 0));  //This instantiates a mirror in the level
                    mirrorCount++;
                }
                else if (levelData[row, col] == "59")      //59 is another mirror variant (\)
                {
                    //Instantiate(mirror, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, -45, 0));   //This instantiates a mirror in the level
                    mirrorCount++;
                }
                else if (levelData[row, col].Contains("300"))     //10x + 20x -> 300 (source + source -> target)
                {
                    instantiatedMulti = Instantiate(targetMulti, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
                    instantiatedMulti.GetComponent<ImageOutput>().SetupImageOutput(_goalImage3);
                    imageOutputs.Add(instantiatedMulti.GetComponent<ImageOutput>());
                }
                else if (levelData[row, col].Contains("900"))     //Multiples of 100 are different targets
                {
                    instantiatedMulti = Instantiate(targetMulti, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
                    instantiatedMulti.GetComponent<ImageOutput>().SetupImageOutput(_goalImage9);
                    imageOutputs.Add(instantiatedMulti.GetComponent<ImageOutput>());
                }
                else if(levelData[row, col] == "100")   //Targets starting with 1, 2, 4 or 5 only have a single source
                {
                    instantiatedMulti = Instantiate(targetMulti, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
                    instantiatedMulti.GetComponent<ImageOutput>().SetupImageOutput(_sourceImage1);
                    imageOutputs.Add(instantiatedMulti.GetComponent<ImageOutput>());
                }
                else if (levelData[row, col] == "200")
                {
                    instantiatedMulti = Instantiate(targetMulti, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
                    instantiatedMulti.GetComponent<ImageOutput>().SetupImageOutput(_sourceImage2);
                    imageOutputs.Add(instantiatedMulti.GetComponent<ImageOutput>());
                }
                else if (levelData[row, col] == "400")
                {
                    instantiatedMulti = Instantiate(targetMulti, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
                    instantiatedMulti.GetComponent<ImageOutput>().SetupImageOutput(_sourceImage4);
                    imageOutputs.Add(instantiatedMulti.GetComponent<ImageOutput>());
                }
                else if (levelData[row, col] == "500")
                {
                    instantiatedMulti = Instantiate(targetMulti, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
                    instantiatedMulti.GetComponent<ImageOutput>().SetupImageOutput(_sourceImage5);
                    imageOutputs.Add(instantiatedMulti.GetComponent<ImageOutput>());
                }
                else if (levelData[row, col].Contains("02"))   //Multiples of 100 ending on 2 means a source with a down output 
                {
                    instantiatedSource = Instantiate(source, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, 90, 0));
                    isSource = true;
                }
                else if (levelData[row, col].Contains("04"))   //Ending on 4 means left
                {
                    instantiatedSource =  Instantiate(source, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, 180, 0));
                    isSource = true;
                }
                else if (levelData[row, col].Contains("06"))   //6 means right
                {
                    instantiatedSource = Instantiate(source, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, 0, 0));
                    isSource = true;
                }
                else if (levelData[row, col].Contains("08"))   //8 means up
                {
                    instantiatedSource = Instantiate(source, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, -90, 0));
                    isSource = true;
                }

                timeAttackManager.SetOutputImages(imageOutputs);

                if (isSource && levelData[row, col].Contains("10"))
                {
                    //Debug.Log("Sourceimage1");
                    instantiatedSource.GetComponent<ImageInput>().instantiatedInGame = true;
                    instantiatedSource.GetComponent<ImageInput>().SetUpImage(_sourceImage1);
                    //testImage1.sprite = Sprite.Create(_sourceImage1, new Rect(0, 0, _sourceImage1.width, _sourceImage1.height), new Vector2(0.5f, 0.5f));
                }
                else if (isSource && levelData[row, col].Contains("20"))
                {
                    //Debug.Log("Sourceimage2");
                    instantiatedSource.GetComponent<ImageInput>().instantiatedInGame = true;
                    instantiatedSource.GetComponent<ImageInput>().SetUpImage(_sourceImage2);
                    //testImage2.sprite = Sprite.Create(_sourceImage2, new Rect(0, 0, _sourceImage2.width, _sourceImage2.height), new Vector2(0.5f, 0.5f));
                }
                else if (isSource && levelData[row, col].Contains("40"))
                {
                    instantiatedSource.GetComponent<ImageInput>().instantiatedInGame = true;
                    instantiatedSource.GetComponent<ImageInput>().SetUpImage(_sourceImage4);
                }
                else if (isSource && levelData[row, col].Contains("50"))
                {
                    instantiatedSource.GetComponent<ImageInput>().instantiatedInGame = true;
                    instantiatedSource.GetComponent<ImageInput>().SetUpImage(_sourceImage5);
                }


                counter++;
            }
        }
        //inventory.GetComponent<Inventory>().items[0].blockAmount = mirrorCount;
    }

    public void LoadData()
    {
        if (File.Exists(getLevelFilePath()))
        {
            this.jsonAsString = File.ReadAllText(getLevelFilePath());
            //print("DataAsJson: " + jsonAsString);
            SplitData(jsonAsString);
        }
        else
        {
            Debug.Log("Please save the .json to " + Application.streamingAssetsPath + " and write '/Levels/NAME.json' into script inspector field.");
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
        string[] levels = dataAsJson.Split(']');
        string[] rows = levels[levelIndex].Split('"');
        //foreach (string s in rows) { print("s in rows(direkt nach Initialisierung): " + s); }
        int count = 0;

        //Split the jsonData into an array containing strings with a row each.
        for (int s = 0; s < rows.Length; s++)
        {
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

        for (int i = 0; i < rows.Length; i++)
        {
            string[] split = rows[i].Split(',');

            for (int j = 0; j < colLength.Length; j++)
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

    public string getLevelFilePath()
    {
        return Application.streamingAssetsPath + dataFileName;
    }

    public Texture2D getPicA()
    {
        return Resources.Load(picFolderName+"/abc" + randomFolder + "/a") as Texture2D;
    }

    public Texture2D getPicB()
    {
        return Resources.Load(picFolderName+ "/abc" + randomFolder + "/b") as Texture2D;
    }

    public Texture2D getPicC()
    {
        return Resources.Load(picFolderName+ "/abc" + randomFolder + "/c") as Texture2D;
    }

    // Update is called once per frame
    void Update()
    {

    }
}