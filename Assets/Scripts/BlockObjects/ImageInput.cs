using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageInput : BlockObject
{
    [SerializeField]
    private LaserOutput laserOutput;
    [SerializeField]
    private Texture2D inputImage;

    public Image debugImage; //just for now

    protected override void Start()
    {
        base.Start();

        //convert the picture into the RGBA32 texture format
        Texture2D outputImage = new Texture2D(inputImage.width, inputImage.height, TextureFormat.RGBA32, false);
        for (int y = 0; y < outputImage.height; y++)
        {
            for (int x = 0; x < outputImage.width; x++)
            {
                outputImage.SetPixel(x, y, inputImage.GetPixel(x, y));
            }

        }
        outputImage.Apply();

        laserOutput.laser.image = outputImage;
        laserOutput.active = true;


        debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, outputImage.width, outputImage.height), new Vector2(0.5f, 0.5f));

    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.I)) ToogleDebugImage();
    }

    void ToogleDebugImage()
    {
        if (debugImage.gameObject.activeSelf)
        {
            debugImage.gameObject.SetActive(false);
        }
        else
        {
            debugImage.gameObject.SetActive(true);
        }
    }

}
