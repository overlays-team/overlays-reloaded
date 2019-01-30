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
        //adjust the padding, if we are using a tablet we make it smaller
        if (Screen.height * 1f / Screen.width  < 1.6)
        {
            padding = padding * 0.75f;
        }
 
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
