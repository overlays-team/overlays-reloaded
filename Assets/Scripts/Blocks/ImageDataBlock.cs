using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ImageDataBlock : SimpleBlock {

    List<ImageData> inputs;
    ImageData imageDataOutput;
    private ColorMixingBehavior colorMixingBehavior;

    public ImageDataBlock(RotateBehavior rotateBehavior, MoveBehavior moveBehavior, ColorMixingBehavior colorMixingBehavior) : base(rotateBehavior, moveBehavior)
    {
        this.colorMixingBehavior = colorMixingBehavior;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // update inputs

        //calculate imageDataOutput
        this.imageDataOutput = colorMixingBehavior.calculateImageDataOutput(this.inputs);

    }

    public void setColorMixingBehavior(ColorMixingBehavior colorMixingBehavior)
    {
        this.colorMixingBehavior = colorMixingBehavior;
    }
}
