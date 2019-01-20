using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : BlockObject
{
    // Update is called once per frame
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

        UpdateOutputImageDisplayAndSendThroughLaser();
    }

    protected override Color ProcessPixel(int x, int y)
    {
        return new Color(Mathf.Abs(1-inputImage1.GetPixel(x, y).r), Mathf.Abs(1 - inputImage1.GetPixel(x, y).g), Mathf.Abs(1 - inputImage1.GetPixel(x, y).b), inputImage1.GetPixel(x, y).a);
    }
}
