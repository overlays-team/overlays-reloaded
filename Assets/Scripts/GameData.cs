using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool isMuted;
    public List<Level> levels;

    public GameData()
    {
        level = new List<Level>();
    }
}
