using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataEditor : MonoBehaviour
{
    public static GameDataEditor Instance;
    public string dataFileName;
    public bool loadFromFile;
    public GameData data;

    public string testString = "abc";

    private void Awake()
    {

        Debug.Log("Awake()");


        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        Debug.Log("2, Awake()");
        DontDestroyOnLoad(this);
        Debug.Log("3, Awake()");

        Debug.Log("testString: " + testString);
        Debug.Log("dataFileName:" + "$"+dataFileName +"$");

        Debug.Log("IsNullOrEmpty:" + string.IsNullOrEmpty(dataFileName));
        Debug.Log("dataFileName == \"\" " + (dataFileName == ""));
        Debug.Log("dataFileName == null " + (dataFileName == null));

        if (string.IsNullOrEmpty(dataFileName)) //TODO: nullではなく、""では？ ->正解！！ただし、string.IsNullOrEmpty()を使うべき。
        {
            Debug.Log("4, if");
            dataFileName = "/gameData.json";
            Debug.Log("5, dataFileName" + dataFileName);

            //CreateLevelState();
        }

        Debug.Log("5-1: loadFromFile:" + loadFromFile);
        loadFromFile = File.Exists(getFilePath());
        Debug.Log("5-2: loadFromFile:" + loadFromFile);

        Debug.Log("6, before, if(loadFromFile): " + loadFromFile);
        //TODO : THIS IS IMPORTANT! これなにやってるん？？　ここtrueにせなあかんのちゃうん？
        //しかも、上のifのelseにかかなあかんのちゃう?->いや、関係ない。上のifはファイル名をセットしているだけ。
        if (loadFromFile)  
        {
            Debug.Log("6-2, in, if(loadFromFile)" + loadFromFile);
            data = new GameData();
            LoadData();

            Debug.Log("6-3,  after LoadData()" + data);
        } else {
            Debug.Log("6-4,  else, if (loadFromFile) ");
            CreateLevelState();
        }


        //debug
        ShowGameData();

    }

    //TODO: というか先に、保存問題に取り組むべきか！？！？　いや、Gamedataが空だから保存できないのでは？
    private void ShowGameData()
    {
        Debug.Log("7: "+GameDataEditor.Instance.data.levels.Count);
        for (int i = 0; i<GameDataEditor.Instance.data.levels.Count; i++)
        {
            Debug.Log(GameDataEditor.Instance.data.levels[i].completed);
            Debug.Log(GameDataEditor.Instance.data.levels[i].sceneID);
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }


    private void Start()
    {

    }

    public string getFilePath()
    {
        return Application.persistentDataPath + dataFileName;
    }

    public void LoadData()
    {
        if (File.Exists(getFilePath()))
        {
            string dataAsJson = File.ReadAllText(getFilePath());
            data = JsonUtility.FromJson<GameData>(dataAsJson);
        }
        else
        {
            data = new GameData();
        }
    }

    public void SaveData()
    {
        Debug.Log("hallo SaveData()");
        string dataString = JsonUtility.ToJson(data);

        Debug.Log("getFilePath(): " + getFilePath());
        File.WriteAllText(getFilePath(), dataString);
    }

    public void ClearData()
    {
        data = new GameData();

        SaveData();
    }


    private void CreateLevelState()
    {
        Debug.Log("こんにちは、CreateLevelState()");

        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL1", true));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL2", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL3", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL4", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL5", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL6", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL7", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL8", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL9", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL11", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL12", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL13", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL14", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL15", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL16", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL17", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL18", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL19", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL20", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL21", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL22", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL23", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL24", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL25", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL26", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL27", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL28", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL29", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL30", false));

        //GameDataEditor.Instance.data.levels[0].score = 2;
        //GameDataEditor.Instance.data.levels[1].score = 1;
    }

}
