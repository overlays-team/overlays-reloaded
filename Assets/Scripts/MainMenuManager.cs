using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    [Header("Insert Prefabs")]
    public GameObject mainMenu;
    public GameObject levelSelect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Functions of the main menu buttons
    public void ExitApp()
    {
        Application.Quit();
    }

    public void MainMenuLevelSelectSwitch()
    {
        if (mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
            levelSelect.SetActive(true);
        } else if (levelSelect.activeSelf)
        {
            mainMenu.SetActive(true);
            levelSelect.SetActive(false);
        }
    }
}
