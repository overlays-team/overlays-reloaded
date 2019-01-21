using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageMultiOutput : ImageOutput
{

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
            else if (activeLasers.Count == 1)
            {
                debugImage.sprite = Sprite.Create(activeLasers[0].inputLaser.image, new Rect(0, 0, activeLasers[0].inputLaser.image.width, activeLasers[0].inputLaser.image.height), new Vector2(0.5f, 0.5f));
                CheckIfImageIsCorrect(activeLasers[0].inputLaser.image);
            }
            else
            {
                inputImage1 = null;
                inputImage2 = null;

                debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
                frame.SetColors(Color.red, Color.red);
                imageCorrectGlitterEffect.SetActive(false);
                imageCorrect = false;
            }
        }

        if (imageProcessingState != ImageProcessingState.Displaying)
        {
            if (imageProcessingState == ImageProcessingState.Ready)
            {
                debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, outputImage.width, outputImage.height), new Vector2(0.5f, 0.5f));

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
                    frame.SetColors(Color.red, Color.red);
                    imageCorrectGlitterEffect.SetActive(false);
                }
            }
        }


        //export function to get a goal image
        if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftControl))
        {
            if (outputImage!=null)ExportImage(outputImage);
        }
    }

    protected override Color ProcessPixel(int x, int y)
    {
        return new Color(
                        1 - (1 - inputImage1.GetPixel(x, y).r) * (1 - inputImage2.GetPixel(x, y).r) / 1,
                        1 - (1 - inputImage1.GetPixel(x, y).g) * (1 - inputImage2.GetPixel(x, y).g) / 1,
                        1 - (1 - inputImage1.GetPixel(x, y).b) * (1 - inputImage2.GetPixel(x, y).b) / 1
                        );
    }

}
