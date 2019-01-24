using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeAttackUI : MonoBehaviour
{
    private string[] star3Texts = new string[] { "You did it!", "You rock!", "Awesome!" };
    private string[] star2Texts = new string[] { "Not bad!", "Good!", "Good job!" };
    private string[] star1Texts = new string[] { "Could be better!", "Don't give up!", "Lucky!" };
    public GameObject levelCompletePanel;
    public GameObject gameOverPanel;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
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
        TransparentToZero(pauseMenuPanel.transform);
        TransparentToZero(gameOverPanel.transform);
        TransparentToZero(levelCompletePanel.transform);
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

    IEnumerator WaitAndPrint(int star, int totalScore, int highestTotalScore, bool attackMode)
    {
        yield return new WaitForSeconds(winWaitTime);

        if (attackMode) totalScoreText.text = GameDataEditor.Instance.data.playerName + "'s SCORE: " + totalScore;

        if (attackMode) highestScoreText.text = "HIGHEST SCORE: " + highestTotalScore;

        switch (star)
        {
            case 1:
                shoutOutText.text = star1Texts[Random.Range(0, 3)];
                star2.SetActive(false);
                star3.SetActive(false);

                break;
            case 2:
                shoutOutText.text = star2Texts[Random.Range(0, 3)];
                star3.SetActive(false);
                break;
            case 3:
                shoutOutText.text = star3Texts[Random.Range(0, 3)];
                break;
        }

        levelCompletePanel.SetActive(true);
        pauseButton.SetActive(false);
        pauseMenuPanel.SetActive(false);
        StartCoroutine(AnimateBlurIn(levelCompletePanel, blurAnimDuration));
    }

    public void ShowLevelCompletePanel(int star, int totalScore, int highestTotalScore, bool attackMode)
    {
        //levelCompleteText = levelCompleteMenu.transform.Find("LevelCompletedText").gameObject.GetComponent<Text>();
        //myImageComponent = levelCompleteMenu.transform.Find("StarReceivementImage").gameObject.GetComponent<Image>();

        //sh
        //totalScoreText.text = "YOUR SCORE: " + totalScore;
        StartCoroutine(WaitAndPrint(star, totalScore, highestTotalScore, attackMode));
        TimeAttackManager.Instance.PauseGame();
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

    public void UpdateCountDown(float time, float maxTime)
    {
        timerDecimalText.text = time.ToString("00");
        timerFloatText.text = (time % 1 * 100).ToString("00");
        timerGraphic.fillAmount = time / maxTime;
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
