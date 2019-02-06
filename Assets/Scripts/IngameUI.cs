using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour {

    //Shout out texts
    private string[] goodShoutoutTexts = new string[] { "You did it!", "You rock!", "Awesome!" };
    private string[] mediumShoutoutTexts = new string[] { "Not bad!", "Good!", "Good job!" };
    private string[] badShoutoutTexts = new string[] { "Could be better!", "Don't give up!", "Close one!" };
    public Text shoutOutText;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public GameObject levelCompletePanel;
    public GameObject levelRevisitPanel;
    public GameObject pauseMenuPanel;
    public GameObject inventory;
    public GameObject pauseButton;
    public GameObject nextButton;

    public float winWaitTime = 2f;
    public float blurAnimDuration;

    //Tutorial variables
    public GameObject tutorialPanel;
    public GameObject tutorialButton;
    public bool tutorialOn;

    // Use this for initialization
    void Start()
    {
        //Make panels transparent at first
        TransparentToZero(pauseMenuPanel.transform);
        TransparentToZero(levelCompletePanel.transform);

        if (tutorialOn) ToogleTutorialOn();
    }

    #region tutorial code
    public void ToogleTutorialOn()
    {
        if (tutorialPanel.activeInHierarchy)
        { 
            tutorialPanel.SetActive(false);
            ShowIngameUI();
            IngameManager.Instance.ResumeGame();
        }
        else
        {    
            tutorialPanel.SetActive(true);
            HideIngameUI();
            IngameManager.Instance.PauseGame();
        }
    }
    #endregion

    //Hide all UI
    public void HideIngameUI()
    {
        Debug.Log("Hide");
        pauseButton.SetActive(false);
        tutorialButton.SetActive(false);
        inventory.SetActive(false);
    }

    //Show all UI
    public void ShowIngameUI()
    {
        pauseButton.SetActive(true);
        inventory.SetActive(true);
        if (tutorialOn)tutorialButton.SetActive(true);
    }

    //Show review panel
    public void ShowLevelRevisitPanel()
    {
        levelRevisitPanel.SetActive(true);
    }

    //Close review panel
    public void HideLevelRevisitPanel()
    {
        levelRevisitPanel.SetActive(false);
    }

    //Hide level complete for review state
    public void HideLevelCompletePanelForRevisit()
    {
        TransparentToZero(levelCompletePanel.transform);
        levelCompletePanel.SetActive(false);
    }

    //Bluring animation for panel
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

    //Delay to show level complete panel after win the level
    IEnumerator DelayedLevelCompletePanel(int star, bool endOfLevel)
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

        //Not active next button in the end of level
        if (endOfLevel)
        {
            nextButton.SetActive(false);
        }

        StartCoroutine(AnimateBlurIn(levelCompletePanel, blurAnimDuration));
    }

    //Show level complete
    public void ShowLevelCompletePanel(int star, bool endOfLevel)
    {
        StartCoroutine(DelayedLevelCompletePanel(star, endOfLevel));
        IngameManager.Instance.PauseGame();
    }

    //Show the level complete panel at once
    public void ShowLevelCompletePanel()
    {
        levelCompletePanel.SetActive(true);
        pauseButton.SetActive(false);
        pauseMenuPanel.SetActive(false);
        inventory.SetActive(false);
    }

    //Game paused toggle
    public void TogglePause()
    {
        HideIngameUI();
        pauseMenuPanel.SetActive(true);
        StartCoroutine(AnimateBlurIn(pauseMenuPanel, blurAnimDuration));

    }

    //Game played toggle
    public void TogglePlay()
    {
        ShowIngameUI();
        pauseMenuPanel.SetActive(false);
        TransparentToZero(pauseMenuPanel.transform);
    }

    //Hide level complete panel
    public void HideLevelCompletePanel()
    {
        ShowIngameUI();
        levelCompletePanel.SetActive(false);
    }

    //Do all childerns to transparent
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
