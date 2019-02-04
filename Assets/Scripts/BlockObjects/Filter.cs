using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Filter : BlockObject
{

    public enum FilterColor { RED, GREEN, BLUE, SANDBOX, NONE };
    [Header("Filter")]
    public FilterColor filterMode = FilterColor.NONE;

    //only for sandbox mode
    float red = 0.5f;
    float green = 0.5f;
    float blue = 0.5f;


    protected override void Start()
    {
        base.Start();
        laserOutput.active = false;

        /* falls wir den laser färben wollen
        switch (filterMode)
        {
            case FilterColor.RED:
                laserOutput.laser.GetComponent<LineRenderer>().SetColors(Color.red, Color.red);
                frame.SetColors(Color.red, Color.red);
                break;
            case FilterColor.GREEN:
                laserOutput.laser.GetComponent<LineRenderer>().SetColors(Color.green, Color.green);
                frame.SetColors(Color.green, Color.green);
                break;
            case FilterColor.BLUE:
                laserOutput.laser.GetComponent<LineRenderer>().SetColors(Color.blue, Color.blue);
                frame.SetColors(Color.blue, Color.blue);
                break;
            case FilterColor.NONE:
                laserOutput.laser.GetComponent<LineRenderer>().SetColors(Color.white, Color.white);
                frame.SetColors(Color.white, Color.white);
                break;
        }
        */
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

        switch (filterMode)
        {
            case FilterColor.RED:
                return new Color(pixel.r, 0f, 0f, pixel.a);

            case FilterColor.GREEN:
                return new Color(0f, pixel.g, 0f, pixel.a);

            case FilterColor.BLUE:
                return new Color(0f, 0f, pixel.b, pixel.a);

            case FilterColor.NONE:
                return pixel;

            case FilterColor.SANDBOX:
                return new Color(pixel.r * red, pixel.g * green, pixel.b * blue);

            default:
                return new Color();
        }
    }



    #region only for sandbox mode
    public void SetRed(Slider slider)
    {
        red = slider.value;
        StartImageProcessing();
    }
    public void SetGreen(Slider slider)
    {
        green = slider.value;
        StartImageProcessing();
    }
    public void SetBlue(Slider slider)
    {
       blue = slider.value;
       StartImageProcessing();
    }


    #endregion
}
