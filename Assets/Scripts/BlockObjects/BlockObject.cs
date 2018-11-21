using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObject : MonoBehaviour {

    /*
     * Der Grundbaustein, alle anderen Blocks erben von diesem
     * Er hat eine Collection von Lasern, welche ihn hitten, 
     * die Kinder entscheiden dann was sie mit diesen Lasern tun und 
     * wieviele sie überhaupt wahrnehmen
     */
    
    //für Positionierung
    [HideInInspector]
    public GridPlane currentAssignedGridPlane;
    protected Vector3 heightCorrector; //Vector der jeweils die Hälfte der Höhe des Objektes beträgt, um ihn auf Planes auf korrekter Höhe aufstellen zu können

    [Tooltip("if this is true - then we cant move the object from its position - good for some puzzle objects like walls etc")]
    public bool stationary = false;
    [Tooltip("if this is true we cant perform the onClickAction;")]
    public bool actionBlocked = false;

    #region smoothing variables

    enum BlockMovementState
    {
        Default, //Block lies on the ground
        Moving,
        Dropping // when we release the block and it slowly falls down
    }

    BlockMovementState movementState;

    //for smoothed rotation
    Quaternion desiredRotation;
    protected float degreesToRotate ; //how many degrees do we roatet
    bool rotate;


    //for smooth block dragging
    Vector3 targetDragPosition;
    #endregion

    //For Lasers
    protected List<Laser> inputLasers;

   

    // Use this for initialization
    protected virtual void Start ()
    {
        heightCorrector = currentAssignedGridPlane.transform.up;
        heightCorrector *= transform.localScale.y / 2;
        transform.position = currentAssignedGridPlane.transform.position + heightCorrector ;
        currentAssignedGridPlane.taken = true;
       
        movementState = BlockMovementState.Default;
        rotate = false;
        desiredRotation = transform.localRotation;
        degreesToRotate = 90;
    }

    protected virtual void Update()
    {
        #region smooth movement update
        if (rotate) PerformRotation();
        switch (movementState)
        {
            case BlockMovementState.Moving:
                PerformDrag();
                break;
            case BlockMovementState.Dropping:
                PerformSnapToPosition();
                break;
        }
        #endregion

        inputLasers = LaserManager.Instance.GetInputLasers(this);
        //every child decides here what to do with the input Lasers
    }



    public virtual void OnMouseClick()
    {
        //most of the mneed to rotate, if they need something else they just override
        if (!actionBlocked)
        {
            Rotate();
        }
        
    }

    #region smooth movement code

    void Rotate()
    {
        rotate = true;
        desiredRotation = desiredRotation*Quaternion.Euler(0, degreesToRotate, 0);
    }

    void PerformRotation()
    {

        if (desiredRotation != transform.rotation)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, desiredRotation, PlayerController.Instance.blockRotationSpeed * Time.deltaTime);

        }
        else
        {
            rotate = false;
        }
    }

    //diese Funktion sorgt dafür dass unser Block nach der bewegung shön weich landed
    public void SnapToPosition(GridPlane gridPlane)
    {
        movementState = BlockMovementState.Dropping;
        if (currentAssignedGridPlane != null) currentAssignedGridPlane.taken = false;
        //transform.position = gridPlane.transform.position + new Vector3(0, 0.5f, 0);
        currentAssignedGridPlane = gridPlane;
        currentAssignedGridPlane.taken = true;

        heightCorrector = currentAssignedGridPlane.transform.up;
        heightCorrector *= transform.localScale.y / 2;
    }

    void PerformSnapToPosition()
    {
        if(currentAssignedGridPlane.transform.position + heightCorrector  != transform.position)
        {
            transform.position = Vector3.Lerp(transform.position, (currentAssignedGridPlane.transform.position + heightCorrector), PlayerController.Instance.blockSnappingSpeed * Time.deltaTime);
            //if we change planes 
           // transform.up = Vector3.Lerp(transform.up, currentAssignedGridPlane.transform.up, PlayerController.Instance.blockSnappingSpeed * Time.deltaTime);
        }
        else
        {
            movementState = BlockMovementState.Default;
        }

    }

    //Diese funktion sorgt dafür, dass unser Object sich schön smooth von seimem GridPlane zu unserem Finger bewegt, wenn wir ihn bewegen wollen und dass es in der luft ebenfalls schön smooth fliegt
    public void SetMovePosition(Vector3 position)
    {
         targetDragPosition = position;
         movementState = BlockMovementState.Moving;

    }

    void PerformDrag()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetDragPosition, PlayerController.Instance.blockDragSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    #endregion


}
