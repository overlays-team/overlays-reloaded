using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridPlane : MonoBehaviour
{
    [Tooltip("if empthy, the plane is a hole where we cant position blocks")]
    public bool empty = false; 

    [HideInInspector]
    public bool taken = false;
    public GameObject positiveHalo;
    public GameObject negativeHalo;

    public MeshCollider meshCollider;
    public MeshRenderer meshRenderer;

    

    private void Start()
    {
        SetEmpthy(empty);
    

        positiveHalo.SetActive(false);
        negativeHalo.SetActive(false);
    }

    private void Update()
    {
        SetEmpthy(empty);
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
            empty = true;
            meshCollider.enabled = false;
            meshRenderer.enabled = false;
        }
        else
        {
            empty = false;
            meshCollider.enabled = true;
            meshRenderer.enabled = true;
        }
    }

}
