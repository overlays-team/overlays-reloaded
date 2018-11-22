using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageOutput : BlockObject {

    //wenn wir nur einen ImageInput haben wollen das:
    Texture2D inputImage;

    public Texture2D noImage; //just a white texture we show when no image is present
    public Texture2D goalImage;

    protected override void Start()
    {
        base.Start();
        inputImage = null;
        debugImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (lasersChanged)
        {
            if (laserInputs[0].active)
            {
                inputImage = laserInputs[0].inputLaser.image;
                debugImage.sprite = Sprite.Create(inputImage, new Rect(0, 0, inputImage.width, inputImage.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                inputImage = null;
                debugImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
            }
        }

        //export function to get a goal image
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExportCurrentImage();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(CheckIfImageIsCorrect());
        }
    }

    bool CheckIfImageIsCorrect()
    {
        bool isCorrect = true;
        for (int y = 0; y < inputImage.height; y++)
        {
            for (int x = 0; x < inputImage.width; x++)
            {
                Color color1 = inputImage.GetPixel(x, y);
                Color color2 = goalImage.GetPixel(x, y);
                if (y == 1) { 
                Debug.Log("color1.r: " + color1.r);
                Debug.Log("color2.r: " + color2.r);
            }
                //Debug.Log("color1.b: " + color1.b);
                //Debug.Log("color2.b: " + color2.b);
                if (color1.r != color2.r)
                {
                    isCorrect = false;
                }
                else if  (color1.g != color2.g)
                {
                        isCorrect = false;
                }
                else if (color1.b != color2.b)
                {
                    isCorrect = false;
                }
            }
        }

        return isCorrect;
    }

    void ExportCurrentImage()
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
