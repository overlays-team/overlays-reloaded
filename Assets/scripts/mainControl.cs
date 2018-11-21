using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainControl : MonoBehaviour {

    //array of complete level game objets
    public GameObject[] completesButtons;
    //time to win
    float timeLeft = 5.0f;
    public GameObject pauseButton;

    private bool win;

    // Use this for initialization
    void Start () {

        win = false;
        //all complete level game objets are unactive
        for (int i = 0; i < completesButtons.Length; i++)
        {
            completesButtons[i].SetActive(false);
        }



    }
	
	// Update is called once per frame
	void Update () {

        timeLeft -= Time.deltaTime;

        Debug.Log(timeLeft);
        //if win
        win |= timeLeft < 0;
        if(win){
            Win();
        }

    }




    private void Win(){
        for (int i = 0; i < completesButtons.Length; i++)
        {
            completesButtons[i].SetActive(true);
        }
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }
}
