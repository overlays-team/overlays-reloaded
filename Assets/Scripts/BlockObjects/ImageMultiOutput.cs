using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageMultiOutput : ImageOutput
{

    //for image processing
    Texture2D inputImage1;
    Texture2D inputImage2;
    Texture2D outputImage;

    protected override void Start()
    {
        base.Start();
        frame.SetColors(Color.red, Color.red);
    }

    // Update is called once per frame
    protected override void ImageOutputUpdate()
    { 
        if (lasersChanged)
        {
            imageReady = false;
            List<LaserInput> activeLasers = new List<LaserInput>();
            foreach (LaserInput laserInput in laserInputs)
            {
                if(laserInput.active)
                {
                    activeLasers.Add(laserInput);
                }
            }

            if (activeLasers.Count >= 2)
            {
                inputImage1 = activeLasers[0].inputLaser.image;
                inputImage2 = activeLasers[1].inputLaser.image;
                StartImageProcessing();
            }
            else if(activeLasers.Count == 1)
            {
                StopImageProcessing();
                debugImage.sprite = Sprite.Create(activeLasers[0].inputLaser.image, new Rect(0, 0, activeLasers[0].inputLaser.image.width, activeLasers[0].inputLaser.image.height), new Vector2(0.5f, 0.5f));
                if (CheckIfImageIsCorrect(activeLasers[0].inputLaser.image))
                {
                    //validityImage.sprite = Sprite.Create(yepImage, new Rect(0, 0, yepImage.width, yepImage.height), new Vector2(0.5f, 0.5f));
                    frame.SetColors(Color.green, Color.green);
                    imageCorrect = true;
                }
                else
                {
                    // validityImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
                    frame.SetColors(Color.red, Color.red);
                    imageCorrect = false;
                }
            }
            else
            {
                inputImage1 = null;
                inputImage2 = null;
                StopImageProcessing();

                debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
                //validityImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
                frame.SetColors(Color.red, Color.red);
                imageCorrect = false;
            }
        }

        if (imageReady && !imageDisplaying)
        {

            imageDisplaying = true;
            debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, yepImage.width, yepImage.height), new Vector2(0.5f, 0.5f));
            if (CheckIfImageIsCorrect(outputImage))
            {
                //validityImage.sprite = Sprite.Create(yepImage, new Rect(0, 0, yepImage.width, yepImage.height), new Vector2(0.5f, 0.5f));
                frame.SetColors(Color.green, Color.green);
                imageCorrect = true;
            }
            else if  (!imageReady)
            {
                //validityImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
                frame.SetColors(Color.red, Color.red);
                imageCorrect = false;
            }
        }

    }

    

    protected override void StartImageProcessing()
    {
        outputImage = Instantiate(inputImage1); // wir erstellen uns ein neues output Image - welches eine Kopie eines Inputs ist, wird soweiso gleih überschrieben - könnte man schlauer lösen
        if (inputImage1.width != inputImage2.width) Debug.Log("different resolutions!");

        base.StartImageProcessing();
    }

    IEnumerator ImageProcessingEnumerator()
    {
        for (int y = 0; y < outputImage.height; y++)
        {
            for (int x = 0; x < outputImage.width; x++)
            {
                outputImage.SetPixel(x, y,
                   new Color(
                       1 - (1 - inputImage1.GetPixel(x, y).r) * (1 - inputImage2.GetPixel(x, y).r) / 1,
                       1 - (1 - inputImage1.GetPixel(x, y).g) * (1 - inputImage2.GetPixel(x, y).g) / 1,
                       1 - (1 - inputImage1.GetPixel(x, y).b) * (1 - inputImage2.GetPixel(x, y).b) / 1
                   ));
            }
            if (y % 10 == 0) yield return null;
        }
        outputImage.Apply();

        imageInProcess = false;
        imageReady = true;
    }
}
