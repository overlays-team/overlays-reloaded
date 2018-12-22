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

    public Text countDownText;
    public GameObject countDownPanel;
    public GameObject totalScorePanel;
    public GameObject highestScorePanel;
    public Text totalScoreText;
    public Text highestScoreText;
    public GameObject nameInputPanel;
    public Text nameInputPanelText;
    public GameObject nameInputSubPanel;
    public InputField nameInputField;
    public GameObject messageDialogPanel;
    public Text messageDialogText;
    public Text mesageDialogButtonText;

    public Material blurMaterial;

    Material blurMateriaInPauseMenuPanel;
    Material blurMaterialInLevelCompleteMenu;
    Material blurMateriaInGameOverMenu;

    public bool blurring = false;
    float timeBlur = 0f;
    float newValue = 0f;

    // Use this for initialization
    void Start()
    {
        blurMateriaInPauseMenuPanel = Instantiate(pauseMenuPanel.GetComponent<Image>().material);
        blurMaterialInLevelCompleteMenu = Instantiate(levelCompleteMenu.GetComponent<Image>().material);
        blurMateriaInGameOverMenu = Instantiate(gameOverMenu.GetComponent<Image>().material);
    }

    // Update is called once per frame
    void Update()
    {
        blurEffect();
    }

    private void blurEffect()
    {
        //früher (unfixed)
        /*
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
        */

        //fixed methode
        if (blurring)
        {
            //duration of blur motion
            timeBlur += Time.deltaTime;
            newValue += 0.01f;
            //Wie stark die Veränderung
            float ratio = 7f;
            //Color Motion
            Color pausePanelNewColor = new Color(1 - newValue * ratio, 1 - newValue * ratio, 1 - newValue * ratio, 1);
            Color levelCompleteMenuNewColor = new Color(1 - newValue * ratio, 1, 1 - newValue * ratio, 1);
            Color gameOverMenuNewColor = new Color(1, 1 - newValue * ratio, 1 - newValue * ratio, 1);

            //Blur motion
            blurMateriaInPauseMenuPanel.SetColor("_Color", pausePanelNewColor);
            blurMateriaInPauseMenuPanel.SetFloat("_Size", newValue * 50.0f);

            blurMaterialInLevelCompleteMenu.SetColor("_Color", levelCompleteMenuNewColor);
            blurMaterialInLevelCompleteMenu.SetFloat("_Size", newValue * 50.0f);

            blurMateriaInGameOverMenu.SetColor("_Color", gameOverMenuNewColor);
            blurMateriaInGameOverMenu.SetFloat("_Size", newValue * 50.0f);

            pauseMenuPanel.GetComponent<Image>().material = blurMateriaInPauseMenuPanel;
            levelCompleteMenu.GetComponent<Image>().material = blurMaterialInLevelCompleteMenu;
            gameOverMenu.GetComponent<Image>().material = blurMateriaInGameOverMenu;

            //Debug.Log("timeBlur: " + timeBlur + " New Value:  " + newValue);
            if (timeBlur > 0.3f)
            {
                blurring = false;
                timeBlur = 0;
                newValue = 0;
            }
        }
    }

    public void ShowLevelCompletePanel(int star, int totalScore, int highestTotalScore, bool attackMode)
    {
    
        Debug.Log("totalScore: " + totalScore);
        //levelCompleteText = levelCompleteMenu.transform.Find("LevelCompletedText").gameObject.GetComponent<Text>();
        //myImageComponent = levelCompleteMenu.transform.Find("StarReceivementImage").gameObject.GetComponent<Image>();

        //sh
        //totalScoreText.text = "YOUR SCORE: " + totalScore;
        if(attackMode) totalScoreText.text = GameDataEditor.Instance.data.playerName +"'s SCORE: " + totalScore;

        if (attackMode) highestScoreText.text = "HIGHEST SCORE: " + highestTotalScore;


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

    public void ShowNameInputPanel()
    {
        nameInputPanel.SetActive(true);
    }
    public void HideNameInputPanel()
    {
        nameInputPanel.SetActive(false);
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

    public void ShowCountDownText(bool isEnabled)
    {
        countDownPanel.SetActive(isEnabled);
        countDownText.enabled = isEnabled;
    }

    public void ShowTotalScorePanel(bool isEnabled)
    {
        totalScorePanel.SetActive(isEnabled);
        totalScoreText.enabled = isEnabled;
    }

    public void ShowHighestScorePanel(bool isEnabled)
    {
        highestScorePanel.SetActive(isEnabled);
        highestScoreText.enabled = isEnabled;
    }

    public void ShowMessageDialogPanel(string message, string buttonText)
    {
        messageDialogText.text = message;
        //messageDialogButton.GetComponentInChildren<Text>().text = "abc";
        mesageDialogButtonText.text = buttonText;
        gameOverMenu.SetActive(false);
        messageDialogPanel.SetActive(true);
    }

    public void HideMessageDialogPanel()
    {
        messageDialogPanel.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    public void ShowSubmitCompleteMessage()
    {
        nameInputPanelText.text = "THANK YOU!";
        nameInputSubPanel.SetActive(false);
    }


    public void UpdateCountDown(float timeRemaining, bool win)
    {
        countDownText.text = "TIME REMAINING: " + Mathf.Round(timeRemaining) + "s";
        if (win)
        {
            countDownText.text = "";
        }
    }


}
