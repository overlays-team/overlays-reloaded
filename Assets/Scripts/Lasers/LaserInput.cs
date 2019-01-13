using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserInput : MonoBehaviour
{
    //diese werden als Children einem BlockObjekt angehängt und er überprüft jeweils ob deren (Typ?) und Rotation mit den einfallenden Lasern stimmen
    public Laser inputLaser;
    public bool active; //are active if a laser id hitting a input
    [SerializeField]
    private Color activecolor = new Color(1, 0, 0); //if laser output is active
    [SerializeField]
    private Color inactivecolor = new Color(1, 1, 1);//if laser output is inactive

    private void Start()
    {
        active = false;
    }

    void Update()
    {
        if (active)
        {
            ChangeMaterial(activecolor);
        }
        else
        {
            ChangeMaterial(inactivecolor);
        }
    }

    //chnages emission (glow effect) in each children of graphics
    void ChangeMaterial(Color emissioncolor)
    {
        if (this.transform.childCount == 0)
        {
            return;
        }
        Transform graphics = this.gameObject.transform.GetChild(0);
        foreach (Transform child in graphics)
        {
            GameObject gochild = child.gameObject;
            Renderer meshrenderer = gochild.GetComponent<Renderer>();
            if (meshrenderer != null)
            {
                meshrenderer.material.SetColor("_Color", emissioncolor); //_EmissionColor
            }
        }
    }
}
