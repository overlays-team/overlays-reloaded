using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageOutput : BlockObject {

    //wenn wir nur einen ImageInput haben wollen das:
    [SerializeField]
    LaserInput laserInput;
    Laser inputLaser;
    Texture2D inputImage;

    public Image debugImage; //just for now
    public Texture2D noImage; //just a white texture we show when no image is present

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (inputLasers.Count > 0)
        {
            bool inputLaserFound = false;
            foreach (Laser laser in inputLasers)
            {
                //check if one of our input Lasers hits the input
                if (Vector3.Angle(laser.laserOutput.forward, laserInput.transform.forward) < 5)
                {
                    inputLaserFound = true;
                    inputLaser = laser;
                    inputImage = laser.image;
                    debugImage.sprite = Sprite.Create(inputImage, new Rect(0, 0, inputImage.width, inputImage.height), new Vector2(0.5f, 0.5f));
                    Debug.Log("we got an image");
                    //jetzt müssten wir irgendwie schauen ob input Image richtig ist
                }
            }
            if (!inputLaserFound)
            {
                inputLaser = null;
                debugImage.sprite = Sprite.Create(noImage, new Rect(0, 0, inputImage.width, inputImage.height), new Vector2(0.5f, 0.5f));
            }
        }
        else
        {
            inputLaser = null;
            debugImage.sprite = Sprite.Create(noImage, new Rect(0, 0, inputImage.width, inputImage.height), new Vector2(0.5f, 0.5f));

        }
    }
}
