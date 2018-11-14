using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTestingChild : BlockObject {

    /* block for testing purposes, if this block gets a laser input, it copies the same laser as output*/
    [SerializeField]
    private LaserInput laserInput;
    private Laser inputLaser;
    [SerializeField]
    private LaserOutput laserOutput;


    protected override void Start()
    {
        base.Start();
        laserOutput.active = false;
        degreesToRotate = 45;
    }

    protected override void Update()
    {
        base.Update();
        //here do something else with the lasers how you please
        if (inputLasers.Count > 0)
        {
            foreach(Laser laser in inputLasers)
            {
                //check if one of our input Lasers hits the input
                if(Vector3.Angle(laser.laserOutput.forward, laserInput.transform.forward)<1)
                {
                    laserOutput.active = true;
                    inputLaser = laser;
                    return;
                }
                else
                {
                    laserOutput.active = false;
                    inputLaser = null;
                }
            }
        }
        else
        {
            laserOutput.active = false;
            inputLaser = null;
        }
    }
}
