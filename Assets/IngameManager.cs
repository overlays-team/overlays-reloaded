using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour
{
    public IngameUI ingameUI;
    public int score;

    //sh
    private int totalScore;
    private bool timeRunsOut;


    private bool win;
    private bool lose;
    float timeRemaing = 5.0f;


    public ImageOutput[] outputImages; //holds a collection of all output Images

    // Use this for initialization
    void Start()
    {
        win = false;
        lose = false;
        Resume();
        ingameUI.HideLevelCompletePanel();
        ingameUI.HideGameOverPanel();


        score = Random.Range(1, 4); //sh, for testing. generate score randomlly.
        CreateTestLevelState(); //sh, needed for test

        LoadLevelState();
    }



    // Update is called once per frame
    void Update()
    {
        CountTime();
        CheckIfWeWon();

        //sh, for debug, total score
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Win();
        }

    }

    //sh
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
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL10", false));

        GameDataEditor.Instance.data.levels[0].score = 2;
        GameDataEditor.Instance.data.levels[1].score = 1;
    }



    //sh
    private void LoadLevelState() //not only used for getting total score
    {
        int numberOfLevelsInGameData = GameDataEditor.Instance.data.levels.Count;

        for (int i = 0; i < numberOfLevelsInGameData; i++)
        {
            if (GameDataEditor.Instance.data.levels[i].completed)
            {
                totalScore += GameDataEditor.Instance.data.levels[i].score;
            }
        }
    }


    void Win()
    {
        ingameUI.ShowLevelCompletePanel(score);
        win = true;

        //sh
        SaveLevelState();
    }


    //sh
    private void SaveLevelStateAAA()
    {
        LevelData levelData = new LevelData();
        levelData.score = this.score;
        levelData.completed = this.win;
        GameDataEditor.Instance.data.levels.Add(levelData);
    }


    private void SaveLevelStateBBB()
    {
        int numberOfLevelsInGameData = GameDataEditor.Instance.data.levels.Count;
        LevelData lastLevelState = GameDataEditor.Instance.data.levels[numberOfLevelsInGameData-1];
    }



    //sh
    private void SaveLevelState()
    {
        int numberOfLevelsInGameData = GameDataEditor.Instance.data.levels.Count;
        //Debug.Log("numberOfLevelsInGameData: " + numberOfLevelsInGameData);

        int lastCompletedLevel = 0;
        int currentLevel = 0;

        //get last completed level 
        for (int i = 0; i < numberOfLevelsInGameData; i++)
        {
            if (GameDataEditor.Instance.data.levels[i].completed)
            {
                lastCompletedLevel = i;
            }
        }

        //get currentLevel
        if ((lastCompletedLevel == 0) && (numberOfLevelsInGameData == 0)) //for THE first time
        {
            currentLevel = 0; //not necessarily but for security
        }
        else 
        { 
          currentLevel = lastCompletedLevel + 1;
        }


        //save score and win/lose state
        GameDataEditor.Instance.data.levels[currentLevel].score = this.score;
        GameDataEditor.Instance.data.levels[currentLevel].completed = this.win;

        /*
        //if the current level doesn't exists in array, create a new and save
        if (currentLevel > numberOfLevelsInGameData) { 
            LevelData levelData = new LevelData();
            levelData.score = this.score;
            levelData.completed = this.win;
            GameDataEditor.Instance.data.levels.Add(levelData);

        } else { // if it exists, update it.
            GameDataEditor.Instance.data.levels[currentLevel].score = this.score;
            GameDataEditor.Instance.data.levels[currentLevel].completed = this.win;
        }
        */
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
        ingameUI.TogglePause();
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        ingameUI.TogglePlay();
        Time.timeScale = 1f;
    }


    //sh
    private void CountTime()
    {
        if (!win)
        {
            timeRemaing -= Time.deltaTime;
        }
        timeRunsOut = timeRemaing < 0;

       // Debug.Log(timeRemaing);

        ingameUI.ShowCountDown(timeRemaing, timeRunsOut);
    }

    /*
    private void UpdateTimeRunsOut()
    {
        timeRunsOut = timeRemaing < 0;
    }
    */


    public bool CheckIfWeWon()
    {
        bool allCorrect = true; //sh: false as default is better and remove else?. 

        //sh
        if (timeRunsOut)
        {

            // Lose() works here but not in IEnumerator WinCoroutine()
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