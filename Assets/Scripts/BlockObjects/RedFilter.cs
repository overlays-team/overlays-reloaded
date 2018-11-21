using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFilter : BlockObject
{
    [SerializeField]
    private LaserInput laserInput;
    private Laser inputLaser;
    [SerializeField]
    private LaserOutput laserOutput;

    protected override void Start()
    {
        base.Start();
        laserOutput.active = false;
    }

    protected override void Update()
    {
        base.Update();

        if (inputLasers.Count > 0)
        {
            foreach (Laser laser in inputLasers)
            {
                //check if one of our input Lasers hits the input
                if (Vector3.Angle(laser.laserOutput.forward, laserInput.transform.forward) < 1)
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
