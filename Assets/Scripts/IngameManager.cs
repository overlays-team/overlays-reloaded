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
    public SceneFader fader;

    public int star;
    public int moves = 0;
    public int maxMoves = 15;

    private Scene currentScene;

    public  List<ImageOutput> outputImages = new List<ImageOutput>(); //holds a collection of all output Images

    public void setOutputImages(List<ImageOutput> outputImages)
    {

        this.outputImages = outputImages;
    }

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
        ingameUI.HideLevelCompletePanel();
        ingameUI.HideGameOverPanel();

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
                CheckIfWeWon();
                break;
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


    /*
    private void LoadLevelState() //now only used for getting total score for testing
    {
        int numberOfLevelsInGameData = GameDataEditor.Instance.data.levels.Count;

        for (int i = 0; i < numberOfLevelsInGameData; i++)
        {
            if (GameDataEditor.Instance.data.levels[i].completed)
            {
                previousTotalScore += GameDataEditor.Instance.data.levels[i].score;
            }
        }
    }
    */

    void Win()
    {
        star = 3 - (moves*3 / maxMoves);
        ingameUI.ShowLevelCompletePanel(star);
        SaveLevelState();
        currentState = NormalModeState.GameComplete;
    }

    private void SaveLevelState()
    {
        int numberOfLevelsInGameData = GameDataEditor.Instance.data.levels.Count;
        for (int i = 0; i < numberOfLevelsInGameData; i++)
        {
            if (SceneManager.GetActiveScene().name.Equals(GameDataEditor.Instance.data.levels[i].sceneID))
            {
                GameDataEditor.Instance.data.levels[i].score = star;
                GameDataEditor.Instance.data.levels[i].completed = true;
                GameDataEditor.Instance.data.levels[i].completed = true;
            }
        }


        /*
        GameDataEditor.Instance.data.levels[currentLevel].score = star;
        GameDataEditor.Instance.data.levels[currentLevel].completed = true;
        GameDataEditor.Instance.data.levels[nextLevel].completed = true;
        */
    }

    void Lose()
    {
        ingameUI.ShowGameOverPanel();
    }

    public void Next()
    {
        fader.FadeToNextScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
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
        Time.timeScale = 1f;
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

    public bool CheckIfWeWon()
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
        print("your moves: " +moves);
    }

    IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(1);
        if (CheckIfWeWon()) Win();
    }


}