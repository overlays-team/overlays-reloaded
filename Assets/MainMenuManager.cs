using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    public GameObject MainMenu;
    public GameObject LevelSelect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitApp()
    {
        Application.Quit();
    }

    public void MainMenulevelSelectSwitch()
    {
        if (MainMenu.activeSelf)
        {
            MainMenu.SetActive(false);
            LevelSelect.SetActive(true);
        } else if (LevelSelect.activeSelf)
        {
            MainMenu.SetActive(true);
            LevelSelect.SetActive(false);
        }
    }
}
