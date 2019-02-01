using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour
{
    public enum NormalModeState
    {
        Playing, GameComplete, Paused
    }
    public NormalModeState currentState;

    public IngameUI ingameUI;

    public int score;
    public int moves = 0;
    public int maxMoves = 15;

    public  List<ImageOutput> outputImages = new List<ImageOutput>(); //holds a collection of all output Images

    public void SetOutputImages(List<ImageOutput> outputImages)
    { 
        this.outputImages = outputImages;
    }
    public static IngameManager Instance;

    private Scene currentScene;
    private bool endOfLevel = false;
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

        //get out ImageOutputs
        GameObject[] imageOutputGO = GameObject.FindGameObjectsWithTag("blockObject");
        foreach (GameObject go in imageOutputGO)
        {
            if (go.GetComponent<ImageOutput>() != null)
            {
                outputImages.Add(go.GetComponent<ImageOutput>());
            }
        }
        currentState = NormalModeState.Playing;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case NormalModeState.Playing:
                CheckWinCondition();
                break;
            case NormalModeState.Paused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Resume();
                }
                break;
        }
    }

    void Win()
    {
        score = 3 - (moves * 3 / maxMoves);
        if(score <= 0)
        {
            score = 1;
        }
        SaveLevelState();
        ingameUI.ShowLevelCompletePanel(score, endOfLevel);
        currentState = NormalModeState.GameComplete;
    }

    private void SaveLevelState()
    {
        int numberOfLevelsInGameData = GameDataEditor.Instance.data.levels.Count;
        for (int i = 0; i < numberOfLevelsInGameData; i++)
        {
            if (SceneManager.GetActiveScene().name.Equals(GameDataEditor.Instance.data.levels[i].sceneID))
            {
                if (GameDataEditor.Instance.data.levels[i].score<score)
                {
                    GameDataEditor.Instance.data.levels[i].score = score;
                }
                GameDataEditor.Instance.data.levels[i].completed = true;
                if (i == numberOfLevelsInGameData-1)
                {
                    endOfLevel = true;
                }
                else
                {
                    GameDataEditor.Instance.data.levels[i + 1].completed = true;
                }
            }
        }
        GameDataEditor.Instance.SaveData();
    }

    public void Next()
    {
        SceneFader.Instance.FadeToNextScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneFader.Instance.FadeTo("MainMenu");
    }

    public void LevelSelect()
    {
        SceneFader.Instance.FadeTo("MainMenuLevelSelect");
    }


    public void Pause()
    {
        currentState = NormalModeState.Paused;
        ingameUI.TogglePause();
        PauseGame();
    }

    //pauses the inventory and layerController
    public void PauseGame()
    {
        PlayerController.Instance.enabled = false;
        ingameUI.HideIngameUI();
    }

    public void Resume()
    {
        currentState = NormalModeState.Playing;
        ingameUI.TogglePlay();
        ResumeGame();
    }

    public void ResumeGame()
    {
        PlayerController.Instance.Reset();
        PlayerController.Instance.enabled = true;
        ingameUI.ShowIngameUI();
    }

    public void RevisitLevelAfterWin()
    {
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

        ingameUI.HideLevelCompletePanelForRevisit();
        ingameUI.ShowLevelRevisitPanel();
    }

    public bool CheckWinCondition()
    {
        bool allCorrect = true; 

        if (outputImages.Count> 0)
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

    public void RaiseMoves()
    {
        moves++;
    }

    IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(1);
        if (CheckWinCondition()) Win();
    }
}