﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageOutput : BlockObject {

    //wenn wir nur einen ImageInput haben wollen das:
    Texture2D inputImage;

    public Texture2D goalImage;

    public bool imageCorrect; // for the ingameManager, so he knows


    protected override void Start()
    {
        base.Start();
        inputImage = null;
        //debugImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
        debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
        frame.SetColors(Color.red, Color.red);
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        ImageOutputUpdate();

        //export function to get a goal image
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExportCurrentImage();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(CheckIfImageIsCorrect(inputImage));
        }
    }

    protected virtual void ImageOutputUpdate()
    {
        if (lasersChanged)
        {
            if (laserInputs[0].active)
            {
                inputImage = laserInputs[0].inputLaser.image;
                if (CheckIfImageIsCorrect(inputImage))
                {
                    //debugImage.sprite = Sprite.Create(yepImage, new Rect(0, 0, yepImage.width, yepImage.height), new Vector2(0.5f, 0.5f));
                    imageCorrect = true;
                    frame.SetColors(Color.green, Color.green);
                }
                else
                {
                    //debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
                    imageCorrect = false;
                    frame.SetColors(Color.red, Color.red);
                }
            }
            else
            {
                //debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
                frame.SetColors(Color.red, Color.red);
                inputImage = null;
                imageCorrect = false;
            }
        }
    }

    protected bool CheckIfImageIsCorrect(Texture2D image)
    {
        float biggestError = 0;
        bool isCorrect = true;
        for (int y = 0; y < image.height; y++)
        {
            for (int x = 0; x < image.width; x++)
            {
                Color color1 = image.GetPixel(x, y);
                Color color2 = goalImage.GetPixel(x, y);

                if (Mathf.Abs(color2.r - color1.r) > biggestError) biggestError = Mathf.Abs(color2.r - color1.r);
                if (Mathf.Abs(color2.g - color1.g) > biggestError) biggestError = Mathf.Abs(color2.g - color1.g);
                if (Mathf.Abs(color2.b - color1.b) > biggestError) biggestError = Mathf.Abs(color2.b - color1.b);

            }
        }
        //Debug.Log(biggestError);
        if (biggestError > 0) isCorrect = false;
        return isCorrect;
    }

   protected virtual void ExportCurrentImage()
    {
        if (inputImage != null)
        {
            byte[] bytes = inputImage.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../Assets/Images/Exports/SavedScreen.png", bytes);
        }
        else
        {
            Debug.Log("input image is Null");
        }
    }
}
