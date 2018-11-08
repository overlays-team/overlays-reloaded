using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataEditor : MonoBehaviour
{
    public static GameDataEditor Instance;
    public bool clearData = false;
    public string saveDataName;
    public float playerPrice;
    public GameData data;

    private string filePath;

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

        saveDataName = "/gameData.json";

        filePath = Application.persistentDataPath + saveDataName;

        data = new GameData();

        LoadData();

        if (clearData)
        {
            ClearData();
        }
    }

    private void Start()
    {

    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
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

        File.WriteAllText(filePath, dataString);
    }

    public void ClearData()
    {
        data = new GameData();

        SaveData();
    }

    public bool IsMuted()
    {
        return data.isMuted;
    }

    public void AddLevel()
    {
        LevelData level = new LevelData();
        data.levels.Add(level);

        SaveData();
    }
}
