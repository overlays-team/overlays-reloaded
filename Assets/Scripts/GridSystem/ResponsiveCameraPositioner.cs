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


    public void AdjustCamera()
    {
        if(gridPositoner)
        {
            Vector3 cameraPosition = gridPositoner.GetMiddlePoint();
            cameraPosition.y += CalculateHeight();

            transform.position = cameraPosition;
        }
    }

    private float CalculateHeight()
    {
        float degree = Camera.main.fieldOfView/2;
        float height;
        float alpha = Mathf.Deg2Rad * degree;

        height = (gridPositoner.GetGridWidth() / 2 + padding) / Mathf.Tan(alpha);       

        return height;
    }
}
