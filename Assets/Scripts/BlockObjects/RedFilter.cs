using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFilter : BlockObject
{
    [SerializeField]
    LaserOutput laserOutput;

    //for image processing
    Texture2D inputImage;
    Texture2D outputImage;


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
            if (laserInputs[0].active)
            {
                inputImage = laserInputs[0].inputLaser.image;
                StartImageProcessing();
            }
            else
            {
                inputImage = null;
                StopImageProcessing();
            }

        }

        if (imageReady)
        {
            laserOutput.active = true;

            //added with 13-graphics update
            debugImage.gameObject.SetActive(true);
            debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, outputImage.width, outputImage.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            debugImage.gameObject.SetActive(false);
            laserOutput.active = false;
        }

    }

    protected override void StartImageProcessing()
    {
        outputImage = Instantiate(inputImage);
        base.StartImageProcessing(); 
    }


    //könnte auch in die Vaterklasse verlagert werden, nur weiß ich nicht wie das schlau mit enumeratoren geht
    IEnumerator ImageProcessingEnumerator()
    {
        for (int y = 0; y < outputImage.height; y++)
        {
            for (int x = 0; x < outputImage.width; x++)
            {
                outputImage.SetPixel(x, y, new Color(outputImage.GetPixel(x, y).r, 0f, 0f));
            }
            if (y % 10 == 0) yield return null;
        }
        outputImage.Apply();
        
        imageInProcess = false;
        imageReady = true;

        laserOutput.laser.image = outputImage;
    }
}
