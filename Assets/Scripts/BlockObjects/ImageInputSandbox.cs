using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageInputSandbox : ImageInput
{
    /*
     * here we can import an image from our gallery to use it for our image processing, 
    we dont use the SetUp image function of imageInput because we do noot need the same
    compressionFormat for all images here
    */

    Texture2D loadedImage;

    protected override void Start()
    {
        imageProcessingState = ImageProcessingState.Displaying; // we set this here so we can always zoom in the cetailed node view

        base.Start();
        outputImage = new Texture2D(inputImage.width, inputImage.height, textureFormat, false);
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
        detailedNodeViewImage.sprite = debugImage.sprite;
    }

    public void OnImportButtonClicked()
    {
        Debug.Log("import Button clicked");

        PickImageFromGallery();
    }

    protected override void Update()
    {
        base.Update();

        if (loadedImage != null)
        {
            Texture2D newImageNotCroppedYet = duplicateTexture(loadedImage);
            Texture2D newImage = CropToSquare(newImageNotCroppedYet);

            outputImage = newImage;
            laserOutput.laser.image = outputImage;
            debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, outputImage.width, outputImage.height), new Vector2(0.5f, 0.5f));
            detailedNodeViewImage.sprite = debugImage.sprite;

            loadedImage = null;
        }

    }

    //here we use a plugin from the asset store
    public void PickImageFromGallery(int maxSize = 1024)
    {

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
        Texture2D croppedTexture = new Texture2D(sideLength, sideLength, textureFormat, false);
       
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
