using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ColorMixingBehavior {

       ImageData calculateImageDataOutput(List<ImageData> inputs);
}
