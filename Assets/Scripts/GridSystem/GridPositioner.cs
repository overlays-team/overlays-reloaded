using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*positions the gridPlanes in EditMode*/
[ExecuteInEditMode]
public class GridPositioner : MonoBehaviour
{

    [SerializeField]
    bool timeAttack = false; // only used by camera positioner

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

    [Tooltip("Only used in Time attack Mode")]
    public LevelInstantiator LevelInstantiator;

    //variables necessary for camera positioning
    float gridHeight;
    float gridWidth;
    Vector3 middlePoint;

    private void Awake()
    {
        gridWidth = colums + (padding - 1) * (colums - 1);
        gridHeight = rows + (padding - 1) * (rows - 1);

        if(!timeAttack) middlePoint = transform.position + new Vector3((gridWidth / 2) - 0.5f, 0f, (gridHeight / 2) - 0.5f); //0.5f because thats half of the plane and the gridPositioner starts in the middle of the bottom left plane
        else middlePoint = transform.position + new Vector3((gridWidth / 2) - 0.5f, 0f, -(gridHeight / 2) + 0.5f);
    }

    #if UNITY_EDITOR
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
                GameObject plane = PrefabUtility.InstantiatePrefab(gridPlane) as GameObject;
                plane.transform.position = new Vector3(currentX, transform.position.y, currentZ);
                plane.transform.SetParent(transform);
                //plane.transform.SetParent(transform);
                currentZ += padding;
            }

            currentX += padding;
        }
    }
    #endif

    public void UpdatePlanes(int _rows, int _columns, float _padding)
    {
        rows = LevelInstantiator.getIdx0();
        colums = LevelInstantiator.getIdx1();
        gridPlaneArray = new GameObject[rows, colums];

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

        gridWidth = this.colums + (padding - 1) * (this.colums - 1);
        gridHeight = rows + (padding - 1) * (rows - 1);
        //0.5f because thats half of the plane and the gridPositioner starts in the middle of the bottom left plane
        middlePoint = transform.position + new Vector3((gridWidth / 2) - 0.5f, 0f, -(gridHeight / 2) + 0.5f);
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
 
}