using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour
{
    public IngameUI InGameUIScripts;
    public bool win;
    float timeLeft = 5.0f;

    // Use this for initialization
    void Start()
    {
        win = false;
        Resume();
    }

    // Update is called once per frame
    void Update()
    {

        timeLeft -= Time.deltaTime;

        Debug.Log(timeLeft);
        //if win
        win |= timeLeft < 0;
        if (win)
        {
            Win();
        }
    }

    void Win()
    {
        InGameUIScripts.ShowLevelCompletePanel();
        win = true;
    }

    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Home");
    }
    public void Pause()
    {
        InGameUIScripts.TogglePause();
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        InGameUIScripts.TogglePlay();
        Time.timeScale = 1f;
    }


    /*
    GameData gameData;
    //GoalBlock goalBlock; //uncomment when here IS a goalBlock
    Scene thisScene;

    // Use this for initialization
    void Start () {

        thisScene = SceneManager.GetActiveScene();

    }

    // Update is called once per frame
    void Update () {

        //if (goalBlock.isGoalAchieved) //uncomment when here IS a goalBlock
        {
            List<LevelData> levels = gameData.levels; //why public field and not a getter?
            foreach (LevelData level in levels)
            {
                if (level.sceneID == thisScene.name)
                {
                    level.completed = true; //better rename it to isCompleted 
                }
            }
            SceneManager.LoadScene("LevelComplete"); //or otherwise 
        }

    }
    */
}
