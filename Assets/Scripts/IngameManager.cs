using System.Collections;
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
    public HttpCommunicator httpCommunicator;

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

        star = Random.Range(1, 4); //sh, for testing. generate star randomlly.

        // needed for test
        CreateTestLevelState(); //sh, 


        GameDataEditor.Instance.data.highestTotalScore = 177;//sh, for testing
        GameDataEditor.Instance.data.playerName = "Player"; //sh. for testing
        if(attackMode)ingameUI.ShowCountDownText(attackMode);
        if (attackMode) ingameUI.ShowTotalScorePanel(attackMode);
        if (attackMode)ingameUI.ShowHighestScorePanel(attackMode);
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Lose();
        }




    }



    private void LoadLevelState() //now only used for getting total score for testing
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

    private string GetRandomPlayerName()
    {
        //string datetimeStr = System.DateTime.Now.ToString();
        //Debug.Log("time:" + datetimeStr);

        string year = System.DateTime.Now.Year.ToString();
        string month = System.DateTime.Now.Month.ToString();
        string day = System.DateTime.Now.Day.ToString();

        int random = Random.Range(1000, 9999);

        //return ("player" + year + month + day + "-" + random);
        return ("player" + "-" + random);
    }


    public void SubmitScore()
    {
        string playerName = "";

        //random player name
        //playerName = GetRandomPlayerName();

        if (ingameUI.nameInputField.text.Equals(""))
        {

            ingameUI.ShowMessageDialogPanel("Please enter your name!", "return");
        }
        else
        {
            //TODO: doent't work if "c" is being inputed in inputTextField. 
            playerName = ingameUI.nameInputField.text;
            httpCommunicator.SendScoreToServer(playerName, newTotalScore);
            GameDataEditor.Instance.data.highestTotalScorePlayerName = playerName;

            Debug.Log(playerName);
            ingameUI.ShowSubmitCompleteMessage();
        }
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
    }



    void Lose()
    {
        ingameUI.ShowGameOverPanel();
        lose = true;

        //sh, Name Input Panel wiil be shown only when (newTotalScore > 0)
        if (newTotalScore > 0)
        {
            ingameUI.ShowNameInputPanel();
        }
        else
        {
            ingameUI.HideNameInputPanel();
        }
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
    }
    public void Resume()
    {
        paused = false;
        ingameUI.TogglePlay();
        Time.timeScale = 1f;
    }


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
    //needed for testing with InGameUI Scene
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

}