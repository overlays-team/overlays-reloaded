using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlane : MonoBehaviour
{
    public bool taken = false;
    public GameObject positiveHalo;
    public GameObject negativeHalo;

    private void Start()
    {
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

}
