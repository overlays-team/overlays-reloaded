using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    public GameObject logo;
    public GameObject mainMenu;
    public GameObject modeSelect;
    public GameObject levelSelect;

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

    public void ShowModeSelect()
    {
        modeSelect.SetActive(true);
        levelSelect.SetActive(false);
        mainMenu.SetActive(false);
        logo.SetActive(false);
    }

    public void ShowLevelSelect()
    {
        levelSelect.SetActive(true);
        mainMenu.SetActive(false);
        logo.SetActive(false);
        modeSelect.SetActive(false);
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        logo.SetActive(true);
        modeSelect.SetActive(false);
        levelSelect.SetActive(false);
    }
}
