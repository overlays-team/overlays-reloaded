using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyMovement : MoveBehavior {

    Transform MoveBehavior.changePlace(Transform transform)
    {
        // TODO: I should somehow wait for the second click and get the new Position
        // something like: Transform newPlace = GameObject.Transform();
        // return assignNewPlace(transform, newPlace);

        return transform; //delete this code line when the lines above are implemented
    }
    private Transform assignNewPlace(Transform transformComponent, Transform newPlace) {
        transformComponent.position = newPlace.position;
        return transformComponent;
    }
}
