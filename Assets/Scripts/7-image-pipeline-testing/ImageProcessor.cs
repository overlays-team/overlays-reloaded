using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageProcessor : MonoBehaviour {

    //reference ogf next node
    public Out imageOut;

    Texture2D inputImage;
    Texture2D outputImage;

	void Start () {
		
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (inputImage != null)
            {
                ProcessImage();

            }
        }
	}

    public void SetInputImage(Texture2D inputImage)
    {
        this.inputImage = inputImage;
    }

    void ProcessImage()
    {
        //hier kommt der Image Processing Code: zum beispiel Rot Filter

        //Debug.Log(_inputImage.format);
        outputImage = Instantiate(inputImage);

         for (int y = 0; y < outputImage.height; y++)
         {
             for (int x = 0; x < outputImage.width; x++)
             {
                 //if (x < 20 || x > 150)
                 //{
                 outputImage.SetPixel(x, y, new Color(inputImage.GetPixel(x, y).r, 0f, 0f));

                 //}
             }
         }
         outputImage.Apply();




        imageOut.SetInputImage(outputImage);
    }
}
