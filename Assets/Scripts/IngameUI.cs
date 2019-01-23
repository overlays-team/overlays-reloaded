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

    public float blurAnimDuration;

    // Use this for initialization
    void Start()
    {
        TransparentToZero(pauseMenuPanel.transform);
        TransparentToZero(gameOverMenu.transform);
        TransparentToZero(levelCompleteMenu.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator AnimateBlurIn(GameObject panel, float time)
    {
        Material cloneMaterial = Instantiate(panel.GetComponent<Image>().material);

        float elapsedTime = 0;
        float initBlur = cloneMaterial.GetFloat("_Size");
        cloneMaterial.SetFloat("_Size", 0);

        while (elapsedTime < time)
        {
            float newBlur = Mathf.Lerp(0, initBlur, elapsedTime / time);
            cloneMaterial.SetFloat("_Size", newBlur);
            panel.GetComponent<Image>().material = cloneMaterial;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void ShowLevelCompletePanel(int star, int totalScore, int highestTotalScore, bool attackMode)
    {
    
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
        StartCoroutine(AnimateBlurIn(levelCompleteMenu, blurAnimDuration));
    }

    public void TogglePause()
    {
        pauseButton.SetActive(false);
        pauseMenuPanel.SetActive(true);
        StartCoroutine(AnimateBlurIn(pauseMenuPanel, blurAnimDuration));

    }
    public void TogglePlay()
    {
        pauseButton.SetActive(true);
        pauseMenuPanel.SetActive(false);
        TransparentToZero(pauseMenuPanel.transform);
    }
    public void ShowGameOverPanel()
    {
        gameOverMenu.SetActive(true);
        pausePlayButton.SetActive(false);
        StartCoroutine(AnimateBlurIn(gameOverMenu, blurAnimDuration));
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

    private void TransparentToZero(Transform trans)
    {
        Color newColor = new Color(1f, 1f, 1f, 0f);
        foreach (Transform child in trans)
        {
            //child is your child transform
            if (child.gameObject.GetComponent<Button>() != null)
            {
                var colors = child.gameObject.GetComponent<Button>().colors;
                colors.normalColor = newColor;
                child.gameObject.GetComponent<Button>().colors = colors;
            }
            else if (child.gameObject.GetComponent<Text>() != null)
            {
                var colors = child.gameObject.GetComponent<Text>().color;
                colors = newColor;
                child.gameObject.GetComponent<Text>().color = colors;
            }
            else if (child.gameObject.GetComponent<Image>())
            {
                var colors = child.gameObject.GetComponent<Image>().color;
                colors = newColor;
                child.gameObject.GetComponent<Image>().color = colors;
            }

        }
    }


}
