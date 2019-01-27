﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeAttackUI : MonoBehaviour
{
    private string[] badShoutoutTexts = new string[] { "You did it!", "You rock!", "Awesome!" };
    private string[] mediumShoutoutTexts = new string[] { "Not bad!", "Good!", "Good job!" };
    private string[] goodShoutoutTexts = new string[] { "Could be better!", "Don't give up!", "Lucky!" };
    public GameObject levelCompletePanel;
    public GameObject LevelRevisitPanel;
    public GameObject gameOverPanel;
    public GameObject pauseButton;
    public GameObject pauseMenuPanel;
    public Text shoutOutText;
    public Text timerDecimalText;
    public Text timerFloatText;
    public Image timerGraphic;
    public GameObject timerPanel;
    public GameObject totalScorePanel;
    public GameObject highestScorePanel;
    public Text totalScoreText;
    public Text highestScoreText;
    public Text scoreSubmitText;
    public Text scoreCountText;
    public InputField nameInputField;
    public GameObject SubmitButton;

    public float winWaitTime = 2f;
    public float blurAnimDuration;

    // Use this for initialization
    void Start()
    {
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

    IEnumerator DelayedLevelCompletePanel(float timeLeft, int totalScore, int highestTotalScore)
    {
        yield return new WaitForSeconds(winWaitTime);

        totalScoreText.text = GameDataEditor.Instance.data.playerName + "'s SCORE: " + totalScore;
        highestScoreText.text = "HIGHEST SCORE: " + highestTotalScore;

        if(timeLeft > 20)
        {
            shoutOutText.text = goodShoutoutTexts[Random.Range(0, 3)];
        }
        else if(timeLeft > 10)
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
        StartCoroutine(AnimateBlurIn(levelCompletePanel, blurAnimDuration));
    }

    public void ShowLevelCompletePanel(int star, int totalScore, int highestTotalScore)
    {
        StartCoroutine(DelayedLevelCompletePanel(star, totalScore, highestTotalScore));
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
        SubmitButton.SetActive(true);
    }
    public void HideNameInputPanel()
    {
        nameInputField.gameObject.SetActive(false);
        SubmitButton.SetActive(false);
    }

    public void HideLevelCompletePanel()
    {
        //Reset the transparency for the fade-in animation
        foreach (Text text in levelCompletePanel.transform.GetComponentsInChildren<Text>())
        {
            text.color = new Color(1, 1, 1, 0);
        }
        levelCompletePanel.SetActive(false);
        pauseButton.SetActive(true);
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

    public void ShowSubmitCompleteMessage()
    {
        scoreSubmitText.text = "THANK YOU!";
        nameInputField.gameObject.SetActive(false);
        SubmitButton.SetActive(false);
    }

    public void ShowLevelRevisitPanel()
    {
        LevelRevisitPanel.SetActive(true);
    }

    public void HideLevelRevisitPanel()
    {
        LevelRevisitPanel.SetActive(false);
    }

    public void UpdateCountDown(float time, float maxTime)
    {
        timerDecimalText.text = time.ToString("00");
        timerFloatText.text = (time % 1 * 100).ToString("00");
        timerGraphic.fillAmount = time / maxTime;
    }

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
