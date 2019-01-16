using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentMirror : BlockObject
{
    [Header("Translucent Mirror")]

    [Tooltip("this is the reflected laser")]
    public LaserOutput outputLaserFront1;
    [Tooltip("this is the laser which passes through the mirror")]
    public LaserOutput outputLaserFront2;
    [Tooltip("this is the reflected laser")]
    public LaserOutput outputLaserBack1;
    [Tooltip("this is the laser which passes through the mirror")]
    public LaserOutput outputLaserBack2;


    //inputLaser[0] is on the forwrd side of the mirror, inputLaser[1] on the backside

    List<Laser> inputLasersFrontThisFrame = new List<Laser>();
    List<Laser> inputLasersBackThisFrame = new List<Laser>();

    //we only reflect one laser at once, if another laser coulb be reflected by this mirror, we still reflect the old one

    protected override void Start()
    {
        base.Start();

        //laserInputMaxIncidenceAngle = 85;
    }

    protected override void Update()
    {
        base.Update();

        //first we fill the collections - how many do hit the front side of the mirror and how many the backside?

        inputLasersFrontThisFrame.Clear();
        inputLasersBackThisFrame.Clear();

        foreach (Laser laser in LaserManager.Instance.GetInputLasers(this))
        {
            //do we have one or more frontLasers
            if (Vector3.Angle(laser.laserOutput.forward, transform.forward) < 85)
            {
                if (!laser.isMovingFast) inputLasersBackThisFrame.Add(laser);
            }
            else if (Vector3.Angle(laser.laserOutput.forward, -transform.forward) < 85)
            {
                if (!laser.isMovingFast) inputLasersFrontThisFrame.Add(laser);
            }

        }

        //now we check if one of our collection is bigger than 1, if not, just reflect

        //front
        if (inputLasersFrontThisFrame.Count == 0)
        {
            outputLaserFront1.active = false;
            outputLaserFront2.active = false;
        }
        else if (inputLasersFrontThisFrame.Count == 1)
        {
            outputLaserFront1.laser.image = inputLasersFrontThisFrame[0].image;
            outputLaserFront1.transform.forward = Vector3.Reflect(inputLasersFrontThisFrame[0].laserOutput.forward, transform.forward);
            outputLaserFront1.active = true;

            outputLaserFront2.laser.image = inputLasersFrontThisFrame[0].image;
            outputLaserFront2.transform.forward = inputLasersFrontThisFrame[0].laserOutput.forward;
            outputLaserFront2.active = true;
        }

        //back
        if (inputLasersBackThisFrame.Count == 0)
        {
            outputLaserBack1.active = false;
            outputLaserBack2.active = false;
        }
        else if (inputLasersBackThisFrame.Count == 1)
        {
            outputLaserBack1.laser.image = inputLasersBackThisFrame[0].image;
            outputLaserBack1.transform.forward = Vector3.Reflect(inputLasersBackThisFrame[0].laserOutput.forward, -transform.forward);
            outputLaserBack1.active = true;

            outputLaserBack2.laser.image = inputLasersBackThisFrame[0].image;
            outputLaserBack2.transform.forward = inputLasersBackThisFrame[0].laserOutput.forward;
            outputLaserBack2.active = true;
        }


    }
}
