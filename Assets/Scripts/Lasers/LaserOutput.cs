using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOutput : MonoBehaviour {

    /*
     * Every blockObjects which shoots a laser has one or several laserOutputs
     * the lasers rotation is the same as the outputs
     */

    public GameObject laserPrefab;
    public Laser laser;
    public bool active = false;

	void Awake () {
        laser = Instantiate(laserPrefab).GetComponent<Laser>();
        laser.active = false;
        laser.startingBlock = transform.parent.GetComponent<BlockObject>();
        laser.transform.position = transform.position;
        laser.transform.forward = transform.forward;
        laser.transform.parent = transform;
	}
	
	void Update () {
        if (active)
        {
            laser.active = true;
        }
        else
        {
            laser.active = false;
        }
	}

    public void SetLaser(Transform startPoint, BlockObject startingBlock)
    {
        laser.startingBlock = startingBlock;
    }


}
