﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField]
    bool developerMode = true; // if this is true - the grid system automaticly searches far all active planes and grid blocks and assigns them 
                               // this is convenient for the level designer but not optimal for the builded game, if a level is ready we should disable developer mode and assign all gridBlocks and gridBlanes in the inspector,
                               // although we can leave it, because its not much of a performance impact

    public GridPlane[] gridPlanes; // collection of all gridPlanes in the scene
    public BlockObject[] blockObjects; //collection of al the objects in the scene

	void Awake ()
    {
        if (developerMode)
        {
            GameObject[] gridGO  = GameObject.FindGameObjectsWithTag("gridPlane");
            GameObject[] blockGO = GameObject.FindGameObjectsWithTag("blockObject");

            gridPlanes = new GridPlane[gridGO.Length];
            blockObjects = new BlockObject[blockGO.Length];

            for(int i = 0; i< gridGO.Length; i++)
            {
                gridPlanes[i] = gridGO[i].GetComponent<GridPlane>();
            }

            for (int i = 0; i < blockGO.Length; i++)
            {
                blockObjects[i] = blockGO[i].GetComponent<BlockObject>();
            }

        }
        
        
        //search the nearets gridPlane for every blockObject, snap them
        foreach(BlockObject blockObject in blockObjects)
        {
            float nearestDistance = float.PositiveInfinity;
            GridPlane nearestPlane = null;

            foreach (GridPlane gridPlane in gridPlanes)
            {
                if(gridPlane.taken == false && !gridPlane.empty)
                {
                    float currentDistance = Vector3.Distance(gridPlane.transform.position, blockObject.transform.position);
                    if (currentDistance < nearestDistance)
                    {
                        nearestDistance = currentDistance;
                        nearestPlane = gridPlane;
                    }
                }
            }

            blockObject.currentAssignedGridPlane = nearestPlane;
        }
	}
}
