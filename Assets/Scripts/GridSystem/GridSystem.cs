using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{

    public GridPlane[] gridPlanes; // collection of all gridPlanes
    public BlockObject[] blockObjects; 

	void Awake ()
    {
		//suche für jedes BlockObjectk die näheste GridPlane, snappe
        foreach(BlockObject blockObject in blockObjects)
        {
            float nearestDistance = float.PositiveInfinity;
            GridPlane nearestPlane = null;

            foreach (GridPlane gridPlane in gridPlanes)
            {
                if(gridPlane.taken == false)
                {
                    float currentDistance = Vector3.Distance(gridPlane.transform.position, blockObject.transform.position);
                    if (currentDistance < nearestDistance)
                    {
                        nearestDistance = currentDistance;
                        nearestPlane = gridPlane;
                    }
                }
            }

            //blockObject.transform.position = nearestPlane.transform.position + new Vector3(0,0.5f,0);
            blockObject.currentAssignedGridPlane = nearestPlane;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
