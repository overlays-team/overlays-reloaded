using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveCombine2 : BlockObject
{

    //takes 2 lasers as Input and gives one as output


    protected override void Update()
    {
        base.Update();

        if (lasersChanged)
        {
            imageProcessingState = ImageProcessingState.NoImage;
            if (laserInputs[0].active && laserInputs[1].active)
            {
                inputImage1 = laserInputs[0].inputLaser.image;
                inputImage2 = laserInputs[1].inputLaser.image;
                Grow();
                StartImageProcessing();

            }
            else
            {
                inputImage1 = null;
                inputImage2 = null;
                Shrink();
                StopImageProcessing();
            }

        }

        UpdateOutputImageDisplayAndSendThroughLaser();
    }

    protected override void StartImageProcessing()
    {
        base.StartImageProcessing();
        if (inputImage1.width != inputImage2.width) Debug.Log("different resolutions of the images we want to screen/overlay!");
    }

    protected override Color ProcessPixel(int x, int y)
    {
        return new Color(
                        1 - (1 - inputImage1.GetPixel(x, y).r) * (1 - inputImage2.GetPixel(x, y).r) / 1,
                        1 - (1 - inputImage1.GetPixel(x, y).g) * (1 - inputImage2.GetPixel(x, y).g) / 1,
                        1 - (1 - inputImage1.GetPixel(x, y).b) * (1 - inputImage2.GetPixel(x, y).b) / 1
                        );
    }

        
}
