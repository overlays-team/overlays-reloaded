using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate90 : RotateBehavior
{
	Transform RotateBehavior.rotate(Transform transform) {
        transform.Rotate(Vector3.up, 90f);
        return transform;
    }
}
