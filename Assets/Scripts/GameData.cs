using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool isMuted;
    public List<LevelData> levels;

    public GameData()
    {
        levels = new List<LevelData>();
    }
}
