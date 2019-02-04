using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditiveCombine2 : BlockObject
{

    //takes 2 lasers as Input and gives one as output

    float image1Weight = 1;
    float image2Weight = 1;


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
        float resolutionDifference;
        Color pixel1;
        Color pixel2;

        if (inputImage2.width > inputImage1.width)
        {
            resolutionDifference = inputImage2.width / inputImage1.width;

            pixel1 = inputImage1.GetPixel((int)(x / resolutionDifference), (int)(y / resolutionDifference));
            pixel2 = inputImage2.GetPixel(x, y);  
        }
        else
        {
            resolutionDifference = inputImage1.width / inputImage2.width;

            pixel1 = inputImage1.GetPixel(x, y);
            pixel2 = inputImage2.GetPixel((int)(x / resolutionDifference), (int)(y / resolutionDifference));
        }

        return new Color(
                        1 - (1 - (pixel1.a * image1Weight * pixel1.r)) * (1 - (pixel2.a * image2Weight * pixel2.r)) / 1,
                        1 - (1 - (pixel1.a * image1Weight * pixel1.g)) * (1 - (pixel2.a * image2Weight * pixel2.g)) / 1,
                        1 - (1 - (pixel1.a * image1Weight * pixel1.b)) * (1 - (pixel2.a * image2Weight * pixel2.b)) / 1,
                        Mathf.Max(pixel1.a, inputImage2.GetPixel(x, y).a)
                        );


    }

    //gets called by the slider in the detsiled node view, updates the weight of the two images
    public void UpdateImageWeight(Slider slider)
    {
        Debug.Log(slider.value);
        //image1Weight = slider.value * 2;
        image1Weight = 1 + slider.value;
        image2Weight = 1 + (1 - slider.value);
        //image2Weight = (1 - slider.value) * 2;
        if (inputImage1 != null && inputImage2 != null) StartImageProcessing();
    }

        
}
