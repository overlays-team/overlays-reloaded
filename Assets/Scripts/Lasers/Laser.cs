using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    /*
     * the lasers work with a line renderer and raycasting, they only hit objects on the blockObjectsGraphics layer
     */


    [Header("Raycasting")]
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

    [Header("removing the \"glitch in the matrix\"")]
    public bool isMovingFast = false; //if this is true inputs and mirrors ignore tis laser - to prevent too fast laser movements
    public float fastMovementTreshhold = 0.2f;
    Quaternion lastRotation;


    private void Start()
    {
        LaserManager.Instance.AddLaser(this);
        SetLaserColor(new Color(1f, 0.5f, 0.5f,0.7f));
        fastMovementTreshhold *= PlayerController.Instance.blockRotationSpeed;
    }

    //this function shoots a raycast to see which block we hit and draws the laser accordingly
    public void UpdateLaser()
    {
        if (active)
        {
            //check if isMovingFast should Be true
            if(Quaternion.Angle(lastRotation, transform.rotation) > fastMovementTreshhold)
            {
                isMovingFast = true;
            }
            else
            {
                isMovingFast = false;
            }

            //activate visuals
            lineRenderer.enabled = true;
            directionFlowParticle.SetActive(true);
            //particleLight.SetActive(true); // we disabled laser lighting for the sake of performance, but could be reenabled

            RaycastHit hit;
            // Bit shift the index of the layer (0) to get a bit mask
            int layerMask = 1 << 11;

            //calculate the endPoint
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100, layerMask))
            {
                GameObject hittedObject = hit.collider.gameObject;
                endPoint = hit.point;

                //set impact particle position and activation
                impactParticle.transform.position = hit.point;
                Vector3 forwardOfImpactParticle = (transform.position - hit.point);
                if (forwardOfImpactParticle != Vector3.zero) impactParticle.transform.forward = forwardOfImpactParticle;
                impactParticle.SetActive(true);

                //set light position
                //particleLight.transform.position = laserOutput.position + (hit.point - laserOutput.position)/2;

                //set flow particle position and length
                directionFlowParticle.transform.position = transform.position;
                directionFlowParticle.transform.forward = transform.forward;
                float currentLaserLength = (hit.point - transform.position).magnitude;
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
                        impactParticle.SetActive(false); //if we hit a laserInput or a mirror, there is no impact particle, because these should be there
                    }
                }
                else
                {
                    destinationBlock = null;
                }

                lineRenderer.SetPosition(0, transform.position);
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
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + transform.forward*100);

                //set light position if we dont hit anything just 2 in front of laser
                //particleLight.transform.position = laserOutput.position + laserOutput.forward*2;

                //set flow particle position
                directionFlowParticle.transform.position = transform.position;
                directionFlowParticle.transform.forward = transform.forward;

                directionFlowParticle.GetComponent<ParticleSystem>().startLifetime = 100; //lifetime = laser Length
            }

            lastRotation = transform.rotation;

        }
        else
        {
            destinationBlock = null;

            //deactivate visuals
            lineRenderer.enabled = false;
            impactParticle.SetActive(false);
            directionFlowParticle.SetActive(false);
            //particleLight.SetActive(false);
        }

    }

    void SetLaserColor(Color color)
    {
        lineRenderer.material.color = color;
        lineRenderer.material.SetColor("_EmissionColor", color);
    }

    public void OnDestroy()
    {
        LaserManager.Instance.RemoveLaser(this);
    }

    public Vector3 GetHitPoint()
    {
        return endPoint;
    }

}
