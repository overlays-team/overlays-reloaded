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

    [SerializeField]
    GameObject gridPlane;
	
	public void UpdatePlanes()
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            DestroyImmediate(child.gameObject);
        }

        float currentZ = transform.position.z;
        float currentX = transform.position.x;

        for (int row = 0; row < rows; row++)
        {
            currentZ = transform.position.z;
            for (int column = 0; column < columns; column++)
            {
                GameObject plane = Instantiate(gridPlane, new Vector3(currentX,transform.position.y,currentZ), transform.rotation);
                plane.transform.SetParent(transform);
                currentZ += padding;
            }

            currentX += padding;
        }
        
	}
}
