using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : BlockObject
{

    public LaserOutput outputLaserFront;
    public LaserOutput outputLaserBack;

    //inputLaser[0] is on the forwrd side of the mirror, inputLaser[1] on the backside
    //Laser inputLaserBack;
    //Laser inputLaserFront;

    protected override void Start()
    {
        base.Start();

        laserInputMaxIncidenceAngle = 85;
    }

    protected override void Update()
    {
        base.Update();


        if (laserInputs[0].active)
        {
            //int angleDifference = (int)Vector3.Angle(laserInputs[0].inputLaser.laserOutput.forward, transform.forward);
            outputLaserFront.gameObject.transform.forward = Vector3.Reflect(laserInputs[0].inputLaser.laserOutput.forward, transform.forward);
            outputLaserFront.laser.image = laserInputs[0].inputLaser.image;
            outputLaserFront.active = true;
        }
        else
        {
            outputLaserFront.laser.image = null;
            outputLaserFront.active = false;
        }
        if (laserInputs[1].active)
        {
            //int angleDifference = (int)Vector3.Angle(laserInputs[0].inputLaser.laserOutput.forward, transform.forward);
            outputLaserBack.gameObject.transform.forward = Vector3.Reflect(laserInputs[1].inputLaser.laserOutput.forward, transform.forward);
            outputLaserBack.laser.image = laserInputs[1].inputLaser.image;
            outputLaserBack.active = true;
        }
        else
        {
            outputLaserBack.laser.image = null;
            outputLaserBack.active = false;
        }
    }

    public override void ReturnToInventory()
    {
        outputLaserFront.laser.active = false;
        outputLaserBack.laser.active = false;
        base.ReturnToInventory();
    }
}
