using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Saturation : BlockObject
{
    [SerializeField]
    private float saturationValue = 2;

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

        float h;
        float s;
        float v;

        Color.RGBToHSV(pixel, out h, out s, out v);

        s = s * saturationValue;

        Color result = Color.HSVToRGB(h, s, v);
        result.a = pixel.a;

        return result;
    }

    public void SetSaturationValue(Slider slider)
    {
        saturationValue = slider.value;
        StartImageProcessing();
    }
}
