using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonControl : MonoBehaviour {
   
    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Home(){
        SceneManager.LoadScene("Home");
    }

    public void Select(string Level)
    {
        SceneManager.LoadScene(Level);
    }
}
