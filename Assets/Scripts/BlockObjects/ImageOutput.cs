using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class ImageOutput : BlockObject {
    
    /*
     * this is our standard goalBLock, if all goalBlocks get a laser with an image, which is the same as their goalImage , we win the level
     */


    [Header("Image Output")]
    [SerializeField]
    [Tooltip("which image needs to be passed to this object to win?")]
    protected Texture2D goalImage;

    protected Texture2D imageToCheck; //which image are we currently checking for correctness

    [Tooltip("this bool tells the gameManager if we suceeded with this outputImage")]
    public bool imageCorrect;

    public GameObject imageCorrectBurstEffect;
    public GameObject imageCorrectGlitterEffect;

    //in which imageCheckingstate are we - are we currently checking if this image is correct or ...
    protected enum ImageCheckingState
    {
        NoImage,
        Checking,
        Checked,
        Displaying
    }

    protected ImageCheckingState imageCheckingState = ImageCheckingState.NoImage;


    protected override void Start()
    {
        base.Start();
        SetupImageOutput(goalImage);
        imageProcessingState = ImageProcessingState.Displaying; // we set this here so we can always zoom in the cetailed node view
    }

    public void SetupImageOutput(Texture2D _goalImage)
    {
        goalImage = _goalImage;
        inputImage1 = null;
        debugImage.sprite = Sprite.Create(goalImage, new Rect(0, 0, goalImage.width, goalImage.height), new Vector2(0.5f, 0.5f));
        if (detailedNodeViewImage != null)
        {
            detailedNodeViewImage.sprite = debugImage.sprite;
        }
        frame.SetColors(Color.red, Color.red);
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        ImageOutputUpdate();
    }

    protected virtual void ImageOutputUpdate()
    {
        if (lasersChanged)
        {
            imageCorrect = false;
            frame.SetColors(Color.red, Color.red);
            StopCoroutine("ImageCheckingEnumerator");
            imageCheckingState = ImageCheckingState.NoImage;
            imageCorrectGlitterEffect.SetActive(false);

            if (laserInputs[0].active)
            {
                inputImage1 = laserInputs[0].inputLaser.image;
                imageCheckingState = ImageCheckingState.Checking;
                frame.SetColors(Color.yellow, Color.yellow);
                CheckIfImageIsCorrect(inputImage1);
            }
            else
            {
                inputImage1 = null;
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
            if(inputImage1!=null)ExportImage(inputImage1);
        }
    }

    protected virtual void CheckIfImageIsCorrect(Texture2D image)
    {
        imageToCheck = image;
        StartCoroutine("ImageCheckingEnumerator");
    }

    IEnumerator ImageCheckingEnumerator()
    {
        float biggestError = 0;

        for (int y = 0; y < imageToCheck.height; y++)
        {
            for (int x = 0; x < imageToCheck.width; x++)
            {
                Color color1 = imageToCheck.GetPixel(x, y);
                Color color2 = goalImage.GetPixel(x, y);

                if (Mathf.Abs(color2.r - color1.r) > biggestError) biggestError = Mathf.Abs(color2.r - color1.r);
                if (Mathf.Abs(color2.g - color1.g) > biggestError) biggestError = Mathf.Abs(color2.g - color1.g);
                if (Mathf.Abs(color2.b - color1.b) > biggestError) biggestError = Mathf.Abs(color2.b - color1.b);

            }
            if (y % imageProcessingTime == 0) yield return null;
        }

        if (biggestError > 0.01)
        {
            imageCorrect = false;
        }
        else
        {
            imageCorrect = true;
        }

        imageCheckingState = ImageCheckingState.Checked;

    }

    protected virtual void ExportImage(Texture2D imageToExport)
    {
        Debug.Log("Export");
        if (imageToExport != null)
        {
            byte[] bytes = imageToExport.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../Assets/Images/Exports/SavedScreen_" + SceneManager.GetActiveScene().name + "_" + gameObject.name +".png", bytes);
        }
        else
        {
            Debug.Log("input image is Null");
        }
    }
}
