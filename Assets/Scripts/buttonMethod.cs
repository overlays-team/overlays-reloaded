using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonMethod : MonoBehaviour {

    public GameObject levelSelect;
    public GameObject clickParticle;
    GameObject mainMenu;

	// Use this for initialization
	void Start () {
        mainMenu = GameObject.FindGameObjectWithTag("MainMenuScreen");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void exitApp()
    {
        Application.Quit();
        Debug.Log("App ended.");
    }

    public void levelSelectScreen()
    {
        //Hier "Level Select"-Screen öffnen.
        //clickParticle.SetActive(true);
        levelSelect.SetActive(true);
        mainMenu.SetActive(false);
        Debug.Log("Level Selected.");
    }
}
