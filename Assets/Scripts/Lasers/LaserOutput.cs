using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOutput : MonoBehaviour {

    /*
     * Jedes BlockObject, welches Laser rausschießt hat einen oder mehrere LaserOutputs
     * Abhängig davon wie die Rotation eines Laser  outputs ist, so werden auch die Laser rausgeschossen
     */

    public GameObject laserPrefab;
    public Laser laser;
    public bool active = false;

	void Awake () {
        laser = Instantiate(laserPrefab).GetComponent<Laser>();
        laser.active = false;
        laser.startingBlock = transform.parent.GetComponent<BlockObject>();
        laser.laserOutput = transform;  //output point sets direction, can be at 000 transform of parent
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
        laser.laserOutput = startPoint;
        laser.startingBlock = startingBlock;
    }


}
