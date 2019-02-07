using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Saturation : BlockObject
{
    /*
     * This class implements block that changes saturation
     */
    [SerializeField]
    //by value 1, saturation of the image doesn't change; by value 2 saturation of the image is doubled
    private float saturationValue = 2;

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

    //implementation of staturation
    protected override Color ProcessPixel(int x, int y)
    {
        Color pixel = inputImage1.GetPixel(x, y);

        float h;
        float s;
        float v;

        Color.RGBToHSV(pixel, out h, out s, out v);

        s = s * saturationValue;

        Color result = Color.HSVToRGB(h, s, v);
        result.a = pixel.a;

        return result;
    }

    #region only for sandbox mode, set the slider values
    public void SetSaturationValue(Slider slider)
    {
        saturationValue = slider.value;
        StartImageProcessing();
    }
    #endregion
}
