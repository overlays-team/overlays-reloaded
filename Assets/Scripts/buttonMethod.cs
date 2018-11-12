using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonMethod : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
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
        Debug.Log("Level Selected.");
    }
}
