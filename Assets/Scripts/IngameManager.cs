﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour
{
    public IngameUI ingameUI;
    public SceneFader fader;

    [SerializeField]
    private bool attackMode;

    //sh
    public int star;
    private int scoreFactor = 10;
    private int thisLevelScore;
    private int previousTotalScore;
    private int newTotalScore;
    public HttpCommunicator httpCommunicator;
    public LevelInstantiator levelInstantiator;

    //private string playerName;
    //private int highestTotalScore;
    //private string highestTotalScorePlayerName;

    private bool timeRunsOut;
    private bool win;
    private bool lose;
    public float initCountdownTime = 30.0f;
    public float countdownTimer = 30.0f;
    bool paused;


    List<ImageOutput> outputImages = new List<ImageOutput>(); //holds a collection of all output Images

    public static IngameManager Instance;

    //Singletoncode
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance); // es kann passieren wenn wir eine neue Scene laden dass immer noch eine Instanz existiert
        }
        else
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        fader.FadeToClear();
        win = false;
        lose = false;
        ingameUI.HideLevelCompletePanel();
        ingameUI.HideGameOverPanel();

        SetTestParameters();

        LoadLevelState();

        FindCurrentGoalBlocks();
    }

    private void SetTestParameters()
    {
        SetAttackMode(attackMode); //sh, for testing

        star = Random.Range(1, 4); //sh, for testing. generate star randomlly.

        // needed for test
        //CreateTestLevelState(); //sh, 

        GameDataEditor.Instance.data.highestTotalScore = 177;//sh, for testing
        GameDataEditor.Instance.data.playerName = "Player"; //sh. for testing
        if(attackMode)ingameUI.ShowCountDownText(attackMode);
        if (attackMode) ingameUI.ShowTotalScorePanel(attackMode);
        if (attackMode)ingameUI.ShowHighestScorePanel(attackMode);
    }


    // Update is called once per frame
    void Update()
    {
        FindCurrentGoalBlocks();

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

    private void FindCurrentGoalBlocks()
    {
        GameObject[] imageOutputGO = GameObject.FindGameObjectsWithTag("blockObject");
        foreach (GameObject go in imageOutputGO)
        {
            if (go.GetComponent<ImageOutput>() != null && !outputImages.Contains(go.GetComponent<ImageOutput>()))
            {
                outputImages.Add(go.GetComponent<ImageOutput>());
            }
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
        //StopCoroutine("WinCoroutine");
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
        string year = System.DateTime.Now.Year.ToString();
        string month = System.DateTime.Now.Month.ToString();
        string day = System.DateTime.Now.Day.ToString();

        int random = Random.Range(1000, 9999);

        return ("player" + "-" + random);
    }


    public void SubmitScore()
    {
        string playerName = "";

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
        GameDataEditor.Instance.data.levels[currentLevel].score = thisLevelScore;
        GameDataEditor.Instance.data.levels[currentLevel].completed = win;

        GameDataEditor.Instance.data.levels[nextLevel].completed = true;
    }

    public void SetOutputImages(List<ImageOutput> outputs)
    {
        outputImages = outputs;
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
        if(attackMode)
        {
            levelInstantiator.InstantiateRandomLevel();
            ingameUI.HideLevelCompletePanel();
            win = false;
            countdownTimer = initCountdownTime;
            FindCurrentGoalBlocks();
        }
        else
        {
            fader.FadeToNextScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
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
        PauseGame();
    }

    //pauses the inventory and layerController
    public void PauseGame()
    {
        PlayerController.Instance.enabled = false;
        PlayerController.Instance.inventory.enabled = false;
    }

    public void Resume()
    {
        paused = false;
        ingameUI.TogglePlay();
        Time.timeScale = 1f;
        ResumeGame();
    }

    public void ResumeGame()
    {
        PlayerController.Instance.Reset();
        PlayerController.Instance.enabled = true;
        PlayerController.Instance.inventory.enabled = true;
    }


    private void CountTime()
    {
        if (!win && attackMode)
        {
            countdownTimer -= Time.deltaTime;
        }
        timeRunsOut = countdownTimer < 0;

        if(attackMode)ingameUI.UpdateCountDown(countdownTimer, timeRunsOut);
    }


    public bool CheckIfWeWon()
    {
        bool allCorrect = true; 

        if (timeRunsOut &!lose)
        {
            Lose();
            return false;
        }


        if (outputImages.Count > 0)
        {
            foreach (ImageOutput imageOutput in outputImages)
            {
                if (!imageOutput.imageCorrect) allCorrect = false;
            }

            if (allCorrect) Win();
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


}