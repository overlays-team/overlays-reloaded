using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveCameraPositioner : MonoBehaviour
{
    [SerializeField]
    GridPositioner gridPositoner;

    [SerializeField]
    float padding;

    private void Start()
    {
        AdjustCamera();
    }

    //dor debug purposes
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AdjustCamera();
        }
    }*/


    private void AdjustCamera()
    {
        Vector3 cameraPosition = gridPositoner.GetMiddlePoint();
        cameraPosition.y += CalculateHeight();

        transform.position = cameraPosition;
    }

    private float CalculateHeight()
    {
        float degree = Camera.main.fieldOfView/2;
        float height;
        float alpha = Mathf.Deg2Rad * degree;
        if (gridPositoner.GetGridWidth() >= gridPositoner.GetGridHeight())
        {
            height = (gridPositoner.GetGridWidth() + padding) / 2  / Mathf.Tan(alpha);
        }
        else
        {
            height = (gridPositoner.GetGridHeight() + padding) / 2 / Mathf.Tan(alpha);
        }

        return height;
    }
}
