using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataEditor : MonoBehaviour
{
    public static GameDataEditor Instance;
    public string dataFilePath = "/gameData.json";
    public string defaultDataPath = "Data/DefaultGameData";
    public bool loadFromDefault;
    public GameData data;

    private void Awake()
    {
        ForceSingletonPattern();

        if(string.IsNullOrEmpty(dataFilePath))
        {
            dataFilePath = "/gameData.json";
        }

        if(loadFromDefault)
        {
            LoadDefaultData();
        }
        else
        {
            if (File.Exists(getFilePath()))
            {
                LoadData();
            }
            else
            {
                LoadDefaultData();
            }
        }
    }

    private void Start()
    {

    }

    void ForceSingletonPattern()
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
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public string getFilePath()
    {
        return Application.persistentDataPath + dataFilePath;
    }

    public void LoadData()
    {
        string dataAsJson = File.ReadAllText(getFilePath());
        data = JsonUtility.FromJson<GameData>(dataAsJson);
        print(getFilePath());
    }

    public void LoadDefaultData()
    {
        TextAsset defaultGameData = Resources.Load(defaultDataPath) as TextAsset;
        data = JsonUtility.FromJson<GameData>(defaultGameData.text);
    }

    public void SaveData()
    {
        string dataString = JsonUtility.ToJson(data);
        File.WriteAllText(getFilePath(), dataString);
    }

    public void ResetData()
    {
        LoadDefaultData();
        SaveData();
    }

    private void PrintGameData()
    {
        Debug.Log("Showing Game Data: ");
        Debug.Log("Level Count(): " + GameDataEditor.Instance.data.levels.Count);
        for (int i = 0; i < GameDataEditor.Instance.data.levels.Count; i++)
        {
            Debug.Log(GameDataEditor.Instance.data.levels[i].completed);
            Debug.Log(GameDataEditor.Instance.data.levels[i].sceneID);
        }
    }
}
