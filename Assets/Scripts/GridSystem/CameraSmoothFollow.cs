using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    /*
     *  this code lets the camera smoothly transition to the desired position
     */
    public Transform target;
    float offset;

    [Tooltip("how fast does the camera follow the player")]
    public float smoothSpeed = 0.125f;
    [Tooltip("how fast does the camera change rotation to face the player")]
    public float smoothSpeedTurning = 10f;

    Vector3 smoothedPosition;
    Vector3 desiredPosition;

    void Start()
    {
        offset = (target.position - transform.position).magnitude;
    }

    private void Update()
    {
        //position
        desiredPosition = target.up * offset;

        //rotation
        Vector3 desiredlookDirection = target.position - transform.position;

        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed/1000 * Time.time);

        Vector3 smoothedlookDirection = Vector3.Slerp(transform.forward, desiredlookDirection, smoothSpeedTurning * Time.deltaTime);

        transform.position = smoothedPosition;
        transform.forward = smoothedlookDirection;

    }


   

}
