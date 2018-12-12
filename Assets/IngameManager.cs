using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour
{
    public IngameUI ingameUI;
    public int wert;
    private bool win;
    private bool lose;
    float timeLeft = 5.0f;
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

    }

    // Update is called once per frame
    void Update()
    {
        //im Spiel
        if (!win & !lose & !paused)
        {
            //wenn blurStand not aktiv und Spiel wurde pausiert
            if (!ingameUI.blurStand & paused)
            {
                Time.timeScale = 0;
            }

            //Countdown
            timeLeft -= Time.deltaTime;
            Debug.Log(timeLeft);

            //wenn lose
            lose |= timeLeft < 0;
            if (lose)
            {
                Lose();

            }

            //wenn win
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Win();

            }
        }

        CheckIfWeWon();
    }

    void Win()
    {
        ingameUI.ShowLevelCompletePanel(wert);
        win = true;
    }
    void Lose()
    {
        ingameUI.ShowGameOverPanel();
        lose = true;
    }

    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
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
        timeLeft += 0.3f;
        paused = true;
        ingameUI.TogglePause();
    }
    public void Resume()
    {
        paused = false;
        ingameUI.TogglePlay();
        Time.timeScale = 1f;
    }

    bool CheckIfWeWon()
    {
        bool allCorrect = true;
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
    }


    /*
    GameData gameData;
    //GoalBlock goalBlock; //uncomment when here IS a goalBlock
    Scene thisScene;

    // Use this for initialization
    void Start () {

        thisScene = SceneManager.GetActiveScene();

    }

    // Update is called once per frame
    void Update () {

        //if (goalBlock.isGoalAchieved) //uncomment when here IS a goalBlock
        {
            List<LevelData> levels = gameData.levels; //why public field and not a getter?
            foreach (LevelData level in levels)
            {
                if (level.sceneID == thisScene.name)
                {
                    level.completed = true; //better rename it to isCompleted 
                }
            }
            SceneManager.LoadScene("LevelComplete"); //or otherwise 
        }

    }
    */
}