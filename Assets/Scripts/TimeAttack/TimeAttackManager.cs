using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeAttackManager : MonoBehaviour
{
    public enum TimeAttackState
    {
        Playing, GameOver, GameComplete, Paused, Countdown, GameBegin
    }

    public TimeAttackState currentState;
    public static TimeAttackManager Instance;
    public TimeAttackUI timeAttackUI;
    public LevelInstantiator levelInstantiator;
    public HttpCommunicator httpCommunicator;

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

    /*
    public void SubmitScore()
    {
        string playerName = "";

        if (timeAttackUI.nameInputField.text.Equals(""))
        {

            timeAttackUI.scoreSubmitText.text = "Please enter your name!";
        }

        else
        {
            //TODO: doent't work if "c" is being inputed in inputTextField. 
            playerName = timeAttackUI.nameInputField.text;
            httpCommunicator.SendScoreToServer(playerName, totalScore);
            GameDataEditor.Instance.data.highestTotalScorePlayerName = playerName;

            Debug.Log(playerName);
            timeAttackUI.ShowSubmitCompleteMessage();
        }
    }*/


    public void SubmitScore()
    {
        string playerName = timeAttackUI.nameInputField.text;

        //prepare for speed mesurement
        Debug.Log(playerName);
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        //speed measurement
        sw.Start();
        Debug.Log("IsDirtyWordInString(): " + IsDirtyWordInString(playerName));
        sw.Stop();
        Debug.Log("IsDirtyWordInString(): " + sw.ElapsedMilliseconds + "ms");

        //speed measurement
        sw.Start();
        Debug.Log("IsDirtyWordInList(): " + IsDirtyWordInList(playerName));
        sw.Stop();
        Debug.Log("IsDirtyWordInList(): " + sw.ElapsedMilliseconds + "ms");

        //coonvert for testing
        CovertWordsListToWordsHashSet();
        //speed measurement
        sw.Start();
        Debug.Log("IsDirtyWordInHastSet(): " + IsDirtyWordInHastSet(playerName));
        sw.Stop();
        Debug.Log("IsDirtyWordInHastSet(): " + sw.ElapsedMilliseconds + "ms");


        if (string.IsNullOrEmpty(playerName))
        {
            timeAttackUI.scoreSubmitText.text = "Please enter your name!";
        } 
        else if (IsDirtyWord(playerName))
        {
            timeAttackUI.scoreSubmitText.text = "Please try different name!";
        } 
        else
        {
            //TODO: doent't work if "c" is being inputed in inputTextField. 
            playerName = timeAttackUI.nameInputField.text;
            httpCommunicator.SendScoreToServer(playerName, totalScore);
            GameDataEditor.Instance.data.highestTotalScorePlayerName = playerName;

            Debug.Log(playerName);
            timeAttackUI.ShowSubmitCompleteMessage();
        }
    }

    //facade, will be refactored later
    private bool IsDirtyWord(string playerName){
        return IsDirtyWordInList(playerName);
    }

    private bool IsDirtyWordInList(string playerName)
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


    private bool IsDirtyWordInString(string playerName)
    {
        bool isDirty = false;
 
        //this comparison is meaningless. this is always false
        //i.e) playerName=fuckkkk contains abcedffuckabcedf --> false 
        //i.e) playerName=fuck contains abcedffuckabcedf --> false
        //isDirty = playerName.ToUpper().Contains(GameDataEditor.Instance.dirtyWordsEglish.dirtyWords.ToUpper());

        //i.e) abcdefuckabcde contains playerName=fuck -->true
        //i.e) abcdefuckabcde contains playerName=fuckkkk --> false  --> not good!
        isDirty = GameDataEditor.Instance.dirtyWords.words.ToUpper().Contains(playerName.ToUpper());
        return isDirty;
    }


    private void CovertWordsListToWordsHashSet()
    {
        foreach (string word in GameDataEditor.Instance.dirtyWords.wordsList)
        {
            dirtyWordsHashSetTest.Add(word);
        }
    }

    //this is for testing purpose
    HashSet<string> dirtyWordsHashSetTest = new HashSet<string>();
    private bool IsDirtyWordInHastSet(string playerName)
    {
        bool isDirty = false;
        foreach (string dirtyWord in dirtyWordsHashSetTest)
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
        totalScore += (int) Mathf.Round(timer);

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
        levelInstantiator.InstantiateRandomLevel();
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
        timeAttackUI.HideInventory();
        timeAttackUI.ShowCountdownDisplay();
        yield return new WaitForSeconds(animDuration);
        timeAttackUI.HideCountdownDisplay();
        timeAttackUI.ShowTimerDisplay();
        timeAttackUI.ShowPauseButton();
        timeAttackUI.ShowInventory();
        currentState = TimeAttackState.Playing;
    }

    public void RevisitLevelAfterWin()
    {
        //we enabe the playerController so we can magnify our images
        PlayerController.Instance.Reset();
        PlayerController.Instance.enabled = true;
        //but we set all blockObject so stationary and not moveable so we cant move them anymore
        GameObject[] blockObjects = GameObject.FindGameObjectsWithTag("blockObject");
        foreach(GameObject go in blockObjects)
        {
            BlockObject bo = go.GetComponent<BlockObject>();
            bo.stationary = true;
            bo.actionBlocked = true;
        }

        timeAttackUI.HideLevelCompletePanel();
        timeAttackUI.ShowLevelRevisitPanel();
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
        currentState = TimeAttackState.Paused;
        timeAttackUI.TogglePause();
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
        currentState = TimeAttackState.Playing;
        timeAttackUI.TogglePlay();
        ResumeGame();
    }

    public void ResumeGame()
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