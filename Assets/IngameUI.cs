using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour {
    public GameObject levelCompleteMenu;
    public Image myImageComponent;
    public GameObject gameOverMenu;
    public Sprite star1;
    public Sprite star2;
    public Sprite star3;
    public GameObject pauseButton;
    public GameObject playButton;
    public GameObject pauseMenuPanel;
    public GameObject pausePlayButton;
    private string[] star3Texts = new string[] { "You did it!", "You rock!", "Awesome!" };
    private string[] star2Texts = new string[] { "Not bad!", "Good!", "Good job!" };
    private string[] star1Texts = new string[] { "Could be better!", "Don't give up!", "Lucky!" };
    public Text levelCompleteText;

    //sh
    public Text countDownText;
    public GameObject countDownPanel;
    public GameObject highestScorePanel;
    public Text totalScoreText;
    public Text highestScoreText;

    public Material blurMaterial;
    public bool blurring = false;
    float timeBlur = 0f;
    float newValue = 0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        blurEffect();
    }

    private void blurEffect()
    {
        if (blurring)
        {
            //duration of blur motion
            timeBlur += Time.deltaTime;
            newValue += 0.01f;
            //Wie stark die Veränderung
            float ratio = 7f;
            //Color Motion
            Color newColor = new Color(1 - newValue * ratio, 1 - newValue * ratio, 1 - newValue * ratio, 1);
            //Blur motion
            blurMaterial.SetColor("_Color", newColor);
            blurMaterial.SetFloat("_Size", newValue * 50.0f);

            Debug.Log("timeBlur: " + timeBlur + " New Value:  " + newValue);
            if (timeBlur > 0.3f)
            {
                blurring = false;
                timeBlur = 0;
                newValue = 0;
            }
        }
    }

    public void ShowLevelCompletePanel(int star, int totalScore, int highestTotalScore)
    {
    
        Debug.Log("totalScore: " + totalScore);
        //levelCompleteText = levelCompleteMenu.transform.Find("LevelCompletedText").gameObject.GetComponent<Text>();
        //myImageComponent = levelCompleteMenu.transform.Find("StarReceivementImage").gameObject.GetComponent<Image>();

        //sh
        //totalScoreText.text = "YOUR SCORE: " + totalScore;
        totalScoreText.text = GameDataEditor.Instance.data.playerName +"'s SCORE: " + totalScore;

        highestScoreText.text = "HIGHEST SCORE: " + highestTotalScore;


        switch (star)
        {
            case 1:
                levelCompleteText.text = star1Texts[Random.Range(0, 3)]; 
                myImageComponent.sprite = star1;
                break;
            case 2:
                levelCompleteText.text = star2Texts[Random.Range(0, 3)];
                myImageComponent.sprite = star2;
                break;
            case 3:
                levelCompleteText.text = star3Texts[Random.Range(0, 3)];
                myImageComponent.sprite = star3;
                break;
        }

        levelCompleteMenu.SetActive(true);
        pausePlayButton.SetActive(false);
        pauseMenuPanel.SetActive(false);
        blurring = true;

    }

    public void TogglePause()
    {
        pauseButton.SetActive(false);
        //PlayButton.SetActive(true);
        pauseMenuPanel.SetActive(true);
        blurring = true;
    }
    public void TogglePlay()
    {
        pauseButton.SetActive(true);
        //PlayButton.SetActive(false);
        pauseMenuPanel.SetActive(false);

    }
    public void ShowGameOverPanel()
    {
        gameOverMenu.SetActive(true);
        pausePlayButton.SetActive(false);
        blurring = true;
    }
    public void HideLevelCompletePanel()
    {
        levelCompleteMenu.SetActive(false);
        pausePlayButton.SetActive(true);

    }
    public void HideGameOverPanel()
    {
        gameOverMenu.SetActive(false);
        pausePlayButton.SetActive(true);
    }

    //sh
    public void ShowCountDownText(bool isEnabled)
    {
        countDownPanel.SetActive(isEnabled);
        countDownText.enabled = isEnabled;
    }

    public void ShowHighestScorePanel(bool isEnabled)
    {
        highestScorePanel.SetActive(isEnabled);
        highestScoreText.enabled = isEnabled;
    }


    //sh
    public void UpdateCountDown(float timeRemaining, bool win)
    {
        //Debug.Log("ShowCountDown()");

        countDownText.text = "TIME REMAINING: " + Mathf.Round(timeRemaining) + "s";
        if (win)
        {
            countDownText.text = "";
        }
    }


}
