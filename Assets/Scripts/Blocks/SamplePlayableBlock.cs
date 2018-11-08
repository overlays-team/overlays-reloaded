using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayableBlock : ImageDataBlock {

    public SamplePlayableBlock(RotateBehavior rotateBehavior, MoveBehavior moveBehavior, ColorMixingBehavior colorMixingBehavior) 
        : base(rotateBehavior, moveBehavior, colorMixingBehavior) { }

    // Use this for initialization
    void Start () {
        //you can also try AnyMovement() when we know how it works with grid cells
        new SamplePlayableBlock(new Rotate90(), new NoMovement(), new AdditivieColorMixing());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
