using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageInput : BlockObject
{
    [Header("Image Input")]
    [SerializeField]
    protected Texture2D inputImage;
    [SerializeField]
    [Tooltip("Which texture format will we be working with in the game - every image needs to be importet in this format to prevent pixel errors")]
    protected TextureFormat textureFormat = TextureFormat.RGBA32;

    protected override void Start()
    {
        base.Start();

        SetUpImage(inputImage);
    }

    //convert the picture into the RGBA32 texture format and set it up
    protected void SetUpImage(Texture2D image)
    {
        Texture2D outputImage = new Texture2D(image.width, image.height, textureFormat, false);
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
}
