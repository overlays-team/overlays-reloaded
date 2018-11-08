using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainControl : MonoBehaviour {

    //array of complete level game objets
    public GameObject[] buttons;
    //time to win
    float timeLeft = 5.0f;

    // Use this for initialization
    void Start () {


        //all complete level game objets are unactive
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {

        timeLeft -= Time.deltaTime;
        //if win
        if (timeLeft < 0)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].SetActive(true);
            }
        }

    }
}
