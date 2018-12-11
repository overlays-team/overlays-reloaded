using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour
{
    public IngameUI ingameUI;
    public int score;

    private bool win;
    private bool lose;
    float timeRemaing = 5.0f;

    //sh
    private bool timeRunsOut;

    public ImageOutput[] outputImages; //holds a collection of all output Images

    // Use this for initialization
    void Start()
    {
        win = false;
        lose = false;
        Resume();
        ingameUI.HideLevelCompletePanel();
        ingameUI.HideGameOverPanel();
        //int sceneNo = GameDataEditor.Instance.data.levels.Count;

        //sh, for testing. generate score randomlly.
        score = Random.Range(1, 4);
    }



    // Update is called once per frame
    void Update()
    {    
        CountTime();
        CheckIfWeWon();

        //sh, for debug, total score
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Win();
        }
       
    }


    void Win()
    {
        ingameUI.ShowLevelCompletePanel(score);
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
        ingameUI.TogglePause();
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        ingameUI.TogglePlay();
        Time.timeScale = 1f;
    }


    private void CountTime()
    {
        if (!win)
        {
            timeRemaing -= Time.deltaTime;
        }
        timeRunsOut = timeRemaing < 0;

        Debug.Log(timeRemaing);

        ingameUI.ShowCountDown(timeRemaing, timeRunsOut);
    }

    /*
    private void UpdateTimeRunsOut()
    {
        timeRunsOut = timeRemaing < 0;
    }
    */


    public bool CheckIfWeWon()
    {
        bool allCorrect = true; //sh: false as default is better and remove else?. 

        //sh
        if (timeRunsOut) {

            // Lose() works here but not in IEnumerator WinCoroutine()
            Lose();
            return false; 
        }


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

        /*
        if (CheckIfWeWon()){
            Win();
        } else {
            Lose();
        }
        */
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