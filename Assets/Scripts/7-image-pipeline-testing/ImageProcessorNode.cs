using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageProcessorNode : MonoBehaviour {

    //reference ogf next node
    public Out imageOut;

    //for fps count on Mobile
    public Text fpsText;
    float smallestFrameRate = 60;

    Texture2D inputImage;
    Texture2D outputImage;
    bool imageReady;
    bool imageInProcess;



	void Start () {
		
	}

	void Update () {
        float currentframeRate = (1.0f / Time.deltaTime);
        if (currentframeRate < smallestFrameRate)
        {
            smallestFrameRate = currentframeRate;
            fpsText.text = smallestFrameRate.ToString();

        }
        

        if (Input.GetMouseButtonDown(0))
        {
            smallestFrameRate = 60;
            if (inputImage != null)
            {
                ProcessImage();
            }
        }



        if (imageInProcess)
        {
            imageOut.debugImage.gameObject.SetActive(false);
            if (imageReady)
            {
                imageOut.debugImage.gameObject.SetActive(true);
                imageInProcess = false;
                imageOut.SetInputImage(outputImage);
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
        imageReady = false;
        imageInProcess = true;
        StartCoroutine("RedFilterEnumerator");


        //imageOut.SetInputImage(ImageProcessor.RedFilter(outputImage));


    }

    IEnumerator RedFilterEnumerator()
    {
        for (int y = 0; y < outputImage.height; y++)
        {
            for (int x = 0; x < outputImage.width; x++)
            {
                //if (x < 20 || x > 150)
                //{
                outputImage.SetPixel(x, y, new Color(outputImage.GetPixel(x, y).r, 0f, 0f));
                
                //}
            }
            if(y%10==0)yield return null;
        }
        outputImage.Apply();
        imageReady = true;
        
    }
}
