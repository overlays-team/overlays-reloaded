using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour {

    private string[] goodShoutoutTexts = new string[] { "You did it!", "You rock!", "Awesome!" };
    private string[] mediumShoutoutTexts = new string[] { "Not bad!", "Good!", "Good job!" };
    private string[] badShoutoutTexts = new string[] { "Could be better!", "Don't give up!", "Close one!" };
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public GameObject levelCompletePanel;
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;
    public GameObject inventory;
    public GameObject pauseButton;
    public Text shoutOutText;

    public float winWaitTime = 2f;
    public float blurAnimDuration;

    //tutorial variables
    public GameObject tutorialPanel;
    public GameObject tutorialButton;
    public bool tutorialOn;




    // Use this for initialization
    void Start()
    {
        TransparentToZero(pauseMenuPanel.transform);
        TransparentToZero(gameOverPanel.transform);
        TransparentToZero(levelCompletePanel.transform);

        if (tutorialOn) ToogleTutorialOn();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenuPanel.active && Input.GetKeyDown(KeyCode.Escape))
        {
            IngameManager.Instance.Resume();
        }
    }

    #region tutorial code
    public void ToogleTutorialOn()
    {
        if (tutorialPanel.activeInHierarchy)
        { 
            tutorialPanel.SetActive(false);
            IngameManager.Instance.ResumeGame();
        }
        else
        {    
            tutorialPanel.SetActive(true); 
            IngameManager.Instance.PauseGame();
        }
    }
    #endregion

    public void HideIngameUI()
    {
        pauseButton.SetActive(false);
        tutorialButton.SetActive(false);
        inventory.SetActive(false);
    }

    public void ShowIngameUI()
    {
        pauseButton.SetActive(true);
        inventory.SetActive(true);
        if (tutorialOn)tutorialButton.SetActive(true);
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

    IEnumerator WaitAndPrint(int star)
    {
        yield return new WaitForSeconds(winWaitTime);

        switch (star)
        {
            case 1:
                shoutOutText.text = badShoutoutTexts[Random.Range(0, 3)];
                star2.SetActive(false);
                star3.SetActive(false);
                break;
            case 2:
                shoutOutText.text = mediumShoutoutTexts[Random.Range(0, 3)];
                star3.SetActive(false);
                break;
            case 3:
                shoutOutText.text = goodShoutoutTexts[Random.Range(0, 3)];
                break;
        }

        levelCompletePanel.SetActive(true);
        pauseButton.SetActive(false);
        pauseMenuPanel.SetActive(false);
        StartCoroutine(AnimateBlurIn(levelCompletePanel, blurAnimDuration));
    }

    public void ShowLevelCompletePanel(int star)
    {
        StartCoroutine(WaitAndPrint(star));
        IngameManager.Instance.PauseGame();
    }

    public void TogglePause()
    {
        HideIngameUI();
        pauseMenuPanel.SetActive(true);
        StartCoroutine(AnimateBlurIn(pauseMenuPanel, blurAnimDuration));

    }
    public void TogglePlay()
    {
        ShowIngameUI();
        pauseMenuPanel.SetActive(false);
        TransparentToZero(pauseMenuPanel.transform);
    }
    public void ShowGameOverPanel()
    {
        HideIngameUI();
        gameOverPanel.SetActive(true);
        StartCoroutine(AnimateBlurIn(gameOverPanel, blurAnimDuration));
    }

    public void HideLevelCompletePanel()
    {
        ShowIngameUI();
        levelCompletePanel.SetActive(false);
    }
    public void HideGameOverPanel()
    {
        ShowIngameUI();
        gameOverPanel.SetActive(false);
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
            else if (child.gameObject.GetComponent<Image>() != null)
            {
                var colors = child.gameObject.GetComponent<Image>().color;
                colors = newColor;
                child.gameObject.GetComponent<Image>().color = colors;
            }

        }
    }


}
