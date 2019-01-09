using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    //public Text fpsText;

    //Declare these in your class
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;

    string frameRate;


    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }

        //fpsText.text = ((int)(m_lastFramerate)).ToString();
        //fpsText.text = ((int)(1.0f / Time.deltaTime)).ToString();
        frameRate = ((int)(m_lastFramerate)).ToString();

    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = Screen.height * 2 / 25;
        style.normal.textColor = new Color(1f, 1f, 1f, 1.0f);
        GUI.Label(new Rect(10, 10, 200, 200), frameRate, style);
    }
}
