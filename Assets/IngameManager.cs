using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour
{
    public IngameUI inGameUIScripts;
    public int wert;
    private bool win;
    private bool lose;
    float timeLeft = 5.0f;

    // Use this for initialization
    void Start()
    {
        win = false;
        lose = false;
        Resume();
        inGameUIScripts.HideLevelCompletePanel();
        inGameUIScripts.HideGameOverPanel();

    }

    // Update is called once per frame
    void Update()
    {

        timeLeft -= Time.deltaTime;

        Debug.Log(timeLeft);

        //if lose
        lose |= timeLeft < 0;
        if (lose)
        {
            Lose();
        }

        //if win
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Win();

        }
    }

    void Win()
    {
        inGameUIScripts.ShowLevelCompletePanel(wert);
        win = true;
        Time.timeScale = 0f;
    }
    void Lose()
    {
        inGameUIScripts.ShowGameOverPanel();
        lose = true;
        Time.timeScale = 0f;
    }

    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    public void Pause()
    {
        inGameUIScripts.TogglePause();
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        inGameUIScripts.TogglePlay();
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