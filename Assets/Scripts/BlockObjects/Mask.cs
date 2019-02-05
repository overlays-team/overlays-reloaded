using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mask : BlockObject
{
    [Tooltip("assign the mask here - the mask takes the r value for alpha blending")]
    public Texture2D mask;

    Texture2D loadedImage; // for image import

    //public Texture2D testLoadedImage;


    protected override void Start()
    {
        base.Start();
        blockImage.GetComponent<Image>().sprite = Sprite.Create(mask, new Rect(0, 0, mask.width, mask.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (loadedImage != null)
        {
            Debug.Log("2");
            Texture2D newImageNotCroppedYet = duplicateTexture(loadedImage);
            Texture2D newImage = CropToSquare(newImageNotCroppedYet);

            //laserOutput.laser.image = newImage;
            mask = newImage;
            blockImage.GetComponent<Image>().sprite = Sprite.Create(newImage, new Rect(0, 0, newImage.width, newImage.height), new Vector2(0.5f, 0.5f));
            //detailedNodeViewImage.sprite = debugImage.sprite;
            if (imageProcessingState != ImageProcessingState.NoImage)
            {
                StartImageProcessing();
            }

            loadedImage = null;
        }
        else
        {
            if (lasersChanged)
            {
                imageProcessingState = ImageProcessingState.NoImage;
                if (laserInputs[0].active)
                {
                    inputImage1 = laserInputs[0].inputLaser.image;
                    Grow();
                    StartImageProcessing();
                }
                else
                {
                    inputImage1 = null;
                    Shrink();
                    StopImageProcessing();
                }
            }

            UpdateOutputImageDisplayAndSendThroughLaser();
        }
    }

    protected override void StartImageProcessing()
    {
        //startet das Image Processing welches über mehrere Frames in dem Enumerator läuft

        //als output Image setzen wir das größere Bilde, das andere Bild wird upgescaled
        if (mask.width > inputImage1.width)
        {
            outputImage = Instantiate(mask);
        }
        else
        {
            outputImage = Instantiate(inputImage1);
        }

        imageProcessingState = ImageProcessingState.Processing;

        StartCoroutine("ImageProcessingEnumerator");
    }

    protected override Color ProcessPixel(int x, int y)
    {
        float resolutionDifference;
        Color pixel;
        Color maskValue;

        if (inputImage1.width > mask.width)
        {
            resolutionDifference = inputImage1.width / mask.width;
            pixel = inputImage1.GetPixel(x, y);
            maskValue = mask.GetPixel((int)(x / resolutionDifference), (int)(y / resolutionDifference));
        }
        else
        {
            resolutionDifference = mask.width / inputImage1.width;

            pixel = inputImage1.GetPixel((int)(x / resolutionDifference), (int)(y / resolutionDifference));
            maskValue = mask.GetPixel(x, y);
        }


            return new Color(
            pixel.r,
            pixel.g,
            pixel.b, 
            maskValue.r);
    }


    // for mask importing:

    public void OnImportButtonClicked()
    {
        Debug.Log("import Button clicked");

        PickImageFromGallery();
    }

    public void PickImageFromGallery(int maxSize = 1024)
    {
        //loadedImage = testLoadedImage;
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Create Texture from selected image
                loadedImage = NativeGallery.LoadImageAtPath(path, maxSize);
            }
        }, maxSize: maxSize);

    }

    //we need to use this workaround because importet textures are not readable, copied from stackOverflow
    Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    Texture2D CropToSquare(Texture2D imageToBeCropped)
    {
        int sideLength = Mathf.Min(imageToBeCropped.width, imageToBeCropped.height);
        Texture2D croppedTexture = new Texture2D(sideLength, sideLength);

        if (imageToBeCropped.width > imageToBeCropped.height)
        {
            int differenceInAspect = imageToBeCropped.width - imageToBeCropped.height;
            int pixelOffset = differenceInAspect / 2;

            for (int y = 0; y < croppedTexture.height; y++)
            {
                for (int x = 0; x < croppedTexture.width; x++)
                {
                    croppedTexture.SetPixel(x, y, imageToBeCropped.GetPixel(x + pixelOffset, y));
                }

            }
            croppedTexture.Apply();
        }
        else if (imageToBeCropped.height > imageToBeCropped.width)
        {
            int differenceInAspect = imageToBeCropped.height - imageToBeCropped.width;
            int pixelOffset = differenceInAspect / 2;

            for (int y = 0; y < croppedTexture.height; y++)
            {
                for (int x = 0; x < croppedTexture.width; x++)
                {
                    croppedTexture.SetPixel(x, y, imageToBeCropped.GetPixel(x, y + pixelOffset));
                }

            }
            croppedTexture.Apply();
        }
        return croppedTexture;
    }

}
