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

        if(dataFileName == null)
        {
            dataFileName = "/gameData.json";
        }

        if(loadFromFile)
        {
            data = new GameData();
            LoadData();
        }
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

}
