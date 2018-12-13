﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool isMuted;
    public List<LevelData> levels;

    public string playerName;
    public int highestTotalScore;
    public string highestTotalScorePlayerName;


    public GameData()
    {
        levels = new List<LevelData>();
    }

    public GameData(List<LevelData> levels)
    {
        this.levels = levels;
    }


}
