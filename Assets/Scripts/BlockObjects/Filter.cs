using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : BlockObject
{

    public enum FilterColor { RED, GREEN, BLUE, NONE };
    public FilterColor filterMode = FilterColor.NONE;


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
        switch (filterMode)
        {
            case FilterColor.RED:
                return new Color(inputImage1.GetPixel(x, y).r, 0f, 0f);

            case FilterColor.GREEN:
                return new Color(0f, inputImage1.GetPixel(x, y).g, 0f);

            case FilterColor.BLUE:
                return new Color(0f, 0f, inputImage1.GetPixel(x, y).b);

            case FilterColor.NONE:
                return inputImage1.GetPixel(x, y);

            default:
                return new Color();
        }
    }
}
