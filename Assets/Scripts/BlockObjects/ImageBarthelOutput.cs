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

    [Tooltip("this is the sum of the two inputimages ID's")]
    public int goalImageID;

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

            //gather all input lasers 
            List<LaserInput> activeLasers = new List<LaserInput>();
            foreach (LaserInput laserInput in laserInputs)
            {
                if (laserInput.active)
                {
                    activeLasers.Add(laserInput);
                }
            }

            if(activeLasers.Count == 1)
            {
                inputImage1 = activeLasers[0].inputLaser.image;

                if (activeLasers[0].inputLaser.imageID == goalImageID)
                {
                    imageCorrect = true;
                    imageCorrectGlitterEffect.SetActive(true);
                    frame.SetColors(Color.green, Color.green);
                    imageCorrectGlitterEffect.SetActive(true);
                    Instantiate(
                        imageCorrectBurstEffect,
                        new Vector3(transform.position.x,
                        imageCorrectBurstEffect.transform.position.y,
                        transform.position.z), imageCorrectBurstEffect.transform.rotation
                    );
                    debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
                    detailedNodeViewImage.sprite = debugImage.sprite;
                    imageProcessingState = ImageProcessingState.Displaying;
                }
                else
                {
                    imageCorrect = false;

                    frame.SetColors(Color.red, Color.red);
                    imageCorrectGlitterEffect.SetActive(false);
                    imageProcessingState = ImageProcessingState.Displaying;
                }
            }
            else if (activeLasers.Count == 2)
            {
                inputImage1 = activeLasers[0].inputLaser.image;
                inputImage2 = activeLasers[1].inputLaser.image;
               
                if(activeLasers[0].inputLaser.imageID + activeLasers[1].inputLaser.imageID == goalImageID)
                {
                    imageCorrect = true;
                    imageCorrectGlitterEffect.SetActive(true);
                    frame.SetColors(Color.green, Color.green);
                    imageCorrectGlitterEffect.SetActive(true);
                    Instantiate(
                        imageCorrectBurstEffect,
                        new Vector3(transform.position.x,
                        imageCorrectBurstEffect.transform.position.y,
                        transform.position.z), imageCorrectBurstEffect.transform.rotation
                    );
                    debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
                    detailedNodeViewImage.sprite = debugImage.sprite;
                    imageProcessingState = ImageProcessingState.Displaying;
                }
                else
                {
                    imageCorrect = false;
                    StartImageProcessing();

                    frame.SetColors(Color.red, Color.red);
                    imageCorrectGlitterEffect.SetActive(false);

                }
               
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
                if (outputImage != null)
                {
                    debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, outputImage.width, outputImage.height), new Vector2(0.5f, 0.5f));
                    detailedNodeViewImage.sprite = debugImage.sprite;
                }
                imageProcessingState = ImageProcessingState.Displaying;
            }
        }

        //export function to get a goal image
        if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftControl))
        {
            if (outputImage != null) ExportImage(outputImage);
        }
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
