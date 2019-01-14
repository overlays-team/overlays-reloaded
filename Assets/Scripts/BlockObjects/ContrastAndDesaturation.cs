using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrastAndDesaturation : BlockObject
{

    public enum ContrastOrDesat { CONTRAST, DESATURATION, NONE };
    [Header("Filter")]
    public ContrastOrDesat filterMode = ContrastOrDesat.NONE;


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
        switch (filterMode)
        {
            case ContrastOrDesat.CONTRAST:
                return new Color(2*inputImage1.GetPixel(x, y).r, 2 * inputImage1.GetPixel(x, y).g, 2 * inputImage1.GetPixel(x, y).b);

            case ContrastOrDesat.DESATURATION:
                return new Color(0.5f * inputImage1.GetPixel(x, y).r, 0.5f * inputImage1.GetPixel(x, y).g, 0.5f * inputImage1.GetPixel(x, y).b);

            case ContrastOrDesat.NONE:
                return inputImage1.GetPixel(x, y);

            default:
                return new Color();
        }
    }
}
