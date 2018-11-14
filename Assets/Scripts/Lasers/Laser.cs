using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    //for display
    public Transform laserOutput;
    private Vector3 endPoint;
    public LineRenderer lineRenderer;
    public bool active = true; //do we see the laser?

    // for gameLogic
    public BlockObject startingBlock;
    public BlockObject destinationBlock;

    //for imageprocessing
    public Texture2D image;

    private void Start()
    {
        LaserManager.Instance.AddLaser(this);
        SetLaserColor(new Color(1f, 0.3f, 0f,0.7f));
    }

    //this function shoots a raycast to see which block we hit and draws the laser accordingly
    public void UpdateLaser()
    {
        if (active)
        {
            lineRenderer.enabled = true;
            RaycastHit hit;
            // Bit shift the index of the layer (0) to get a bit mask
            int layerMask = 1 << 11;

            //calculate the endPoint
            if (Physics.Raycast(laserOutput.position, laserOutput.forward, out hit, 100, layerMask))
            {
                GameObject hittedObject = hit.collider.gameObject;
                endPoint = hit.point;

            
                if (hittedObject.transform.parent.GetComponent<BlockObject>() != null)
                {
                    if (destinationBlock != hittedObject.transform.parent.GetComponent<BlockObject>())
                    {
                        destinationBlock = hittedObject.transform.parent.GetComponent<BlockObject>();
                    }
                }
                else
                {
                    destinationBlock = null;
                }

                lineRenderer.SetPosition(0, laserOutput.position);
                lineRenderer.SetPosition(1, endPoint);
            }
            else
            {
                destinationBlock = null;
                lineRenderer.SetPosition(0, laserOutput.position);
                lineRenderer.SetPosition(1, laserOutput.position + laserOutput.forward*100);
            }
        }
        else
        {
            lineRenderer.enabled = false;
            destinationBlock = null;
        }

    }

    void SetLaserColor(Color color)
    {
        lineRenderer.material.color = color;
        lineRenderer.material.SetColor("_EmissionColor", color);
    }

}
