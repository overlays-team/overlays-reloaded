using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFilter : BlockObject
{
    [SerializeField]
    private LaserInput laserInput;
    private Laser inputLaser;
    [SerializeField]
    private LaserOutput laserOutput;

    bool imageReady;
    bool imageInProcess;
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


        //inputLaser updaten und image Processing starten oder stoppen
        if (inputLasers.Count == 0)
        {
            inputLaser = null;
            StopImageProcessing();
        }
        else
        {
            bool inputLaserFound = false;
            foreach (Laser laser in inputLasers)
            {
                //check if one of our input Lasers hits the input
                if (Vector3.Angle(laser.laserOutput.forward, laserInput.transform.forward) < 5)
                {
                    inputLaserFound = true;
                    if (inputLaser == null || inputLaser != laser)
                    {
                        inputLaser = laser;
                        Debug.Log("start imageProcessing");
                        StartImageProcessing();
                    }
                }
            }
            if (!inputLaserFound)
            {
                inputLaser = null;
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
        if (imageInProcess) Debug.Log("imape in process");
    }


       
    void StartImageProcessing()
    {
        //hier kommt der Image Processing Code: zum beispiel Rot Filter
        inputImage = inputLaser.image;
        outputImage = Instantiate(inputImage);
        imageReady = false;
        imageInProcess = true;
        StartCoroutine("RedFilterEnumerator");
    }

    //is called when the lasr leaves the node - > active image processing is stoppen and the image is deleted
    void StopImageProcessing()
    {
        outputImage = null;
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
        laserOutput.laser.image = outputImage;
        imageInProcess = false;
        imageReady = true;

    }
}
