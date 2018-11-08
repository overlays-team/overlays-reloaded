using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveColorMixing : ColorMixingBehavior {

    List<ImageData> ColorMixingBehavior.calculateImageDataOutput(List<ImageData> inputs)
    {
        //TODO: additive box code
        List<ImageData> outputs = new List<ImageData>();
        outputs.Add(new ImageData());
        return outputs; //change it to the result of additive box calculations
    }
}
