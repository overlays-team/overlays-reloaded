using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentMirror : BlockObject
{
    [Header("Translucent Mirror")]

    [Tooltip("this is the reflected laser")]
    public LaserOutput outputLaser1;
    [Tooltip("this is the laser which passes through the mirror")]
    public LaserOutput outputLaser2;
    /*[Tooltip("this is the reflected laser")]
    public LaserOutput outputLaserBack1;
    [Tooltip("this is the laser which passes through the mirror")]
    public LaserOutput outputLaserBack2;*/


    //inputLaser[0] is on the forwrd side of the mirror, inputLaser[1] on the backside

    List<Laser> inputLasersThisFrame = new List<Laser>();

    //we only reflect one laser at once, if another laser coulb be reflected by this mirror, we still reflect the old one

    protected override void Start()
    {
        base.Start();

        //laserInputMaxIncidenceAngle = 85;
    }

    protected override void Update()
    {
        base.Update();

        inputLasersThisFrame.Clear();

        foreach (Laser laser in LaserManager.Instance.GetInputLasers(this))
        {
            if (!laser.isMovingFast) inputLasersThisFrame.Add(laser);
        }

        if (inputLasersThisFrame.Count == 0)
        {
            outputLaser1.active = false;
            outputLaser2.active = false;
        }
        else if (inputLasersThisFrame.Count == 1)
        {
            outputLaser1.laser.image = inputLasersThisFrame[0].image;
            outputLaser1.transform.forward = Vector3.Reflect(inputLasersThisFrame[0].transform.forward, transform.forward);
            outputLaser1.active = true;

            outputLaser2.laser.image = inputLasersThisFrame[0].image;
            outputLaser2.transform.forward = inputLasersThisFrame[0].transform.forward;
            outputLaser2.active = true;
        }
        else //if 2
        {
            outputLaser1.laser.image = inputLasersThisFrame[0].image;
            outputLaser1.transform.forward = inputLasersThisFrame[0].transform.forward;
            outputLaser1.active = true;

            outputLaser2.laser.image = inputLasersThisFrame[0].image;
            outputLaser2.transform.forward = inputLasersThisFrame[1].transform.forward;
            outputLaser2.active = true;
        }


    }
}
