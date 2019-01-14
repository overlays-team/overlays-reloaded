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

   // Laser inputLaserFrontLastFrame;
    //Laser inputLaserBackLastFrame;

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
            if (Vector3.Angle(laser.laserOutput.forward, transform.forward)<85)
            {
                if(!laser.isMovingFast) inputLasersBackThisFrame.Add(laser);
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
            outputLaserFront.active = false;
        }
        else if (inputLasersFrontThisFrame.Count == 1)
        {
            /*Vector3 intersection;
            bool ist = LineLineIntersection(out intersection,
                inputLasersFrontThisFrame[0].laserOutput.position, inputLasersFrontThisFrame[0].laserOutput.forward,
                transform.position - transform.right * 2, (transform.right * 2));

            outputLaserFront.transform.position = new Vector3(intersection.x, outputLaserBack.transform.position.y, outputLaserFront.transform.position.z);
            */
            outputLaserFront.laser.image = inputLasersFrontThisFrame[0].image;
            outputLaserFront.transform.forward = Vector3.Reflect(inputLasersFrontThisFrame[0].laserOutput.forward, transform.forward);
            outputLaserFront.active = true;

            //inputLaserFrontLastFrame = inputLasersFrontThisFrame[0];
        }

        //back
        if (inputLasersBackThisFrame.Count == 0)
        {
            outputLaserBack.active = false;
        }
        else if (inputLasersBackThisFrame.Count == 1)
        {
            outputLaserBack.laser.image = inputLasersBackThisFrame[0].image;
            outputLaserBack.transform.forward = Vector3.Reflect(inputLasersBackThisFrame[0].laserOutput.forward, -transform.forward);
            outputLaserBack.active = true;

            //inputLaserBackLastFrame = inputLasersBackThisFrame[0];
        }

        
    }

    /*
    //returns the laserHitPoint directly on the mirror, without moving it to the side
    //calculates the crossection between teh laser an the mirror
    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {
        //iognore the y values
        linePoint1.y = 0;
        linePoint2.y = 0;

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parrallel
        if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }
    */
}
