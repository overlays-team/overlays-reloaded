using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMovement : MoveBehavior {

    Transform MoveBehavior.changePlace(Transform transform){
        return transform;
    }
}
