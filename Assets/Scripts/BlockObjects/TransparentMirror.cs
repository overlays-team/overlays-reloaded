using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentMirror : BlockObject
{
    [Header("Mirror")]
    [Tooltip("needs to have the same forward direction as the mirror")]
    public LaserOutput[] outputLaserFronts;


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
            outputLaserFronts[0].gameObject.transform.forward = Vector3.Reflect(laserInputs[0].inputLaser.laserOutput.forward, transform.forward);
            outputLaserFronts[0].laser.image = laserInputs[0].inputLaser.image;
            outputLaserFronts[0].active = true;

            outputLaserFronts[1].gameObject.transform.forward = Vector3.Reflect(laserInputs[0].inputLaser.laserOutput.right, transform.forward);
            outputLaserFronts[1].laser.image = laserInputs[0].inputLaser.image;
            outputLaserFronts[1].active = true;
        }
        else
        {
            outputLaserFronts[0].laser.image = null;
            outputLaserFronts[0].active = false;

            outputLaserFronts[1].laser.image = null;
            outputLaserFronts[1].active = false;
        }

        if (laserInputs[1].active)
        {
            outputLaserFronts[2].gameObject.transform.forward = Vector3.Reflect(laserInputs[1].inputLaser.laserOutput.forward, transform.forward);
            outputLaserFronts[2].laser.image = laserInputs[1].inputLaser.image;
            outputLaserFronts[2].active = true;

            outputLaserFronts[3].gameObject.transform.forward = Vector3.Reflect(laserInputs[1].inputLaser.laserOutput.right, transform.right);
            outputLaserFronts[3].laser.image = laserInputs[1].inputLaser.image;
            outputLaserFronts[3].active = true;
        }
        else
        {
            outputLaserFronts[2].laser.image = null;
            outputLaserFronts[2].active = false;

            outputLaserFronts[3].laser.image = null;
            outputLaserFronts[3].active = false;
        }

        if (laserInputs[2].active)
        {
            outputLaserFronts[4].gameObject.transform.forward = Vector3.Reflect(laserInputs[2].inputLaser.laserOutput.forward, transform.forward);
            outputLaserFronts[4].laser.image = laserInputs[2].inputLaser.image;
            outputLaserFronts[4].active = true;

            outputLaserFronts[5].gameObject.transform.forward = Vector3.Reflect(laserInputs[2].inputLaser.laserOutput.right, transform.forward);
            outputLaserFronts[5].laser.image = laserInputs[2].inputLaser.image;
            outputLaserFronts[5].active = true;
        }
        else
        {
            outputLaserFronts[4].laser.image = null;
            outputLaserFronts[4].active = false;

            outputLaserFronts[5].laser.image = null;
            outputLaserFronts[5].active = false;
        }

        if (laserInputs[3].active)
        {
            outputLaserFronts[6].gameObject.transform.forward = Vector3.Reflect(laserInputs[3].inputLaser.laserOutput.forward, transform.forward);
            outputLaserFronts[6].laser.image = laserInputs[3].inputLaser.image;
            outputLaserFronts[6].active = true;

            outputLaserFronts[7].gameObject.transform.forward = Vector3.Reflect(laserInputs[3].inputLaser.laserOutput.right, transform.right);
            outputLaserFronts[7].laser.image = laserInputs[3].inputLaser.image;
            outputLaserFronts[7].active = true;
        }
        else
        {
            outputLaserFronts[6].laser.image = null;
            outputLaserFronts[6].active = false;

            outputLaserFronts[7].laser.image = null;
            outputLaserFronts[7].active = false;
        }
    }
}
