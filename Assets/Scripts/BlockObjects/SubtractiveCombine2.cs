using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtractiveCombine2 : BlockObject
{
    //takes 2 lasers as Input and gives one as output

    float image1Weight = 1;
    float image2Weight = 1;

    // for image processing
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

        UpdateOutputImageDisplayAndSendThroughLaser();
    }

    protected override void StartImageProcessing()
    {
        base.StartImageProcessing();
        if (inputImage1.width != inputImage2.width) Debug.Log("different resolutions of the images we want to screen/overlay!");
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


        //how to use image weight and the alpha values in multiply?
        if (inputImage1.GetPixel(x, y).a > 0 && inputImage2.GetPixel(x, y).a > 0)
        {
            return new Color(
                        (pixel1.r) * (pixel2.r) / 1,
                        (pixel1.g) * (pixel2.g) / 1,
                        (pixel1.b) * (pixel2.b) / 1
                        );
        }
        else if(inputImage1.GetPixel(x, y).a > 0)
        {
            return new Color(
                        (pixel1.r) ,
                        (pixel1.g) ,
                        (pixel1.b)
                        );
        }
        else if(inputImage2.GetPixel(x, y).a > 0)
        {
            return new Color(
                        (pixel2.r),
                        (pixel2.g),
                        (pixel2.b)
                        );
        }
        else
        {
            return new Color(0f, 0f, 0f, 0f);
        }



    }

    //gets called by the slider in the detsiled node view, updates the weight of the two images
    public void UpdateImageWeight(Slider slider)
    {
        Debug.Log(slider.value);
        image1Weight = slider.value * 2;
        image2Weight = (1 - slider.value) * 2;
        if(inputImage1!=null && inputImage2!=null) StartImageProcessing();
    }
}
