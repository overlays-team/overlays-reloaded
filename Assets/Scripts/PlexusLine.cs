using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlexusLine : MonoBehaviour
{
    bool startFade;
    public float lerpSpeed;
    public float fadeDuration;
    public float maxLength;
    private LineRenderer lr;
    private Color targetColor;

	// Use this for initialization
	void Awake () {
        lr = GetComponent<LineRenderer>();
        ResetColor();
    }
	
	// Update is called once per frame
	void Update () {
        CalculateTargetColor();
        lr.startColor = Color.Lerp(lr.startColor, targetColor, lerpSpeed);
        lr.endColor = Color.Lerp(lr.endColor, targetColor, lerpSpeed);
	}

    void CalculateTargetColor()
    {
        float lineLength = Vector3.Distance(lr.GetPosition(0), lr.GetPosition(1));    
        targetColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, 1 - (lineLength/maxLength));
    }

    void OnDisable()
    {
        //ResetColor();
    }

    void OnEnable()
    {
        //ResetColor();
    }

   void ResetColor()
    {/*
        lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, 0);
        lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, 0);*/
    }
}
