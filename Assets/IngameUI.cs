using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour {
    public GameObject LevelCompleteMenu;
    Image myImageComponent;
    public GameObject GameOverMenu;
    public Sprite Star1;
    public Sprite Star2;
    public Sprite Star3;
    public GameObject PauseButton;
    public GameObject PlayButton;
    public GameObject PauseMenuButton;
    public GameObject PausePlayButton;
    private string[] Star3Texts = new string[] { "You did it!", "You rock it!", "Awesome!" };
    private string[] Star2Texts = new string[] { "Not bad!", "Good!", "Good job!" };
    private string[] Star1Texts = new string[] { "Could be better!", "Don't give up!", "Lucky!" };
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
        levelCompleteText = LevelCompleteMenu.transform.Find("LevelCompleteText").gameObject.GetComponent<Text>();
        myImageComponent = LevelCompleteMenu.transform.Find("StarReceivementImage").gameObject.GetComponent<Image>();
        switch (wert)
        {
            case 1:
                levelCompleteText.text = Star1Texts[Random.Range(0, 3)];
                myImageComponent.sprite = Star1;
                break;
            case 2:
                levelCompleteText.text = Star2Texts[Random.Range(0, 3)];
                myImageComponent.sprite = Star2;
                break;
            case 3:
                levelCompleteText.text = Star3Texts[Random.Range(0, 3)];
                myImageComponent.sprite = Star3;
                break;
        }

        LevelCompleteMenu.SetActive(true);
        PausePlayButton.SetActive(false);
        PauseMenuButton.SetActive(false);

    }

    public void TogglePause()
    {
        PauseButton.SetActive(false);
        PlayButton.SetActive(true);
        PauseMenuButton.SetActive(true);


    }
    public void TogglePlay()
    {
        PauseButton.SetActive(true);
        PlayButton.SetActive(false);
        PauseMenuButton.SetActive(false);

    }
    public void ShowGameOverPanel()
    {
        Debug.Log("You lose!");
        GameOverMenu.SetActive(true);
        PausePlayButton.SetActive(false);
    }
    public void HideLevelCompletePanel()
    {
        LevelCompleteMenu.SetActive(false);
        PausePlayButton.SetActive(true);

    }
    public void HideGameOverPanel()
    {
        GameOverMenu.SetActive(false);
        PausePlayButton.SetActive(true);
    }
}
