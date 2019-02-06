using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserInput : MonoBehaviour
{
    //these are assigned a children of blockObjects, the blockObjects check if they are hitted by lasers
    public Laser inputLaser;
    public bool active; //are active if a laser is hitting a input

    private void Start()
    {
        active = false;
    }
}
