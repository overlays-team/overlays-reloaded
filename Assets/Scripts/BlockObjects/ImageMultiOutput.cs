using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageMultiOutput : ImageOutput
{
    /*
     * finally not used in the game, is the equivalent to the goal block in the TimeAttack mode but with another blending algorythm
     */

    protected override void ImageOutputUpdate()
    {
        if (lasersChanged)
        {
            imageCorrect = false;
            frame.colorGradient = imageWrongGradient;
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
                detailedNodeViewImage.sprite = debugImage.sprite;
                CheckIfImageIsCorrect(activeLasers[0].inputLaser.image);
            }
            else
            {
                inputImage1 = null;
                inputImage2 = null;

                debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
                detailedNodeViewImage.sprite = debugImage.sprite;
                frame.colorGradient = imageWrongGradient;
                imageCorrectGlitterEffect.SetActive(false);
                imageCorrect = false;
            }
        }

        if (imageProcessingState != ImageProcessingState.Displaying)
        {
            if (imageProcessingState == ImageProcessingState.Ready)
            {
                debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, outputImage.width, outputImage.height), new Vector2(0.5f, 0.5f));
                detailedNodeViewImage.sprite = debugImage.sprite;

                //start checking if our image is processed
                imageCheckingState = ImageCheckingState.Checking;
                frame.colorGradient = imageProcessingGradient;
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
                    frame.colorGradient = imageCorrectGradient;
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
                    frame.colorGradient = imageWrongGradient;
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
        Color pixel1 = inputImage1.GetPixel(x, y);
        Color pixel2 = inputImage2.GetPixel(x, y);

        return new Color(
            1 - (1 - pixel1.r) * (1 - pixel2.r) / 1,
            1 - (1 - pixel1.g) * (1 - pixel2.g) / 1,
            1 - (1 - pixel1.b) * (1 - pixel2.b) / 1
        );
    }

}
