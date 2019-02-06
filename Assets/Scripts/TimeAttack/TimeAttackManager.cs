﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeAttackManager : MonoBehaviour
{
    public enum TimeAttackState
    {
        Playing, GameOver, GameComplete, Paused, Countdown, GameBegin, Review
    }

    public TimeAttackState currentState;
    public static TimeAttackManager Instance;
    public TimeAttackUI timeAttackUI;
    public LevelInstantiator levelInstantiator;
    public HttpCommunicator httpCommunicator;

    public int scoreMultiplier;
    public int totalScore;
    public float maxTime;
    public float timer;

    public List<ImageOutput> outputImages = new List<ImageOutput>(); //holds a collection of all output Images

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
        SceneFader.Instance.FadeToClear();
        timeAttackUI.HideLevelCompletePanel();
        timeAttackUI.HideGameOverPanel();
        timeAttackUI.HidePauseButton();
        timeAttackUI.HideTimerDisplay();
        timeAttackUI.HideInventory();
        ResetTimer();
        currentState = TimeAttackState.GameBegin;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case TimeAttackState.Playing:
                CountTime();
                CheckWinCondition();
                break;
            case TimeAttackState.Paused:
                CountTime();
                break;
        }
    }

    public void SetOutputImages(List<ImageOutput> outputImages)
    {
        this.outputImages = outputImages;
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
        string playerName = timeAttackUI.nameInputField.text;
        Debug.Log("playerName: " + playerName);
        bool hasDirtyWord = CheckDirtyWord(playerName);
        Debug.Log("hasDirtyWord: " + hasDirtyWord);

        if (string.IsNullOrEmpty(playerName))
        {
            timeAttackUI.scoreSubmitText.text = "Please enter your name!";
        } 
        else if (hasDirtyWord)
        {
            timeAttackUI.scoreSubmitText.text = "Please try different name!";
        } 
        else
        {
            httpCommunicator.SendScoreToServer(playerName, totalScore);
            GameDataEditor.Instance.data.highestTotalScorePlayerName = playerName;
            timeAttackUI.ShowSubmitCompleteMessage();
        }
    }


    private bool CheckDirtyWord(string playerName)
    {
        bool isDirty = false;
        foreach (string dirtyWord in GameDataEditor.Instance.dirtyWords.wordsList)
        {
            //if (dirtyWord.ToUpper().Contains(playerName.ToUpper())) //  this is meaningless. ie. fuck contains fuckkk -> false
            if (playerName.ToUpper().Contains(dirtyWord.ToUpper()))
            {
                isDirty = true;
                break;
            }
        }
        return isDirty;
    }


    private void CheckHighestTotalScore()
    {
        if (GameDataEditor.Instance.data.highestTotalScore < totalScore)
        {
            SaveHighestTotalScore();
        }
    }

    private void SaveHighestTotalScore()
    {
        GameDataEditor.Instance.data.highestTotalScore = totalScore;
        GameDataEditor.Instance.data.highestTotalScorePlayerName = GameDataEditor.Instance.data.playerName;
    }

    public void StartGame()
    {
        timeAttackUI.HideStartPanel();
        StartCountdown();
    }

    void Win()
    {
        //thisLevelScore = starRating * scoreFactor;
        //starRating = 3 - (moves * 3 / maxMoves);
        totalScore += (int) Mathf.Round(timer * scoreMultiplier);

        CheckHighestTotalScore();
        timeAttackUI.ShowLevelCompletePanel(timer, totalScore, GameDataEditor.Instance.data.highestTotalScore);
        PauseGame();

        currentState = TimeAttackState.GameComplete;
    }

    void Lose()
    {
        currentState = TimeAttackState.GameOver;
        timeAttackUI.ShowGameOverPanel();
        timeAttackUI.scoreCountText.text = totalScore.ToString();

        //sh, Name Input Panel wiil be shown only when (newTotalScore > 0)
        if (totalScore > 0)
        {
            timeAttackUI.ShowNameInputPanel();
        }
        else
        {
            timeAttackUI.scoreSubmitText.text = "Score is too low to submit!";
            timeAttackUI.HideNameInputPanel();
        }
    }
     
    public void NextRandomLevel()
    {

        levelInstantiator.GenerateLevelByDifficulty(totalScore);

        StartCountdown();
        timeAttackUI.HideLevelCompletePanel();
        timeAttackUI.HideLevelRevisitPanel();
        ResetTimer();
        ResumeGame();
    }

    void StartCountdown()
    {
        currentState = TimeAttackState.Countdown;
        StartCoroutine(CountdownCallback(3));
    }

    IEnumerator CountdownCallback(float animDuration)
    {
        timeAttackUI.HideTimerDisplay();
        timeAttackUI.HidePauseButton();
        timeAttackUI.HideTutorialButton();
        timeAttackUI.HideInventory();
        timeAttackUI.ShowCountdownDisplay();
        yield return new WaitForSeconds(animDuration);
        timeAttackUI.HideCountdownDisplay();
        timeAttackUI.ShowTimerDisplay();
        timeAttackUI.ShowTutorialButton();
        timeAttackUI.ShowPauseButton();
        timeAttackUI.ShowInventory();
        currentState = TimeAttackState.Playing;
    }

    public void RevisitLevelAfterWin()
    {
        currentState = TimeAttackState.Review;
        //we enabe the playerController so we can magnify our images
        PlayerController.Instance.Reset();
        PlayerController.Instance.enabled = true;
        //but we set all blockObject so stationary and not moveable so we cant move them anymore
        GameObject[] blockObjects = GameObject.FindGameObjectsWithTag("blockObject");
        foreach (GameObject go in blockObjects)
        {
            BlockObject bo = go.GetComponent<BlockObject>();
            bo.stationary = true;
            bo.actionBlocked = true;
        }

        timeAttackUI.HideLevelCompletePanel();
        timeAttackUI.ShowLevelRevisitPanel();
    }

    public void BackToEndScreen()
    {
        currentState = TimeAttackState.GameComplete;
        PlayerController.Instance.enabled = false;
        timeAttackUI.ShowLevelCompletePanel();
        timeAttackUI.HideLevelRevisitPanel();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneFader.Instance.FadeTo("MainMenu");
    }
    public void Pause()
    { 
        timeAttackUI.TogglePause();
        PauseGame();
    }

    //pauses the inventory and layerController
    public void PauseGame()
    {
        currentState = TimeAttackState.Paused;
        PlayerController.Instance.enabled = false;
        PlayerController.Instance.inventory.enabled = false;
    }

    public void Resume()
    {
        timeAttackUI.TogglePlay();
        ResumeGame();
    }

    public void ResumeGame()
    {
        currentState = TimeAttackState.Playing;
        PlayerController.Instance.Reset();
        PlayerController.Instance.enabled = true;
        PlayerController.Instance.inventory.enabled = true;
    }

    private void ResetTimer()
    {
        timer = maxTime;
    }
   
    private void CountTime()
    {
        timer -= Time.deltaTime;
        timeAttackUI.UpdateCountDown(timer, maxTime);
    }

    public bool CheckWinCondition()
    {  
        if (timer < 0)
        {
            Lose();
            return false;
        }

        bool allCorrect = true;
        if (outputImages.Count > 0)
        {
            //Debug.Log("-------------ot this frame ----------------------------");

            foreach (ImageOutput imageOutput in outputImages)
            {
                if (!imageOutput.imageCorrect) allCorrect = false;
                //Debug.Log("outputImage: " + imageOutput);
            }
            //Debug.Log("correct?e: " + allCorrect);
            if (allCorrect)
            {
                Win();
            } 
            //weil manchmal ein laser nur für eine Sekunde richtig ist, warten wir eine Sekunde
        }
        else
        {
            allCorrect = false;
        }

        return allCorrect;
    }

    public void RaiseMoves()
    {
        //Leave this so the PlayerController doesn't complain
    }
}