﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour
{
    public IngameUI ingameUI;

    [SerializeField]
    private bool attackMode;

    //sh
    public int star;
    private int scoreFactor = 10;
    private int thisLevelScore;
    private int previousTotalScore;
    private int newTotalScore;
    //private string playerName;
    //private int highestTotalScore;
    //private string highestTotalScorePlayerName;

    private bool timeRunsOut;


    private bool win;
    private bool lose;
    public float timeLeft = 5.0f;
    bool paused;


    public ImageOutput[] outputImages; //holds a collection of all output Images

    // Use this for initialization
    void Start()
    {
        win = false;
        lose = false;
        Resume();
        ingameUI.HideLevelCompletePanel();
        ingameUI.HideGameOverPanel();

        setTestParameters();

        LoadLevelState();
    }

    private void setTestParameters()
    {
        SetAttackMode(attackMode); //sh, for testing

        star = Random.Range(1, 4); //sh, for testing. generate score randomlly.
        //CreateTestLevelState(); //sh, needed for test
        GameDataEditor.Instance.data.highestTotalScore = 177;//sh, for testing
        GameDataEditor.Instance.data.playerName = "SHUYA";
        if(attackMode)ingameUI.ShowCountDownText(attackMode);
        if(attackMode)ingameUI.ShowHighestScorePanel(attackMode);
    }


    // Update is called once per frame
    void Update()
    {
        if (!win && !lose && !paused)
        {
            if (attackMode)CountTime();
            CheckIfWeWon();

        
            if (lose)
            {
                Lose();

            }

        }
            //sh, for debug, force win
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Win();
            }

        


    }



    //sh
    private void LoadLevelState() //now only used for getting total score
    {
        //highestTotalScore = GameDataEditor.Instance.data.highestTotalScore;
        //highestTotalScorePlayerName = GameDataEditor.Instance.data.highestTotalScorePlayerName;

        int numberOfLevelsInGameData = GameDataEditor.Instance.data.levels.Count;
        previousTotalScore = 0;

        for (int i = 0; i < numberOfLevelsInGameData; i++)
        {
            if (GameDataEditor.Instance.data.levels[i].completed)
            {
                previousTotalScore += GameDataEditor.Instance.data.levels[i].score;
            }
        }
    }


    void Win()
    {
        StopCoroutine("WinCoroutine");
        thisLevelScore = star * scoreFactor;

        UpdateTotalScore();
        CheckHighestTotalScore();
        ingameUI.ShowLevelCompletePanel(star, newTotalScore, GameDataEditor.Instance.data.highestTotalScore, attackMode);

        win = true;

        //sh
        SaveLevelState();

        LoadLevelState(); //for test we need this here.
    }



    private void UpdateTotalScore()
    {
        newTotalScore = thisLevelScore + previousTotalScore;
    }


    private void CheckHighestTotalScore()
    {
        //if it's not attack mode, do nothing.
        if (!attackMode) return;

        if (GameDataEditor.Instance.data.highestTotalScore < newTotalScore)
        {
            SaveHighestTotalScore();
        }
    }

    private void SaveHighestTotalScore()
    {
        GameDataEditor.Instance.data.highestTotalScore = newTotalScore;
        GameDataEditor.Instance.data.highestTotalScorePlayerName = GameDataEditor.Instance.data.playerName;
    }

    //sh
    public void SetAttackMode(bool attackMode)
    {
        this.attackMode = attackMode;
    }

    //sh
    private void SaveLevelState()
    {
        int numberOfLevelsInGameData = GameDataEditor.Instance.data.levels.Count;
        //Debug.Log("numberOfLevelsInGameData: " + numberOfLevelsInGameData);

        int lastCompletedLevel = 0;
        int currentLevel = 0;
        int nextLevel = 0;

        //get last completed level 
        for (int i = 0; i < numberOfLevelsInGameData; i++)
        {
            string sceneName = "Level" + (i + 1);
            if (GameDataEditor.Instance.data.levels[i].completed)
            {
                lastCompletedLevel = i;
            }
            if (SceneManager.GetActiveScene().name.Equals(sceneName))
            {
                Debug.Log("scene name is same");
                nextLevel = i + 1;
                currentLevel = i;
            }
        }

        //get currentLevel
        if ((lastCompletedLevel == 0) && (numberOfLevelsInGameData == 0)) //for THE first time
        {
            currentLevel = 0; //not necessarily but for security
        }
        /*
        else
        {
            currentLevel = lastCompletedLevel + 1;
        }
        */

        //save score and win/lose state

        GameDataEditor.Instance.data.levels[currentLevel].star = star;
        GameDataEditor.Instance.data.levels[currentLevel].score = thisLevelScore;
        GameDataEditor.Instance.data.levels[currentLevel].completed = win;

        GameDataEditor.Instance.data.levels[nextLevel].completed = true;


        //for future development
        /*
        Scene thisScene = SceneManager.GetActiveScene();
        for (int i = 0; i<numberOfLevelsInGameData; i++)
        {
            if (GameDataEditor.Instance.data.levels[i].sceneID == thisScene.name)
            {
                GameDataEditor.Instance.data.levels[i].score = score;
                GameDataEditor.Instance.data.levels[i].completed = win; 
            }
        }
        */

        //for saving test
        //GameDataEditor.Instance.SaveData();
    }



    void Lose()
    {
        ingameUI.ShowGameOverPanel();
        lose = true;
    }

    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }

    //reload same scene for test
    public void NextTest()
    {
        Retry();
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
        paused = true;
        ingameUI.TogglePause();
        //Time.timeScale = 0f;
    }
    public void Resume()
    {
        paused = false;
        ingameUI.TogglePlay();
        Time.timeScale = 1f;
    }


    //sh
    private void CountTime()
    {
        if (!win && attackMode)
        {
            timeLeft -= Time.deltaTime;
        }
        timeRunsOut = timeLeft < 0;

        if(attackMode)ingameUI.UpdateCountDown(timeLeft, timeRunsOut);
    }


    public bool CheckIfWeWon()
    {
        bool allCorrect = true; //sh: false as default is better and remove else?. 

        //sh
        if (timeRunsOut &!lose)
        {
            //sh, Lose() works here but not in IEnumerator WinCoroutine()
            Lose();
            return false;
        }


        if (outputImages.Length > 0)
        {
            foreach (ImageOutput imageOutput in outputImages)
            {
                if (!imageOutput.imageCorrect) allCorrect = false;
            }

            if (allCorrect) StartCoroutine("WinCoroutine");

            //weil manchmal ein laser nur für eine Sekunde richtig ist, warten wir eine Sekunde

        }
        else
        {
            allCorrect = false;
        }


        return allCorrect;
    }


    IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(1);
        if (CheckIfWeWon()) Win();

        /*
        if (CheckIfWeWon()){
            Win();
        } else {
            Lose();
        }
        */
    }


    
    //sh
    /*
    private void CreateTestLevelState()
    {
        Debug.Log("こんにちは、CreateTestLevelState()");

        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL1", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL2", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL3", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL4", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL5", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL6", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL7", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL8", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL9", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL11", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL12", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL13", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL14", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL15", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL16", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL17", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL18", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL19", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL20", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL21", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL22", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL23", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL24", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL25", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL26", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL27", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL28", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL29", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL30", false));

        //GameDataEditor.Instance.data.levels[0].score = 2;
        //GameDataEditor.Instance.data.levels[1].score = 1;
    }
    */
       


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