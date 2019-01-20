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
            Texture2D newImage = duplicateTexture(loadedImage);

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

    protected override Color ProcessPixel(int x, int y)
    {
        return new Color(
            Mathf.Abs(inputImage1.GetPixel(x, y).r),
            Mathf.Abs(inputImage1.GetPixel(x, y).g),
            Mathf.Abs(inputImage1.GetPixel(x, y).b), 
            mask.GetPixel(x, y).r);
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

}
