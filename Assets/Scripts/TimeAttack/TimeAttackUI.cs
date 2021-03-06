﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeAttackUI : MonoBehaviour
{
    private string[] goodShoutoutTexts = new string[] { "You did it!", "You rock!", "Awesome!" };
    private string[] mediumShoutoutTexts = new string[] { "Not bad!", "Good!", "Good job!" };
    private string[] badShoutoutTexts = new string[] { "Could be better!", "Don't give up!", "Close one!" };
    public GameObject levelCompletePanel;
    public GameObject levelRevisitPanel;
    public GameObject gameOverPanel;
    public GameObject inventory;
    public GameObject pauseButton;
    public GameObject pauseMenuPanel;
    public Text shoutOutText;
    public Text timerDecimalText;
    public Text timerFloatText;
    public Image timerGraphic;
    public GameObject timerPanel;
    public Text totalScoreText;
    public Text scoreSubmitText;
    public Text scoreCountText;
    public Text timeLeftCounterText;
    public InputField nameInputField;
    public GameObject submitButton;
    public GameObject countdownDisplay;
    public GameObject startPanel;
    public GameObject tutorialButton;
    public GameObject tutorialPanel;

    public float winWaitTime = 2f;
    public float blurAnimDuration;

    // Use this for initialization
    void Start()
    {
        // Reset text transparencies to animate them back in with a fade in animation
        TransparencyToZero(pauseMenuPanel.transform);
        TransparencyToZero(gameOverPanel.transform);
        TransparencyToZero(levelCompletePanel.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenuPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            IngameManager.Instance.Resume();
        }
    }

    // Animates UI blur components asynchronously
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

    public IEnumerator AnimateScoreRefill(float timeLeft, int totalScore, int oldScore)
    {
        totalScoreText.text = oldScore.ToString();
        timeLeftCounterText.text = Mathf.Round(timeLeft).ToString();
        // Delay the animation so the blur in animation finishes first
        float delay = 1;
        while(delay > 0)
        {
            delay -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        // Refill 1 second from the time left times the score multiplier to the total score (i.e. 28 sec = 280 score)
        while(int.Parse(timeLeftCounterText.text) > 0)
        {
            totalScoreText.text = (int.Parse(totalScoreText.text) + TimeAttackManager.Instance.scoreMultiplier).ToString();
            timeLeftCounterText.text = (int.Parse(timeLeftCounterText.text) - 1).ToString();
            yield return new WaitForSeconds(0.05f);
        }
    }

    #region tutorial code
    public void ToogleTutorialOn()
    {
        if (tutorialPanel.activeInHierarchy)
        {
            tutorialPanel.SetActive(false);
        }
        else
        {
            tutorialPanel.SetActive(true);
        }
    }
    #endregion

    public void ShowCountdownDisplay()
    {
        StartCoroutine(AnimateCountdown(3));
    }

    IEnumerator AnimateCountdown(float animDuration)
    {
        countdownDisplay.SetActive(true);
        yield return new WaitForSeconds(animDuration);
        countdownDisplay.SetActive(false);
    }

    public void ShowTutorialButton()
    {
        tutorialButton.SetActive(true);
    }

    public void HideTutorialButton()
    {
        tutorialButton.SetActive(false);
    }

    public void HideCountdownDisplay()
    {
        countdownDisplay.SetActive(false);
    }

    public void ShowTimerDisplay()
    {
        timerPanel.SetActive(true);
    }

    public void HideTimerDisplay()
    {
        timerPanel.SetActive(false);
    }

    public void ShowPauseButton()
    {
        pauseButton.SetActive(true);
    }

    public void HidePauseButton()
    {
        pauseButton.SetActive(false);
    }

    public void ShowStartPanel()
    {
        startPanel.SetActive(true);
    }

    public void HideStartPanel()
    {
        startPanel.SetActive(false);
    }

    public void ShowInventory()
    {
        inventory.SetActive(true);
    }

    public void HideInventory()
    {
        inventory.SetActive(false);
    }
    
    //shows the level complete panel delayed
    public void ShowLevelCompletePanel(float timeLeft, int totalScore, int oldScore)
    {
        StartCoroutine(DelayedLevelCompletePanel(timeLeft, totalScore, oldScore));
        HideInventory();
        HidePauseButton();
    }

    // Delay the appearance of the LevelCompletePanel so the user can look at the game result
    IEnumerator DelayedLevelCompletePanel(float timeLeft, int totalScore, int oldScore)
    {
        yield return new WaitForSeconds(winWaitTime);

        StartCoroutine(AnimateScoreRefill(timeLeft, totalScore, oldScore));

        if (timeLeft > 20)
        {
            shoutOutText.text = goodShoutoutTexts[Random.Range(0, 3)];
        }
        else if (timeLeft > 10)
        {
            shoutOutText.text = mediumShoutoutTexts[Random.Range(0, 3)];
        }
        else
        {
            shoutOutText.text = badShoutoutTexts[Random.Range(0, 3)];
        }

        levelCompletePanel.SetActive(true);
        pauseButton.SetActive(false);
        pauseMenuPanel.SetActive(false);
        HideTutorialButton();
        StartCoroutine(AnimateBlurIn(levelCompletePanel, blurAnimDuration));
    }

    //shows the level complete panel at once
    public void ShowLevelCompletePanel()
    {
        levelCompletePanel.SetActive(true);
        pauseButton.SetActive(false);
        pauseMenuPanel.SetActive(false);
        inventory.SetActive(false);
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
        TransparencyToZero(pauseMenuPanel.transform);
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        pauseButton.SetActive(false);
        StartCoroutine(AnimateBlurIn(gameOverPanel, blurAnimDuration));
    }

    public void ShowNameInputPanel()
    {
        nameInputField.gameObject.SetActive(true);
        submitButton.SetActive(true);
    }
    public void HideNameInputPanel()
    {
        nameInputField.gameObject.SetActive(false);
        submitButton.SetActive(false);
    }

    public void HideLevelCompletePanel()
    {
        //Reset the transparency for the fade-in animation
        foreach (Text text in levelCompletePanel.transform.GetComponentsInChildren<Text>())
        {
            text.color = new Color(1, 1, 1, 0);
        }
        levelCompletePanel.SetActive(false);
    }
    public void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void ShowCountDownText(bool isEnabled)
    {
        timerPanel.SetActive(isEnabled);
        timerDecimalText.enabled = isEnabled;
    }

    public void ShowSubmitCompleteMessage()
    {
        scoreSubmitText.text = "Thank You!";
        nameInputField.gameObject.SetActive(false);
        submitButton.SetActive(false);
    }

    public void ShowLevelRevisitPanel()
    {
        levelRevisitPanel.SetActive(true);
    }

    public void HideLevelRevisitPanel()
    {
        levelRevisitPanel.SetActive(false);
    }

    public void UpdateCountDown(float time, float maxTime)
    {
        timerDecimalText.text = Mathf.Floor(time).ToString("00");
        timerFloatText.text = (time % 1 * 100).ToString("00");
        timerGraphic.fillAmount = time / maxTime;
    }

    // Reset text transparencies to 0 so they can be animated again
    private void TransparencyToZero(Transform trans)
    {
        Color transparentColor = new Color(1f, 1f, 1f, 0f);
        foreach (Transform child in trans)
        {
            //child is your child transform
            if (child.gameObject.GetComponent<Text>() != null)
            {
                var colors = child.gameObject.GetComponent<Text>().color;
                colors = transparentColor;
                child.gameObject.GetComponent<Text>().color = colors;
            }
            else if (child.gameObject.GetComponent<Image>())
            {
                var colors = child.gameObject.GetComponent<Image>().color;
                colors = transparentColor;
                child.gameObject.GetComponent<Image>().color = colors;
            }
        }
    }
}
