using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ColorMixingBehavior {

       List<ImageData> calculateImageDataOutput(List<ImageData> inputs);
}
