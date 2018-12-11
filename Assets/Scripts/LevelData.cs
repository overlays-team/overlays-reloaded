using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string sceneID;

    public bool completed;

    public int score;

	public LevelData()
	{

	}

    public LevelData(string id, bool c)
    {
        this.sceneID = id;
        this.completed = c;
    }



}
