using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour {

    GameData gameData;
    GoalBlock goalBlock;
    Scene thisScene;

    // Use this for initialization
    void Start () {

        thisScene = SceneManager.GetActiveScene();

    }

    // Update is called once per frame
    void Update () {

        if (goalBlock.isGoalAchieved)
        {
            List<LevelData> levels = gameData.getLevels();
            foreach (LevelData level in levels)
            {
                if (level.getName == thisScene.name)
                {
                    level.isCompleted = true;
                }
            }
            SceneManager.LoadScene("LevelComplete"); //or otherwise 
        }
		
	}
}
