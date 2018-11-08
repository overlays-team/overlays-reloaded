using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraMovement : MonoBehaviour {

    public GameObject lookAtTarget;
    public Slider sliderX;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(lookAtTarget.transform);
	}

}
