using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mirror : BlockObject
{
    [Header("Mirror")]
    [Tooltip("needs to have the same forward direction as the mirror")]
    public LaserOutput outputLaserFront;
    [Tooltip("needs to have the opposite direction as the mirror")]
    public LaserOutput outputLaserBack;


    List<Laser> inputLasersFrontThisFrame = new List<Laser>();
    List<Laser> inputLasersBackThisFrame = new List<Laser>();

    //we only reflect one laser at once, if another laser coulb be reflected by this mirror, we still reflect the old one

    protected override void Start()
    {
        base.Start();
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
            if (Vector3.Angle(laser.transform.forward, transform.forward)<85)
            {
                if(!laser.isMovingFast) inputLasersBackThisFrame.Add(laser);
            }
            else if (Vector3.Angle(laser.transform.forward, -transform.forward) < 85)
            {
                if (!laser.isMovingFast) inputLasersFrontThisFrame.Add(laser);
            }
            
        }

        //now we check if one of our collection is bigger than 1, if not, just reflect

        //front
        if (inputLasersFrontThisFrame.Count == 0)
        {
            outputLaserFront.active = false;
        }
        else if (inputLasersFrontThisFrame.Count == 1)
        {
            outputLaserFront.laser.image = inputLasersFrontThisFrame[0].image;
            outputLaserFront.transform.forward = Vector3.Reflect(inputLasersFrontThisFrame[0].transform.forward, transform.forward);
            outputLaserFront.active = true;
        }
        else
        {
            outputLaserFront.active = false;
        }

        //back
        if (inputLasersBackThisFrame.Count == 0)
        {
            outputLaserBack.active = false;
        }
        else if (inputLasersBackThisFrame.Count == 1)
        {
            outputLaserBack.laser.image = inputLasersBackThisFrame[0].image;
            outputLaserBack.transform.forward = Vector3.Reflect(inputLasersBackThisFrame[0].transform.forward, -transform.forward);
            outputLaserBack.active = true;
        }
        else
        {
            outputLaserBack.active = false;
        }


    }
}
