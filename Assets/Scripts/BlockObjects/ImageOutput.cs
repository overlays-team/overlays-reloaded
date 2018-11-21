using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageOutput : BlockObject {

    //wenn wir nur einen ImageInput haben wollen das:
    Texture2D inputImage;

    public Image debugImage; //just for now
    public Texture2D noImage; //just a white texture we show when no image is present

    protected override void Start()
    {
        base.Start();
        inputImage = null;
        debugImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (lasersChanged)
        {
            if (laserInputs[0].active)
            {
                inputImage = laserInputs[0].inputLaser.image;
                debugImage.sprite = Sprite.Create(inputImage, new Rect(0, 0, inputImage.width, inputImage.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                inputImage = null;
                debugImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
            }

        }
    }           

}
