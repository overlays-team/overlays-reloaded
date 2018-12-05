using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepScaleAfterReactivation : MonoBehaviour {

    public Transform origTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        Debug.Log("OnEnable is working");
    }
}
