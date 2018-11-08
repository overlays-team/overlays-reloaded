using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlock : SimpleBlock {

    public WallBlock(RotateBehavior rotateBehavior, MoveBehavior moveBehavior) : base(rotateBehavior, moveBehavior) { }

    //die zweite Variante, wobei wir sie vermutlich nicht brauchen
    public WallBlock()
    {
        setRotateBehavior(new NoRotation());
        setMoveBehavior(new NoMovement());
    }

    // Use this for initialization
    void Start()
    {
        new WallBlock(new NoRotation(), new NoMovement());
    }




}
