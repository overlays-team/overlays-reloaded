using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageInput : BlockObject
{
    [SerializeField]
    private LaserOutput laserOutput;
    [SerializeField]
    private Texture2D inputImage;

    protected override void Start()
    {
        base.Start();
        laserOutput.active = true;

        // here we would need to convert the image in another dataformat that we can use to transport it between images and process it with a good performance
        
    }


}
