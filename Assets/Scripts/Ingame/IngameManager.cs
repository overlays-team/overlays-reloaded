using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour
{
    //Game states
    public enum IngameManagerState
    {
        Playing, GameComplete, Paused, Review
    }
    public IngameManagerState currentState;

    public IngameUI ingameUI;

    public int score;
    public int moves = 0;
    [Tooltip("if this is set to 15 we get 1 star after 15 moves, 2 stars with 5-10 moves and on with 0-5 moves")]
    public int maxMoves = 15;

    public  List<ImageOutput> outputImages = new List<ImageOutput>(); //Holds a collection of all output Images

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
            DestroyImmediate(Instance); // It could happen when we load a new scene that still has an instance
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

        //Get out ImageOutputs
        GameObject[] imageOutputGO = GameObject.FindGameObjectsWithTag("blockObject");
        foreach (GameObject go in imageOutputGO)
        {
            if (go.GetComponent<ImageOutput>() != null)
            {
                outputImages.Add(go.GetComponent<ImageOutput>());
            }
        }
        currentState = IngameManagerState.Playing;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case IngameManagerState.Playing:
                CheckWinCondition();
                break;
            case IngameManagerState.Paused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Resume();
                }
                break;
        }
    }

    //Win the game and calculate score
    void Win()
    {
        score = 3 - (moves * 3 / maxMoves);
        if(score <= 0)
        {
            score = 1;
        }
        SaveLevelState();
        ingameUI.ShowLevelCompletePanel(score, endOfLevel);
        currentState = IngameManagerState.GameComplete;
    }

    //Save score of completed level 
    private void SaveLevelState()
    {
        int numberOfLevelsInGameData = GameDataEditor.Instance.data.levels.Count;
        for (int i = 0; i < numberOfLevelsInGameData; i++)
        {
            if (SceneManager.GetActiveScene().name.Equals(GameDataEditor.Instance.data.levels[i].sceneID))
            {
                //Just the greater score will be saved
                if (GameDataEditor.Instance.data.levels[i].score<score)
                {
                    GameDataEditor.Instance.data.levels[i].score = score;
                }
                GameDataEditor.Instance.data.levels[i].completed = true;
                if (i == numberOfLevelsInGameData-1)
                {
                    endOfLevel = true;
                }
                //unlock next level
                else
                {
                    GameDataEditor.Instance.data.levels[i + 1].completed = true;
                }
            }
        }
        GameDataEditor.Instance.SaveData();
    }

    //Next game (when button clicked)
    public void Next()
    {
        SceneFader.Instance.FadeToNextScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Retry game (when button clicked)
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Go to main menu (when button clicked)
    public void MainMenu()
    {
        SceneFader.Instance.FadeTo("MainMenu");
    }

    //Go to level select scene (when button clicked)
    public void LevelSelect()
    {
        SceneFader.Instance.FadeTo("MainMenuLevelSelect");
    }

    //Pause game (when button clicked)
    public void Pause()
    {
        currentState = IngameManagerState.Paused;
        ingameUI.TogglePause();
        PauseGame();
    }

    //Pause the inventory and layerController
    public void PauseGame()
    {
        PlayerController.Instance.enabled = false;
        ingameUI.HideIngameUI();
    }

    //Resume game (when button clicked)
    public void Resume()
    {
        currentState = IngameManagerState.Playing;
        ingameUI.TogglePlay();
        ResumeGame();
    }

    //Resume the inventory and layerController
    public void ResumeGame()
    {
        PlayerController.Instance.Reset();
        PlayerController.Instance.enabled = true;
        ingameUI.ShowIngameUI();
    }

    //Review state after win (when button clicked)
    public void RevisitLevelAfterWin()
    {
        currentState = IngameManagerState.Review;
        //We enabe the playerController so we can magnify our images
        PlayerController.Instance.Reset();
        PlayerController.Instance.enabled = true;
        //But we set all blockObject so stationary and not moveable so we cant move them anymore
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

    //End review state, back to level complete screen (when button clicked)
    public void BackToEndScreen()
    {
        currentState = IngameManagerState.GameComplete;
        PlayerController.Instance.enabled = false;
        ingameUI.ShowLevelCompletePanel();
        ingameUI.HideLevelRevisitPanel();
    }

    //Check if all picuters correctly targeted
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
            //Because sometimes a laser is only right targeted for a second, we wait a second
        }
        else
        {
            allCorrect = false;
        }
        return allCorrect;
    }

    //Moves counter
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