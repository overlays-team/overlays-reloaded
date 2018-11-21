using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFilterNew : BlockObject
{
    [SerializeField]
    LaserOutput laserOutput;

    //for image processing
    Texture2D inputImage;
    Texture2D outputImage;
    bool imageReady;
    bool imageInProcess;

    protected override void Start()
    {
        base.Start();
        laserOutput.active = false;
    }

    protected override void Update()
    {
        base.Update();

        //hier haben wir nur einen Input - trotzdem Schleife?
        /*
        foreach (LaserInput laserInput in laserInputs)
        {
            if (laserInput.active)
            {
                //Debug.Log("1");
                //wir starten das Image processing nur wenn sich das Bild geändert hat, oder davor null war
                if (inputImage == null)
                {
                    //Debug.Log("2");
                    inputImage = laserInput.inputLaser.image;
                    StartImageProcessing();
                }
            }
            else
            {
                inputImage = null;
                StopImageProcessing();
            }
           
        }*/
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
        }
        else
        {
            laserOutput.active = false;
        }
    }

    void StartImageProcessing()
    {
        //hier kommt der Image Processing Code: zum beispiel Rot Filter
        outputImage = Instantiate(inputImage);
        imageReady = false;
        imageInProcess = true;
        StartCoroutine("RedFilterEnumerator");
    }

    //is called when the lasr leaves the node - > active image processing is stoppen and the image is deleted
    void StopImageProcessing()
    {
        imageReady = false;
        imageInProcess = false;
        StopCoroutine("RedFilterEnumerator");
    }


    IEnumerator RedFilterEnumerator()
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
