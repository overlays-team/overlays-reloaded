using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageMultiOutput : BlockObject
{

    //wenn wir nur einen ImageInput haben wollen das:
    Texture2D inputImage;

    //for image processing
    Texture2D inputImage1;
    Texture2D inputImage2;
    Texture2D outputImage;

    public Image validityImage;
    public Texture2D noImage; //just a white texture we show when no image is present
    public Texture2D goalImage;
    public Texture2D yepImage; //gets displayed when we get the goal image right

    public bool imageCorrect; // for the ingameManager, so he knows

    protected override void Start()
    {
        base.Start();
        inputImage = null;

        debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (lasersChanged)
        {
            imageReady = false;
            List<LaserInput> activeLasers = new List<LaserInput>();
            foreach (LaserInput laser in laserInputs)
            {
                if(laser.active)
                {
                    activeLasers.Add(laser);
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
                debugImage.sprite = Sprite.Create(activeLasers[0].inputLaser.image, new Rect(0, 0, activeLasers[0].inputLaser.image.width, activeLasers[0].inputLaser.image.height), new Vector2(0.5f, 0.5f));
                if (CheckIfImageIsCorrect(activeLasers[0].inputLaser.image))
                {
                    validityImage.sprite = Sprite.Create(yepImage, new Rect(0, 0, yepImage.width, yepImage.height), new Vector2(0.5f, 0.5f));
                    imageCorrect = true;
                }
                else
                {
                    validityImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
                    imageCorrect = false;
                }
            }
            else
            {
                inputImage1 = null;
                inputImage2 = null;
                StopImageProcessing();

                debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
                validityImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
                imageCorrect = false;
            }


        }

        if (imageReady)
        {
            debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, yepImage.width, yepImage.height), new Vector2(0.5f, 0.5f));

            if (CheckIfImageIsCorrect(outputImage))
            {
                validityImage.sprite = Sprite.Create(yepImage, new Rect(0, 0, yepImage.width, yepImage.height), new Vector2(0.5f, 0.5f));
                imageCorrect = true;
            }
            else
            {
                validityImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
                imageCorrect = false;
            }
        }
        else
        {

        }

        //export function to get a goal image
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExportCurrentImage();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Debug.Log(CheckIfImageIsCorrect());
        }
    }

    bool CheckIfImageIsCorrect(Texture2D image)
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
