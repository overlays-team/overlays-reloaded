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

    //inputLaser[0] is on the forwrd side of the mirror, inputLaser[1] on the backside

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
}
