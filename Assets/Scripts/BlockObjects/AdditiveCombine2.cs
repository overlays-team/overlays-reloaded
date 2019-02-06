using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class AdditiveCombine2 : BlockObject
{
    /*
    * takes 2 lasers as input and gives one as output, which was belended using "Photoshop Screen",
    * take a closer look at BlockObject to understand everything
    */

    float image1Weight = 1;
    float image2Weight = 1;

    //for image processing
    Color pixel1;
    Color pixel2;

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

        UpdateOutputImageDisplayAndSendImageThroughLaser();
    }

    protected override Color ProcessPixel(int x, int y)
    {

        if (inputImage2.width > inputImage1.width)
        {
            pixel1 = inputImage1.GetPixel((int)(x / resolutionDifference), (int)(y / resolutionDifference));
            pixel2 = inputImage2.GetPixel(x, y);  
        }
        else
        {
            pixel1 = inputImage1.GetPixel(x, y);
            pixel2 = inputImage2.GetPixel((int)(x / resolutionDifference), (int)(y / resolutionDifference));
        }

        //screen blending / additive color blending
        return new Color(
                        1 - (1 - (pixel1.a * image1Weight * pixel1.r)) * (1 - (pixel2.a * image2Weight * pixel2.r)) / 1,
                        1 - (1 - (pixel1.a * image1Weight * pixel1.g)) * (1 - (pixel2.a * image2Weight * pixel2.g)) / 1,
                        1 - (1 - (pixel1.a * image1Weight * pixel1.b)) * (1 - (pixel2.a * image2Weight * pixel2.b)) / 1,
                        Mathf.Max(pixel1.a, inputImage2.GetPixel(x, y).a)
                        );


    }

    //gets called by the slider in the detailed node view, updates the weight of the two images
    public void UpdateImageWeight(Slider slider)
    {
        image1Weight = 1 + slider.value;
        image2Weight = 1 + (1 - slider.value);
        if (inputImage1 != null && inputImage2 != null) StartImageProcessing();
    }

        
}
