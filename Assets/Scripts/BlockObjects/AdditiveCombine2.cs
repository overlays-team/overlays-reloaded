using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditiveCombine2 : BlockObject
{
    [SerializeField]
    private Color activecolor = new Color(1, 0, 0); //if laser output is active
    [SerializeField]
    private Color inactivecolor = new Color(1, 1, 1);//if laser output is inactive

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
                ChangeMaterial(activecolor);

            }
            else
            {
                inputImage1 = null;
                inputImage2 = null;
                Shrink();
                StopImageProcessing();
                ChangeMaterial(inactivecolor);
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
        return new Color(
                        1 - (1 - (image1Weight * inputImage1.GetPixel(x, y).r)) * (1 - (image2Weight * inputImage2.GetPixel(x, y).r)) / 1,
                        1 - (1 - (image1Weight * inputImage1.GetPixel(x, y).g)) * (1 - (image2Weight *  inputImage2.GetPixel(x, y).g)) / 1,
                        1 - (1 - (image1Weight * inputImage1.GetPixel(x, y).b)) * (1 - (image2Weight *  inputImage2.GetPixel(x, y).b)) / 1
                        );
    }

    //gets called by the slider in the detsiled node view, updates the weight of the two images
    public void UpdateImageWeight(Slider slider)
    {
        Debug.Log(slider.value);
        image1Weight = slider.value * 2;
        image2Weight = (1 - slider.value) * 2;
        StartImageProcessing();
    }

    void ChangeMaterial(Color emissioncolor)
    {
        if (this.transform.childCount == 0)
        {
            return;
        }
        foreach (Transform child in graphics.transform)
        {
            GameObject gochild = child.gameObject;
            LineRenderer renderer = gochild.GetComponent<LineRenderer>();
            if (renderer != null)
            {
                renderer.endColor = emissioncolor;
                renderer.startColor = emissioncolor;
            }
        }
    }
}
