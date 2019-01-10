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

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);

        if(string.IsNullOrEmpty(dataFileName))
        {
            dataFileName = "/gameData.json";
        }

        if(loadFromFile)
        {
            data = new GameData();
            LoadData();
        } else {
            CreateLevelStates();
        }

        //debug
        ShowGameData();
    }

    private void ShowGameData()
    {
        Debug.Log("ShowGameData(): " + GameDataEditor.Instance.data.levels.Count);
        for (int i = 0; i < GameDataEditor.Instance.data.levels.Count; i++)
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
        string dataString = JsonUtility.ToJson(data);

        File.WriteAllText(getFilePath(), dataString);
    }

    public void ClearData()
    {
        data = new GameData();

        SaveData();
    }

    private void CreateLevelStates()
    {
        if (GameDataEditor.Instance.data.levels.Count == 0)
        {
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
        }
    }

}
