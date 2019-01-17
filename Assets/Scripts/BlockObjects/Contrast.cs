using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contrast : BlockObject
{
    [SerializeField]
    private float contrastvalue = 2;

    protected override void Start()
    {
        base.Start();
        laserOutput.active = false;
    }

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

        Color pixel = inputImage1.GetPixel(x, y);
        int r = (int)(pixel.r * 255);
        int g = (int)(pixel.g * 255);
        int b = (int)(pixel.b * 255);

        r = (int)(contrastvalue * (r - 127.5) + 127.5);
        g = (int)(contrastvalue * (g - 127.5) + 127.5);
        b = (int)(contrastvalue * (b - 127.5) + 127.5);

        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}
