using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : BlockObject
{

    protected override void Update()
    {
        base.Update();

        if (lasersChanged)
        {
            imageProcessingState = ImageProcessingState.NoImage;
            if (laserInputs[0].active)
            {
                inputImage1 = laserInputs[0].inputLaser.image;
                Grow();
                StartImageProcessing();
            }
            else
            {
                inputImage1 = null;
                Shrink();
                StopImageProcessing();
            }
        }

        UpdateOutputImageDisplayAndSendImageThroughLaser();
    }

    protected override Color ProcessPixel(int x, int y)
    {
        Color pixel = inputImage1.GetPixel(x, y);

        return new Color(Mathf.Abs(1- pixel.r), Mathf.Abs(1 - pixel.g), Mathf.Abs(1 - pixel.b), pixel.a);
    }
}
