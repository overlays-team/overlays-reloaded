using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mask : BlockObject
{
    [Tooltip("assign the mask here - the mask takes the r value for alpha blending")]
    public Texture2D mask;

    protected override void Start()
    {
        base.Start();
        blockImage.GetComponent<Image>().sprite = Sprite.Create(mask, new Rect(0, 0, mask.width, mask.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
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
        return new Color(
            Mathf.Abs(inputImage1.GetPixel(x, y).r),
            Mathf.Abs(inputImage1.GetPixel(x, y).g),
            Mathf.Abs(inputImage1.GetPixel(x, y).b), 
            mask.GetPixel(x, y).r);
    }
}
