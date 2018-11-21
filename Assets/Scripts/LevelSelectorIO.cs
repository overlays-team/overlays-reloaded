using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


//class for testing serialization
public class LevelSelectorIO
{

    public GameData data;
    public string dataFileName = "/gameData.json";


    public LevelSelectorIO() { }

    public LevelSelectorIO(GameData data)
    {
        this.data = data;
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




}
