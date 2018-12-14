using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockObject : MonoBehaviour
{ 

    /*
     * Der Grundbaustein, alle anderen Blocks erben von diesem
     * Er hat eine Collection von Lasern, welche ihn hitten, 
     * die Kinder entscheiden dann was sie mit diesen Lasern tun und 
     * wieviele sie überhaupt wahrnehmen
     */

    //für Positionierung
    //[HideInInspector]
    public GridPlane currentAssignedGridPlane;
    protected Vector3 heightCorrector; //Vector der jeweils die Hälfte der Höhe des Objektes beträgt, um ihn auf Planes auf korrekter Höhe aufstellen zu können

    [Tooltip("if this is true - then we cant move the object from its position - good for some puzzle objects like walls etc")]
    public bool stationary = false;
    [Tooltip("if this is true we cant perform the onClickAction;")]
    public bool actionBlocked = false;

    public bool inInventory = false; // if its in the inventory it wont perform the standard start function
    public int inventoryIndex;
    public Sprite inventoryIcon;

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

    public LineRenderer frame;

    //for laser inputs
    [SerializeField]
    protected LaserInput[] laserInputs;

    protected bool lasersChanged; //did the laserInputs change last frame?
    bool[] activeLasersLastFrame;
    Texture2D[] imagesLastFrame;

    //for image processing
    protected bool imageReady;
    protected bool imageInProcess;
    protected bool imageDisplaying; //refactor this to states, if image is displaying, we dont need to create the sprite anymore

    //for development debugging
    public Image debugImage; //just for now

    // Use this for initialization
    protected virtual void Start ()
    {
        if (!inInventory)
        {
            heightCorrector = currentAssignedGridPlane.transform.up;
            heightCorrector *= transform.localScale.y / 2;
            transform.position = currentAssignedGridPlane.transform.position + heightCorrector;
            currentAssignedGridPlane.taken = true;
        }
       
        movementState = BlockMovementState.Default;
        rotate = false;
        desiredRotation = transform.localRotation;
        degreesToRotate = 90;

        #region  or laserInputUpdate
        inputLasers = LaserManager.Instance.GetInputLasers(this);

        activeLasersLastFrame = new bool[laserInputs.Length];
        imagesLastFrame = new Texture2D[laserInputs.Length];

        for (int i = 0; i < laserInputs.Length; i++)
        {
            activeLasersLastFrame[i] = laserInputs[i].active;
            if (laserInputs[i].inputLaser != null) imagesLastFrame[i] = laserInputs[i].inputLaser.image;
            else imagesLastFrame[i] = null;
        }
        #endregion
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

        UpdateLaserInputs();
        //every child decides here what to do with the input Lasers

        //for debug image
        if (Input.GetKeyDown(KeyCode.I)) ToogleDebugImage();
        if (debugImage != null) debugImage.transform.parent.gameObject.transform.up = Camera.main.transform.up;
    }

    //before returning to inventory some objects needs to deassign some variables or disable lasers
    public virtual void ReturnToInventory()
    {
        currentAssignedGridPlane.taken = false;
        gameObject.SetActive(false);
    }




    public virtual void OnMouseClick()
    {
        //most of the mneed to rotate, if they need something else they just override
        if (!actionBlocked)
        {
            Rotate();
        }
        
    }

    private void UpdateLaserInputs()
    {

        //wir holen uns alle Laser, welche diesen Block trefen
        inputLasers = LaserManager.Instance.GetInputLasers(this);


        foreach (LaserInput laserInput in laserInputs)
        {
            //erstmal alle auf false setzen
            laserInput.active = false;
            laserInput.inputLaser = null;

            //nun schauen wir ob irgend ein Laser einen unserer LaserInputs trifft
            foreach (Laser laser in inputLasers)
            {
                if (Vector3.Angle(laser.laserOutput.forward, laserInput.transform.forward) < 5)
                {
                    laserInput.active = true;
                    laserInput.inputLaser = laser;
                }
            }
        }

        //checken obs ein unterschied zum letztem Frame gibt
        lasersChanged = false;

        for (int i = 0; i < laserInputs.Length; i++)
        {
            if (activeLasersLastFrame[i] != laserInputs[i].active)
            {
                lasersChanged = true;
            }
            else
            {
                if (laserInputs[i].inputLaser == null)
                {
                    if (imagesLastFrame[i] != null) lasersChanged = true;
                }
                else
                {
                    if (imagesLastFrame[i] != laserInputs[i].inputLaser.image) lasersChanged = true;
                }
            }

        }

        //dieses frame fürs nächste speichern
        for (int i = 0; i < laserInputs.Length; i++)
        {
            activeLasersLastFrame[i] = laserInputs[i].active;
            if (laserInputs[i].inputLaser != null) imagesLastFrame[i] = laserInputs[i].inputLaser.image;
            else imagesLastFrame[i] = null;
        }

    }

    //is called by a laser which wants to know if it has hitted an input
    public bool HittedInput(Laser laserToCheck)
    {
        bool hittedInput = false;

        foreach(LaserInput laserInput in laserInputs)
        {
            if (laserInput.inputLaser == laserToCheck) hittedInput = true;
        }

        return hittedInput;
    }

    void ToogleDebugImage()
    {
        if (debugImage != null)
        {
            if (debugImage.gameObject.activeSelf)
            {
                debugImage.gameObject.SetActive(false);
            }
            else
            {
                debugImage.gameObject.SetActive(true);
            }
        }
    }

    #region image processing

    protected virtual void StartImageProcessing()
    {
        //startet das Image Processing welches über mehrere Frames in dem Enumerator läuft
        imageReady = false;
        imageDisplaying = false;
        imageInProcess = true;

        StartCoroutine("ImageProcessingEnumerator");
    }

    protected virtual void StopImageProcessing()
    {
        //is called when the lasr leaves the node - > active image processing is stoppen and the image is deleted
        imageReady = false;
        imageInProcess = false;
        StopCoroutine("ImageProcessingEnumerator");
    }

    #endregion

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

    //diese Funktion sorgt dafür dass unser Block nach der bewegung schön weich landet
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
