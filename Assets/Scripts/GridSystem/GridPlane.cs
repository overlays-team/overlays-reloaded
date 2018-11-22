using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlane : MonoBehaviour
{
    public bool taken = false;
    public GameObject positiveHalo;
    public GameObject negativeHalo;

    public MeshCollider meshCollider;
    public MeshRenderer meshRenderer;

    public bool empthy = false; //if empthy, the plane is a hole where we cant position blocks

    private void Start()
    {
        SetEmpthy(empthy);
    

        positiveHalo.SetActive(false);
        negativeHalo.SetActive(false);
    }

    public void ShowHalo()
    {
        if (taken)
        {
            positiveHalo.SetActive(false);
            negativeHalo.SetActive(true);
        }
        else
        {
            positiveHalo.SetActive(true);
            negativeHalo.SetActive(false);
        }
    }

    public void HideHalo()
    {
        positiveHalo.SetActive(false);
        negativeHalo.SetActive(false);
    }

    //function which toggles between empthy and normal
    void SetEmpthy(bool _empthy)
    {
        if (_empthy)
        {
            empthy = true;
            meshCollider.enabled = false;
            meshRenderer.enabled = false;
        }
        else
        {
            empthy = false;
            meshCollider.enabled = true;
            meshRenderer.enabled = true;
        }
    }

}
