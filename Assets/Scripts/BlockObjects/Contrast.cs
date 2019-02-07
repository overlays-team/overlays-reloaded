using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Contrast : BlockObject
{
    /*
     * This class implements block that changes contrast
     */

    [SerializeField]
    //by value 1, contast of the image doesn't change; by value 2 contrast image is double in contrast
    private float contrastValue = 2;

    //laser logic
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

    //adding contrast to image depending on contrast value
    protected override Color ProcessPixel(int x, int y)
    {

        Color pixel = inputImage1.GetPixel(x, y);
        int r = (int)(pixel.r * 255);
        int g = (int)(pixel.g * 255);
        int b = (int)(pixel.b * 255);

        r = (int)(contrastValue * (r - 127.5) + 127.5);
        g = (int)(contrastValue * (g - 127.5) + 127.5);
        b = (int)(contrastValue * (b - 127.5) + 127.5);

        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, pixel.a);
    }

    #region only for sandbox mode, set the slider values
    public void SetContrastValue(Slider slider)
    {
        contrastValue = slider.value;
        StartImageProcessing();
    }
    #endregion
}
