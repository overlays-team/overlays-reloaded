﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUI : MonoBehaviour {
    public GameObject LevelCompleteMenu;
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
    public void ShowLevelCompletePanel()
    {
        Debug.Log("You Win!");
        LevelCompleteMenu.SetActive(true);
        PausePlayButton.SetActive(false);

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
}
