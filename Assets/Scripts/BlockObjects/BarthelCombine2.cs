using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarthelCombine2 : BlockObject
{
    /*
     * not used in the final game - block equivalent of the timeAttackGoalBLock
     */

    //takes 2 lasers as Input and gives one as output

    float image1Weight = 0.5f;
    float image2Weight = -0.5f;

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
        return new Color(
            Mathf.Max(0,(inputImage1.GetPixel(x, y).r + inputImage2.GetPixel(x, y).r) - 0.5f),
            Mathf.Max(0, (inputImage1.GetPixel(x, y).g + inputImage2.GetPixel(x, y).g) - 0.5f),
            Mathf.Max(0, (inputImage1.GetPixel(x, y).b + inputImage2.GetPixel(x, y).b) - 0.5f),
            Mathf.Max(inputImage1.GetPixel(x, y).a, inputImage2.GetPixel(x, y).a)
        );
    }

    //gets called by the slider in the detsiled node view, updates the weight of the two images
    public void UpdateImageWeight(Slider slider)
    {
        Debug.Log(slider.value);
        image1Weight = slider.value * 2;
        image2Weight = (1 - slider.value) * 2;
        if (inputImage1 != null && inputImage2 != null) StartImageProcessing();
    }
}
