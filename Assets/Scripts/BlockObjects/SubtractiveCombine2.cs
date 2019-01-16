using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtractiveCombine2 : BlockObject
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
        //how to use image weight and the alpha values in multiply?
        if(inputImage1.GetPixel(x, y).a > 0 && inputImage2.GetPixel(x, y).a > 0)
        {
            return new Color(
                        (inputImage1.GetPixel(x, y).r) * (inputImage2.GetPixel(x, y).r) / 1,
                        (inputImage1.GetPixel(x, y).g) * (inputImage2.GetPixel(x, y).g) / 1,
                        (inputImage1.GetPixel(x, y).b) * (inputImage2.GetPixel(x, y).b) / 1
                        );
        }
        else if(inputImage1.GetPixel(x, y).a > 0)
        {
            return new Color(
                        (inputImage1.GetPixel(x, y).r) ,
                        (inputImage1.GetPixel(x, y).g) ,
                        (inputImage1.GetPixel(x, y).b)
                        );
        }
        else if(inputImage2.GetPixel(x, y).a > 0)
        {
            return new Color(
                        (inputImage2.GetPixel(x, y).r),
                        (inputImage2.GetPixel(x, y).g),
                        (inputImage2.GetPixel(x, y).b)
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
