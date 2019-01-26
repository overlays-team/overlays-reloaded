using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*positioniert die GridPlanes im EditMode*/
[ExecuteInEditMode]
public class GridPositioner : MonoBehaviour
{

    [SerializeField]
    int colums;
    [SerializeField]
    int rows;
    [SerializeField]
    float padding;

    [SerializeField]
    GameObject gridPlane;

    [SerializeField]
    GameObject[,] gridPlaneArray;

    [SerializeField]
    GameObject mainCamera;

    [SerializeField]
    float gridPadding = 1f;

    [Tooltip("Only used in Time attack Mode")]
    public LevelInstantiator LevelInstantiator;

    //variables necessary for camera positioning
   
    float gridHeight;
    float gridWidth;
    Vector3 middlePoint;

    public void UpdatePlanes()
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            DestroyImmediate(child.gameObject);
        }

        float currentZ = transform.position.z;
        float currentX = transform.position.x;

        for (int row = 0; row < colums; row++)
        {
            currentZ = transform.position.z;
            for (int column = 0; column < rows; column++)
            {
                GameObject plane = Instantiate(gridPlane, new Vector3(currentX, transform.position.y, currentZ), transform.rotation, transform);
                //plane.transform.SetParent(transform);
                currentZ += padding;
            }

            currentX += padding;
        }

        gridWidth = colums + (padding - 1 ) * (colums - 1);
        gridHeight = rows + (padding - 1) * (rows - 1);

        middlePoint = transform.position + new Vector3((gridWidth / 2) - 0.5f, 0f, (gridHeight / 2) - 0.5f); //0.5f because thats half of the plane and the gridPositioner starts in the middle of the bottom left plane

        adjustCamera();
    }

    public void UpdatePlanes(int _rows, int _columns, float _padding)
    {
        int rows = LevelInstantiator.getIdx0();
        int col = LevelInstantiator.getIdx1();
        gridPlaneArray = new GameObject[rows, col];

        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            DestroyImmediate(child.gameObject);
        }

        float currentZ = transform.position.z;
        float currentX = transform.position.x;

        for (int row = 0; row < _rows; row++)
        {
            currentZ = transform.position.z;
            for (int column = 0; column < _columns; column++)
            {
                GameObject plane = Instantiate(gridPlane, new Vector3(currentX, transform.position.y, currentZ), transform.rotation, transform);
                //plane.transform.SetParent(transform);
                currentZ += _padding;
                gridPlaneArray[row, column] = plane;
            }

            currentX += _padding;
        }

        gridWidth = _columns + (_padding - 1) * (_columns - 1);
        gridHeight = _rows + (_padding - 1) * (_rows - 1);
        middlePoint = transform.position + new Vector3((gridWidth / 2) - 0.5f, 0f, (gridHeight / 2) - 0.5f); //0.5f because thats half of the plane and the gridPositioner starts in the middle of the bottom left plane
        Debug.Log(middlePoint);
        adjustCamera();
    }

    #region Getters

    public GameObject[,] GetGridArray()
    {
        return gridPlaneArray;
    }

    internal float GetPadding()
    {
        return padding;
    }

    //returns the width of the grid in unity units - one gridPlane is 1x1 big
    public float GetGridWidth()
    {
        return gridWidth;
    }

    public float GetGridHeight()
    {  
        return gridHeight;
    }

    //returns the middle point of the grid system - used by the responsive camera positioner
    public Vector3 GetMiddlePoint()
    {
        return middlePoint;
    }

    #endregion

    private void adjustCamera ()
    {
        Vector3 cameraPosition = middlePoint;
        cameraPosition.y = adjustHeight() + this.transform.position.y;

        Debug.Log(cameraPosition);

        mainCamera.transform.position = cameraPosition;
    }

    private void adjustCameraTimeAttack()
    {
        Vector3 cameraPosition = middlePoint;
        cameraPosition.y = adjustHeight() + this.transform.position.y;

        Debug.Log(cameraPosition);

        mainCamera.transform.position = cameraPosition;
    }

    private float adjustHeight()
    {
        float degree = 45;
        float height;
        float alpha = Mathf.Deg2Rad * (degree/2);
        if (gridWidth >= gridHeight)
        {
            height = (gridWidth / 2 + gridPadding) / Mathf.Tan(alpha);
        }
        else
        {
            height = (gridHeight / 2 + gridPadding) / Mathf.Tan(alpha);
        }
       
        return height;
    }
}