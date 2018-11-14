using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class middlePointRotator : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(new Vector3(90, 0, 0));
        }
	}
}
