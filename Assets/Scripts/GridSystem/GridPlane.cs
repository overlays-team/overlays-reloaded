using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GridPlane : MonoBehaviour
{
    [Tooltip("if empthy, the plane is a hole where we cant position blocks")]
    public bool empty = false; 

    [HideInInspector]
    public bool taken = false;
    public GameObject positiveHalo;
    public GameObject negativeHalo;

    public Collider boxCollider;
    public MeshRenderer meshRenderer;

    public Transform frame;


    private void Start()
    {
        positiveHalo.SetActive(false);
        negativeHalo.SetActive(false);
    }


    void OnValidate()
    {
        SetEmpty(empty);
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
    void SetEmpty(bool isEmpty)
    {
        if (isEmpty)
        {
            empty = true;
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
            foreach(Transform child in frame)
            {
                  child.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            empty = false;
            boxCollider.enabled = true;
            meshRenderer.enabled = true;
            foreach (Transform child in frame)
            {
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
}
