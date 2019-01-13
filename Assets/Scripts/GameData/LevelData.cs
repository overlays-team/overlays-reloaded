using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string name;
    public string sceneID;
    public string previewImage;
    public string description;
    public bool completed;
    public int score;

    public LevelData(string id, string previewImage)
    {
        this.sceneID = id;
        this.previewImage = previewImage;
        this.description = "";
        this.completed = false;
        this.score = 0;
    }
}
