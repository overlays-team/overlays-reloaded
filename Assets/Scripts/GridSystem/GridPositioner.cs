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

    public LevelInstantiator LevelInstantiator;

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
                GameObject plane = Instantiate(gridPlane, new Vector3(currentX, transform.position.y, currentZ), transform.rotation);
                plane.transform.SetParent(transform);
                currentZ += padding;
            }

            currentX += padding;
        }

    }

    internal float getPadding()
    {
        return padding;
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
                GameObject plane = Instantiate(gridPlane, new Vector3(currentX, transform.position.y, currentZ), transform.rotation);
                plane.transform.SetParent(transform);
                currentZ += _padding;
                gridPlaneArray[row, column] = plane;
            }

            currentX += _padding;
        }

    }

    public GameObject[,] getGridArray()
    {
        return gridPlaneArray;
    }
}