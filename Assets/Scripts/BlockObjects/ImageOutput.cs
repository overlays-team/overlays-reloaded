using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageOutput : BlockObject {

    //wenn wir nur einen ImageInput haben wollen das:
    [SerializeField]
    LaserInput laserInput;
    Laser inputLaser;
    Texture2D inputImage;

    // Update is called once per frame
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
                    inputLaser = laser;

                    Debug.Log("we got an image");
                    //jetzt müssten wir irgendwie schauen ob input Image richtig ist
                }
            }
        }
    }
}
