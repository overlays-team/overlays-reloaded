using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraMovement : MonoBehaviour {

    public GameObject lookAtTarget;
    public Slider sliderX;
    public Slider sliderY;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(lookAtTarget.transform);
        transform.position = new Vector3(sliderX.value, transform.position.y, transform.position.z);
        transform.position = new Vector3(transform.position.x, sliderY.value, transform.position.z);
    }

}
