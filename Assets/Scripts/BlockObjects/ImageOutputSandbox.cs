using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageOutputSandbox : BlockObject
{
    [Header("Image Output Sandbox")]
    [Tooltip("assign the checker Texture here, what do we see if theres no image here")]
    public Texture2D noImage;

    [SerializeField]
    [Tooltip("this panel shows the detailed node view and can contain further options for settings in the nodes - used for sandbox mode")]
    GameObject detailedNodeView;

    protected override void Start()
    {
        base.Start();
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
                inputImage1 = laserInputs[0].inputLaser.image;
                debugImage.sprite = Sprite.Create(inputImage1, new Rect(0, 0, inputImage1.width, inputImage1.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                inputImage1 = null;
                debugImage.sprite = Sprite.Create(noImage, new Rect(0, 0, noImage.width, noImage.height), new Vector2(0.5f, 0.5f));
            }
        }
    }

    protected override void DoubleClickAction()
    {

        detailedNodeView.SetActive(true);
    }
}
