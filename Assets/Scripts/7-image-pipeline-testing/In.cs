using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class In : MonoBehaviour {

    public Texture2D inputImage;
    public Image debugImage;

    //referenz auf den nächsten Node
    public ImageProcessor imageProcessor;

    void Start()
    {
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

        imageProcessor.SetInputImage(outputImage);
        debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, outputImage.width, outputImage.height), new Vector2(0.5f, 0.5f));
    }

}
