using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

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
            DestroyImmediate(Instance);
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
        LockPlayerController(); // So the user can't zoom into blocks during the tutorial
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

    public void GoToLeaderboard()
    {
        Application.OpenURL("https://overlays-webapp.herokuapp.com/scores/index.html");
    }


    //submit score to server
    public void SubmitScore()
    {
        string playerName = timeAttackUI.nameInputField.text;
        Debug.Log("playerName: " + playerName);
        bool hasDirtyWord = CheckDirtyWord(RemoveSpecialCharacters(playerName)) | CheckDirtyWord(RemoveSpecialCharactersAndNumbers(playerName));
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

    //remove special characters for dirty words check
    private string RemoveSpecialCharacters(string s)
    {
        Regex rx = new Regex(@"[^0-9A-Za-zÖÄÜßöäü]");
        string replacedString = rx.Replace(s, "");

        Debug.Log(replacedString);
        return replacedString;
    }

    //remove special characters and numbers for dirty words check
    private string RemoveSpecialCharactersAndNumbers(string s)
    {
        Regex rx = new Regex(@"[^A-Za-zÖÄÜßöäü]");
        string replacedString = rx.Replace(s, "");

        Debug.Log(replacedString);
        return replacedString;
    }

    //check if it is dirty word
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

    //if player achieved the hithest score, it is saved.
    private void CheckHighestTotalScore()
    {
        if (GameDataEditor.Instance.data.highestTotalScore < totalScore)
        {
            SaveHighestTotalScore();
        }
    }

    //sace total score to GameDataEditor singleton
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
        LockPlayerController();

        currentState = TimeAttackState.GameComplete;
    }

    void Lose()
    {
        LockPlayerController();
        currentState = TimeAttackState.GameOver;
        timeAttackUI.HideInventory();
        timeAttackUI.HideTimerDisplay();
        timeAttackUI.ShowGameOverPanel();
        timeAttackUI.scoreCountText.text = totalScore.ToString();

        // Player can only submit sufficient scores (totalScore > 0)
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
        UnlockPlayerController();
    }

    void StartCountdown()
    {
        currentState = TimeAttackState.Countdown;
        StartCoroutine(CountdownCallback(3));
    }

    // Hide all elements except the game start countdown and then show them again when the countdown has ended
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
        UnlockPlayerController();
        currentState = TimeAttackState.Playing;
    }

    public void RevisitLevelAfterWin()
    {
        currentState = TimeAttackState.Review;
        // Enable the playerController so we can magnify our images
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
    }

    public void MainMenu()
    {
        SceneFader.Instance.FadeTo("MainMenu");
    }
    public void Pause()
    { 
        timeAttackUI.TogglePause();
        LockPlayerController();
        currentState = TimeAttackState.Paused;
    }

    // Pauses the inventory and layerController
    public void LockPlayerController()
    {
        PlayerController.Instance.enabled = false;
        PlayerController.Instance.inventory.enabled = false;
    }

    public void Resume()
    {
        timeAttackUI.TogglePlay();
        UnlockPlayerController();
        currentState = TimeAttackState.Playing;
    }

    public void UnlockPlayerController()
    {
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
            foreach (ImageOutput imageOutput in outputImages)
            {
                if (!imageOutput.imageCorrect) allCorrect = false;
            }
            if (allCorrect)
            {
                Win();
            } 
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