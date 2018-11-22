using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserInput : MonoBehaviour
{
    //diese werden als Children einem BlockObjekt angehängt und er überprüft jeweils ob deren (Typ?) und Rotation mit den einfallenden Lasern stimmen
    public Laser inputLaser;
    public bool active; //are active if a laser id hitting a input

    private void Start()
    {
        active = false;
    }
}
