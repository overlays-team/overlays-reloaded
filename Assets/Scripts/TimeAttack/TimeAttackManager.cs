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

    public void GoToLeaderboard()
    {
        Application.OpenURL("https://overlays-webapp.herokuapp.com/scores/index.html");
    }


    //submit score to server
    public void SubmitScore()
    {
        string playerName = timeAttackUI.nameInputField.text;
        Debug.Log("playerName: " + playerName);


        if (string.IsNullOrEmpty(playerName))
        {
            timeAttackUI.scoreSubmitText.text = "Please enter your name!";
        }
        else if (!IsNameRuleConformed(playerName))
        {
            timeAttackUI.scoreSubmitText.text = "Either only alphabet or alphabet followed by numbers";
        }
        else if (CheckDirtyWord(playerName)) // normal search
        {
            timeAttackUI.scoreSubmitText.text = "Please try different name!";
        }
        else if (CheckDirtyWord(GetRegExPattern(RemoveSpecialCharactersAndNumbers(playerName)))) //strict search
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


    //remove special characters and numbers for dirty words check
    private string RemoveSpecialCharactersAndNumbers(string s)
    {
        Regex rx = new Regex(@"[^A-Za-zöäüÖÄÜß]");
        string replacedString = rx.Replace(s, "");

        Debug.Log(replacedString);
        return replacedString;
    }


    //check if player name conforms the rule. e.g. shuya777
    private bool IsNameRuleConformed(string s)
    {
        //^ and $ are needed to check beginning and ending of word
        Regex rx = new Regex(@"^[a-zA-Zöäüß]+[\d]*$", RegexOptions.IgnoreCase);
        bool result = rx.IsMatch(s);
        return result;
    }


    //create regular exression pattern. please also refer to project documentation
    private Regex GetRegExPattern(string s)
    {
        //replace repeated character to one plus +
        Regex rx1 = new Regex(@"(\w)\1+", RegexOptions.IgnoreCase);
        string pattern = rx1.Replace(s, "$1+");
        Debug.Log(pattern);

        //replace c, k, ck with (k|ck?)+
        Regex rx2 = new Regex(@"(c\+|c|k\+|k)", RegexOptions.IgnoreCase);
        pattern = rx2.Replace(pattern, "(k|ck?)+");
        Debug.Log(pattern);

        //replace sch,sh with sc?h
        Regex rx3 = new Regex(@"sc?h", RegexOptions.IgnoreCase); // equal as "sh|sch"
        pattern = rx3.Replace(pattern, "sc?h");
        Debug.Log(pattern);

        //replace f|s|t|n|l|m|p, which not followed by "+" with $1+(one of them plus +).
        Regex rx4 = new Regex(@"(f|s|t|n|l|m|p)(?!\+)", RegexOptions.IgnoreCase);
        pattern = rx4.Replace(pattern, "$1+");
        Debug.Log(pattern);

        return new Regex("^" + pattern + "$", RegexOptions.IgnoreCase);
    }


    //check word with dictionary
    private bool CheckDirtyWord(string s)
    {
        bool isDirty = false;
        foreach (string dirtyWord in GameDataEditor.Instance.dirtyWords.wordsList)
        {
            if (s.ToUpper().Contains(dirtyWord.ToUpper()))
            {
                isDirty = true;
                break;
            }
        }
        return isDirty;
    }


    //check word with dictionary
    private bool CheckDirtyWord(Regex rx)
    {
        //time measurement start
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Reset();
        sw.Start();

        bool isDirty = false;
        int index = 0;
        int size = GameDataEditor.Instance.dirtyWords.wordsList.Count;
        while (!isDirty)
        {
            if (rx.IsMatch(GameDataEditor.Instance.dirtyWords.wordsList[index]))
            {
                isDirty = true;
                break;
            }
            index++;
            if (index == size) break; 
        }

        //print position in dictionary
        if (isDirty)
        {
            Debug.Log("index:" + index + " , " + "match:" + GameDataEditor.Instance.dirtyWords.wordsList[index]);
        }

        //time measurement end
        sw.Stop();
        Debug.Log("search time:" + sw.ElapsedMilliseconds + "ms");

        return isDirty;
    }



    //if player achieved the highest score, it is saved.
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
        // Remember the old score value to animate the score refill from the time left
        int oldScore = totalScore;
        totalScore += (int) Mathf.Round(timer) * scoreMultiplier;

        CheckHighestTotalScore();
        timeAttackUI.ShowLevelCompletePanel(timer, totalScore, oldScore);
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