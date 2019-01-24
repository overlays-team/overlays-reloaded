using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeAttackManager : MonoBehaviour
{

    public enum TimeAttackState
    {
        Playing, GameOver, GameComplete, Paused
    }

    public TimeAttackState currentState;
    public static TimeAttackManager Instance;
    public TimeAttackUI timeAttackUI;
    public SceneFader fader;
    public LevelInstantiator levelInstantiator;
    public HttpCommunicator httpCommunicator;

    public int starRating;
    public int scoreFactor = 10;
    public int thisLevelScore;
    public int previousTotalScore;
    public int newTotalScore;

    public int moves = 0;
    public int maxMoves = 15;
    
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
        fader.FadeToClear();
        timeAttackUI.HideLevelCompletePanel();
        timeAttackUI.HideGameOverPanel();

        timer = maxTime;

        currentState = TimeAttackState.Playing;
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
            httpCommunicator.SendScoreToServer(playerName, newTotalScore);
            GameDataEditor.Instance.data.highestTotalScorePlayerName = playerName;

            Debug.Log(playerName);
            timeAttackUI.ShowSubmitCompleteMessage();
        }
    }

    private void CheckHighestTotalScore()
    {
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

    void Win()
    {
        //thisLevelScore = starRating * scoreFactor;
        starRating = 3 - (moves * 3 / maxMoves);
        newTotalScore += (int) Mathf.Round(timer);

        CheckHighestTotalScore();
        timeAttackUI.ShowLevelCompletePanel(starRating, newTotalScore, GameDataEditor.Instance.data.highestTotalScore, true);

        currentState = TimeAttackState.GameComplete;
    }

    void Lose()
    {
        currentState = TimeAttackState.GameOver;
        timeAttackUI.ShowGameOverPanel();
        timeAttackUI.scoreCountText.text = newTotalScore.ToString();

        //sh, Name Input Panel wiil be shown only when (newTotalScore > 0)
        if (newTotalScore > 0)
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
        timeAttackUI.HideLevelCompletePanel();
        currentState = TimeAttackState.Playing;
        timer = maxTime;
        ResumeGame();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        fader.FadeTo("MainMenu");
        //SceneManager.LoadScene("MainMenu");
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
        moves++;
        print("your moves: " + moves);
    }

    IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(1);
        if (CheckWinCondition())
        {
            Win();
        }
    }
}