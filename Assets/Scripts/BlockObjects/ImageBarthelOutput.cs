using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageBarthelOutput : ImageOutput
{
    /*
     * this imageOutput can be hit with a laser from any side, if it has 2 lasers, it blends the 2 images together with the image blending technique used in the previous game,
     * sadly this does not work with pixel values of 0-255, so we dont do it correctly here
     */

    protected override void ImageOutputUpdate()
    {
        if (lasersChanged)
        {
            imageCorrect = false;
            frame.SetColors(Color.red, Color.red);
            imageCorrectGlitterEffect.SetActive(false);
            StopCoroutine("ImageCheckingEnumerator");
            imageCheckingState = ImageCheckingState.NoImage;
            StopImageProcessing();

            List<LaserInput> activeLasers = new List<LaserInput>();
            foreach (LaserInput laserInput in laserInputs)
            {
                if (laserInput.active)
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
            else
            {
                inputImage1 = null;
                inputImage2 = null;

                debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
                detailedNodeViewImage.sprite = debugImage.sprite;
                frame.SetColors(Color.red, Color.red);
                imageCorrectGlitterEffect.SetActive(false);
                imageCorrect = false;

                imageProcessingState = ImageProcessingState.Displaying; // so we can always zoom in
            }
        }

        if (imageProcessingState != ImageProcessingState.Displaying)
        {
            if (imageProcessingState == ImageProcessingState.Ready)
            {
                //start checking if our image is processed
                imageCheckingState = ImageCheckingState.Checking;
                frame.SetColors(Color.yellow, Color.yellow);
                CheckIfImageIsCorrect(outputImage);
                imageProcessingState = ImageProcessingState.Displaying;
            }
        }

        if (imageCheckingState != ImageCheckingState.Displaying)
        {
            if (imageCheckingState == ImageCheckingState.Checked)
            {
                imageCheckingState = ImageCheckingState.Displaying;
                if (imageCorrect)
                {
                    debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
                    detailedNodeViewImage.sprite = debugImage.sprite;
                    frame.SetColors(Color.green, Color.green);
                    imageCorrectGlitterEffect.SetActive(true);
                    Instantiate(
                        imageCorrectBurstEffect,
                        new Vector3(transform.position.x,
                        imageCorrectBurstEffect.transform.position.y,
                        transform.position.z), imageCorrectBurstEffect.transform.rotation
                    );
                }
                else
                {
                    if (outputImage != null)
                    {
                        debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, outputImage.width, outputImage.height), new Vector2(0.5f, 0.5f));
                        detailedNodeViewImage.sprite = debugImage.sprite;
                    }
                    frame.SetColors(Color.red, Color.red);
                    imageCorrectGlitterEffect.SetActive(false);
                }
            }
        }


        //export function to get a goal image
        if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftControl))
        {
            if (outputImage != null) ExportImage(outputImage);
        }
    }

    protected override void CheckIfImageIsCorrect(Texture2D image)
    {
        imageToCheck = image;
        StartCoroutine("ImageCheckingEnumerator");
    }

    IEnumerator ImageCheckingEnumerator()
    {
        float biggestError = 0;
        int errorPixelCount = 0;

        for (int y = 0; y < imageToCheck.height; y++)
        {
            for (int x = 0; x < imageToCheck.width; x++)
            {
                Color color1 = imageToCheck.GetPixel(x, y);
                Color color2 = goalImage.GetPixel(x, y);

                if (Mathf.Abs(color2.r - color1.r) > biggestError)
                {
                    biggestError = Mathf.Abs(color2.r - color1.r);
                }
                if (Mathf.Abs(color2.g - color1.g) > biggestError)
                {
                    biggestError = Mathf.Abs(color2.g - color1.g);
                }
                if (Mathf.Abs(color2.b - color1.b) > biggestError)
                {
                    biggestError = Mathf.Abs(color2.b - color1.b);
                }

                if (Mathf.Abs(color2.b - color1.b) != 0 || Mathf.Abs(color2.r - color1.r) != 0 || Mathf.Abs(color2.g - color1.g) != 0)
                {
                    errorPixelCount++;
                }

            }
            if (y % imageProcessingTime == 0) yield return null;
        }
       // Debug.Log("biggestError: " + biggestError);
        //Debug.Log("errorPixelCount: " + errorPixelCount + " of " + 256 * 256);
        if (errorPixelCount > 61500)
        {
            imageCorrect = false;
        }
        else
        {
            imageCorrect = true;
        }

        imageCheckingState = ImageCheckingState.Checked;

    }

    protected override Color ProcessPixel(int x, int y)
    {
        Color pixel1 = inputImage1.GetPixel(x, y);
        Color pixel2 = inputImage2.GetPixel(x, y);

        return new Color(

                        Mathf.Min(Mathf.Max(0, (pixel1.r + pixel2.r) - 0.5f), 1),
                        Mathf.Min(Mathf.Max(0, (pixel1.g + pixel2.g) - 0.5f), 1),
                        Mathf.Min(Mathf.Max(0, (pixel1.b + pixel2.b) - 0.5f), 1),
                        Mathf.Max(pixel1.a, pixel2.a)
                        );
    }

}
