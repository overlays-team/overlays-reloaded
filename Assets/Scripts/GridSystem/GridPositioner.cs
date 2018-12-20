using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*positioniert die GridPlanes im EditMode*/
[ExecuteInEditMode]
public class GridPositioner : MonoBehaviour {

    [SerializeField]
    int rows;
    [SerializeField]
    int columns;
    [SerializeField]
    float padding;

    //int rowsLastFrame;
    //int columnsLastFrame;
    //float paddingLastFrame

    List<GameObject> gridPlanes = new List<GameObject>();
    [SerializeField]
    GameObject gridPlane;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	public void UpdatePlanes()
    {
        foreach (GameObject plane in gridPlanes)
        {
            DestroyImmediate(plane);
        }

        float currentZ = transform.position.z;
        float currentX = transform.position.x;

        for (int row = 0; row < rows; row++)
        {
            currentX += padding;
            currentZ = transform.position.z;
            for (int column = 0; column < columns; column++)
            {
                currentZ += padding;
                GameObject plane = Instantiate(gridPlane, new Vector3(currentX,transform.position.y,currentZ), transform.rotation);
                gridPlanes.Add(plane);
                plane.transform.SetParent(transform);
            }
        }
        
	}
}
