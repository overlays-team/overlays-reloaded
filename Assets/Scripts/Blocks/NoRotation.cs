using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRotation : RotateBehavior {

    Transform RotateBehavior.rotate(Transform transform)
    {
        return transform;
    }

}
