using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    [Header("Raycasting")]
    public Transform laserOutput;
    private Vector3 endPoint;
    public LineRenderer lineRenderer;
    public bool active = true; //do we see the laser?

    [Header("Particle Effect")]
    public GameObject impactParticle;
    public GameObject directionFlowParticle;
    public GameObject particleLight;
    float lastLaserLength; 

    [Header("Game Logic")]
    public BlockObject startingBlock;
    public BlockObject destinationBlock;

    [Header("Image Processing")]
    public Texture2D image;

    private void Start()
    {
        LaserManager.Instance.AddLaser(this);
        SetLaserColor(new Color(1f, 0.5f, 0.5f,0.7f));
    }

    //this function shoots a raycast to see which block we hit and draws the laser accordingly
    public void UpdateLaser()
    {
        if (active)
        {
            //activate visuals
            lineRenderer.enabled = true;
            directionFlowParticle.SetActive(true);
            particleLight.SetActive(true);

            RaycastHit hit;
            // Bit shift the index of the layer (0) to get a bit mask
            int layerMask = 1 << 11;

            //calculate the endPoint
            if (Physics.Raycast(laserOutput.position, laserOutput.forward, out hit, 100, layerMask))
            {
                GameObject hittedObject = hit.collider.gameObject;
                endPoint = hit.point;

                //set impact particle position and activation
                impactParticle.transform.position = hit.point;
                impactParticle.transform.forward = (laserOutput.position - hit.point);
                impactParticle.SetActive(true);

                //set light position
                particleLight.transform.position = laserOutput.position + (hit.point - laserOutput.position)/2;

                //set flow particle position and length
                directionFlowParticle.transform.position = laserOutput.position;
                directionFlowParticle.transform.forward = laserOutput.forward;
                float currentLaserLength = (hit.point - laserOutput.position).magnitude;
                if(currentLaserLength != lastLaserLength) directionFlowParticle.GetComponent<ParticleSystem>().Clear();
                directionFlowParticle.GetComponent<ParticleSystem>().startLifetime = currentLaserLength; //lifetime = laser Length
                lastLaserLength = currentLaserLength;

                if (hittedObject.transform.parent.GetComponent<BlockObject>() != null)
                {
                    if (destinationBlock != hittedObject.transform.parent.GetComponent<BlockObject>())
                    {
                        destinationBlock = hittedObject.transform.parent.GetComponent<BlockObject>();
                        directionFlowParticle.GetComponent<ParticleSystem>().Clear();
                    }
                    if (destinationBlock.HittedInput(this) || destinationBlock is Mirror)
                    {
                        impactParticle.SetActive(false); //wenn wir in einen Input treffen oder einen Reflektor/Spiegel, gibt es kein Impact paricle
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
                if (destinationBlock != null)
                {
                    destinationBlock = null;
                    directionFlowParticle.GetComponent<ParticleSystem>().Clear();
                }
                impactParticle.SetActive(false);
                lineRenderer.SetPosition(0, laserOutput.position);
                lineRenderer.SetPosition(1, laserOutput.position + laserOutput.forward*100);

                //set light position if we dont hit anything just 2 in front of laser
                particleLight.transform.position = laserOutput.position + laserOutput.forward*2;

                //set flow particle position
                directionFlowParticle.transform.position = laserOutput.position;
                directionFlowParticle.transform.forward = laserOutput.forward;

                directionFlowParticle.GetComponent<ParticleSystem>().startLifetime = 100; //lifetime = laser Length
            }
        }
        else
        {
            destinationBlock = null;

            //deactivate visuals
            lineRenderer.enabled = false;
            impactParticle.SetActive(false);
            directionFlowParticle.SetActive(false);
            particleLight.SetActive(false);
        }

    }

    void SetLaserColor(Color color)
    {
        lineRenderer.material.color = color;
        lineRenderer.material.SetColor("_EmissionColor", color);
    }

}
