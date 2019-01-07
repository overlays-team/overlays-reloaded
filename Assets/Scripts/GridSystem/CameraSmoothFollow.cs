using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    /*
     *  this code lets the camera smoothly transition to the desired position
     */

    [Tooltip("how fast does the camera go to the target position")]
    public float smoothSpeed = 0.125f;

    Vector3 desiredPosition;

    private void Start()
    {
        desiredPosition = transform.position;
    }

    private void Update()
    {
        if(transform.position != desiredPosition)
        {
            transform.position = Vector3.Slerp(transform.position, desiredPosition, smoothSpeed / 10 * Time.time);
        }
       
    }

    public void SetTargetPosition(Vector3 desiredPosition)
    {
        this.desiredPosition = desiredPosition;
    }


   

}
