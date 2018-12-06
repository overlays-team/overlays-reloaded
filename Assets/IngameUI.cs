using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour {
    public GameObject levelCompleteMenu;
    Image myImageComponent;
    public GameObject gameOverMenu;
    public Sprite star1;
    public Sprite star2;
    public Sprite star3;
    public GameObject pauseButton;
    public GameObject playButton;
    public GameObject pauseMenuButton;
    public GameObject pausePlayButton;
    private string[] star3Texts = new string[] { "You did it!", "You rock!", "Awesome!" };
    private string[] star2Texts = new string[] { "Not bad!", "Good!", "Good job!" };
    private string[] star1Texts = new string[] { "Could be better!", "Don't give up!", "Lucky!" };
    Text levelCompleteText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowLevelCompletePanel(int wert)
    {

        Debug.Log("You Win!");
        levelCompleteText = levelCompleteMenu.transform.Find("LevelCompleteText").gameObject.GetComponent<Text>();
        myImageComponent = levelCompleteMenu.transform.Find("StarReceivementImage").gameObject.GetComponent<Image>();
        switch (wert)
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
        pauseMenuButton.SetActive(false);

    }

    public void TogglePause()
    {
        pauseButton.SetActive(false);
        //PlayButton.SetActive(true);
        pauseMenuButton.SetActive(true);
    }
    public void TogglePlay()
    {
        pauseButton.SetActive(true);
        //PlayButton.SetActive(false);
        pauseMenuButton.SetActive(false);

    }
    public void ShowGameOverPanel()
    {
        gameOverMenu.SetActive(true);
        pausePlayButton.SetActive(false);
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
}
