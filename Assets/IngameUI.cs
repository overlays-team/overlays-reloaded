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
        myImageComponent = LevelCompleteMenu.transform.Find("StarReceivementImage").gameObject.GetComponent<Image>();
        switch (wert)
        {
            case 1:
                myImageComponent.sprite = Star1;
                break;
            case 2:
                myImageComponent.sprite = Star2;
                break;
            case 3:
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
