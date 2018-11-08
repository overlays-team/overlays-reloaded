using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleBlock : MonoBehaviour { //an interface can't implement MonoBehaviour, that's why an abstract class

    private Transform transformComponent; //this is the current position of this block
    private RotateBehavior rotateBehavior;
    private MoveBehavior moveBehavior;

    public SimpleBlock()
    {
        //default constructor
    }

    public SimpleBlock(RotateBehavior rotateBehavior, MoveBehavior moveBehavior)
    {
        this.rotateBehavior = rotateBehavior;
        this.moveBehavior = moveBehavior;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseDown()
    {
        rotate();
        move();
    }



    public void move() { transformComponent = moveBehavior.changePlace(transformComponent); }
    public void rotate() { transformComponent = rotateBehavior.rotate(transformComponent); }

    public Transform getTransformComponent(){
        return this.transformComponent;
    }

    public void setTransformComponent(Transform transformToSet){
        this.transformComponent = transformToSet;
    }

    public void setRotateBehavior(RotateBehavior rotateBehavior)
    {
        this.rotateBehavior = rotateBehavior;
    }

    public void setMoveBehavior(MoveBehavior moveBehavior)
    {
        this.moveBehavior = moveBehavior;
    }

    //public void delete() {
    // to implement
    //}
}
