using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseControl : MonoBehaviour {

    //array of pause button
    public GameObject pauseMenus;
    public GameObject pauseButton;
    public GameObject playButton;

    // Use this for initialization
    void Start () {
        //all hidden except pause button
        Resume();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void Pause()
    {   
       
        pauseMenus.SetActive(true);
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;

    }
    public void Resume()
    {
        pauseMenus.SetActive(false);
        pauseButton.SetActive(true);
        playButton.SetActive(false);
        Time.timeScale = 1f;
    }
}
