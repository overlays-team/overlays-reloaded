using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockObject : MonoBehaviour
{

    /*
     * The basic block, every blockObject like morrors or filters derive from this.
     * It has a collection of lasers which hit it.
     * The children decide what to do with them and how many they use
     */

    #region Variables


    [Header("Gameplay")]
    [Tooltip("if this is true - then we cant move the object from its position - good for some puzzle objects like walls etc")]
    public bool stationary = false;
    [Tooltip("if this is true we cant perform the onClickAction - rotate for all cases so far, it sets automaticly with stationary")]
    public bool actionBlocked = false;
    [Tooltip("if this is true we cant perform the onDoubleClickAction - only used in sandbox mode so far")]
    public bool doubleClickActionBlocked = false;
    [Space(5)]
    [SerializeField]
    [Tooltip("this panel shows the detailed node view and can contain further options for settings in the nodes - used for sandbox mode")]
    protected GameObject detailedNodeView;
    [SerializeField]
    protected Image detailedNodeViewImage;


    #region positioning variables
    [HideInInspector]
    public GridPlane currentAssignedGridPlane;
    protected Vector3 heightCorrector; //vector which helps with the positioning, is half the length of the blockObjects height
    #endregion

    #region smooth movement variables

    enum BlockMovementState
    {
        Default, //Block lies on the ground
        Moving,
        Dropping // when we release the block and it slowly falls down
    }

    BlockMovementState movementState;

    //for smoothed rotation
    Quaternion desiredRotation;
    protected float degreesToRotate ; //how many degrees do we rotate 90 or 45? or something completely different - 90 for the current game
    bool rotate;


    //for smooth block dragging
    Vector3 targetDragPosition;
    #endregion

    #region  laser Logic variables 

    [Header("Laser Logic")]
    [SerializeField]
    [Tooltip("assign the laserInputs here")]
    protected LaserInput[] laserInputs;

    [Tooltip("only one per default, children can create more if needed")]
    [SerializeField]
    protected LaserOutput laserOutput;

    protected List<Laser> inputLasers;
    bool[] activeLasersLastFrame;
    Texture2D[] imagesLastFrame;

    protected int laserInputMaxIncidenceAngle = 5; //which is the biggest angle under which a laser goes into an input and still gets accepted by it

    protected bool lasersChanged; //did the laserInputs change last frame?
    #endregion

    #region graphics 
    [Header("Graphics")]
    [SerializeField]
    [Tooltip("does not need to be assignes in every block, only used by some of them")]
    protected GameObject graphics;
    [Tooltip("the canvas which holds the image and the icon of the block, only on of them is visible once at a time")]
    public GameObject imageCanvas;
    [SerializeField]
    [Tooltip("both frames should be assigned for moveable blocks")]
    protected LineRenderer frame;
    [SerializeField]
    [Tooltip("both frames should be assigned for moveable blocks")]
    protected GameObject stationaryFrame;
    [Tooltip("the processed image which is visible on the block, if we have one")]
    public Image debugImage;
    [Tooltip("the image which is diplayed on the block ,when we do not have a processed image, an icon mostly")]
    public GameObject blockImage;

    #endregion

    #region variables only only used by invenory objects
    [HideInInspector]
    public bool inInventory = false; //if its in the inventory it wont perform the standard start function
    [HideInInspector]
    public int inventoryIndex; //index zu welchem Invenotry item/button dieser BLock gehört, damit er seinen Weg zurückfindet
    [Tooltip("the icon which will represent this blockObject in the inventory")]
    public Sprite inventoryIcon;
    #endregion

    #region  image variables processing

    protected enum ImageProcessingState
    {
        NoImage, //no image is being processed or displyed
        Ready,  //image was calculated and waits to be displayed
        Processing, //image is being calculated and cant be displayed right now
        Displaying //image was calculate and is displaying on top of the blockObject in the "debugImage"
    }

    protected ImageProcessingState imageProcessingState = ImageProcessingState.NoImage;

    //every block can have a maximum of 4
    protected Texture2D inputImage1;
    protected Texture2D inputImage2;
    protected Texture2D inputImage3;
    protected Texture2D inputImage4;

    //some blocks can have more than one but they will change this on their own
    protected Texture2D outputImage;
    [Tooltip("the bigger, the faster the images process - 10 is default")]
    public int imageProcessingTime = 10;

    protected float resolutionDifference; //if we are using 2 images to calculate the output we use this to scale the smaller up

    #endregion

    #endregion

    #region SetUp


    protected virtual void Start ()
    {
        #region position set up
        if (currentAssignedGridPlane != null) BlockPositionSetUp(currentAssignedGridPlane);
        #endregion

        #region smooth movement set up
        movementState = BlockMovementState.Default;
        rotate = false;
        desiredRotation = transform.localRotation;
        degreesToRotate = 90;

        #endregion

        #region  laser logic Set up
        inputLasers = LaserManager.Instance.GetInputLasers(this);

        activeLasersLastFrame = new bool[laserInputs.Length];
        imagesLastFrame = new Texture2D[laserInputs.Length];

        for (int i = 0; i < laserInputs.Length; i++)
        {
            activeLasersLastFrame[i] = laserInputs[i].active;
            if (laserInputs[i].inputLaser != null) imagesLastFrame[i] = laserInputs[i].inputLaser.image;
            else imagesLastFrame[i] = null;
        }

        if(laserOutput!=null) laserOutput.active = false;
        #endregion
    }

    public void BlockPositionSetUp(GridPlane assignedGridPlane)
    {
        //set the height corrector - corrects the height of objects on gridPlanes
        heightCorrector = assignedGridPlane.transform.up;

        heightCorrector *= transform.localScale.y / 2;
        if (!inInventory)
        {
            transform.position = assignedGridPlane.transform.position + heightCorrector;
            assignedGridPlane.taken = true;
        }
    }

    #endregion

    protected virtual void Update()
    {
        SmoothMovementUpdate();
        UpdateLaserInputs();
        //reposition the image shown above our object
        if (imageCanvas != null)
        {
            imageCanvas.transform.rotation = Camera.main.transform.rotation;
            if (!debugImage.isActiveAndEnabled && blockImage != null) blockImage.SetActive(true);
            else if (blockImage != null) blockImage.SetActive(false);
        }

        //every child decides here what to do with their lasers
    }

    //editor only code - enables or disables the stationary frame
    protected void OnValidate()
    {
        if (stationaryFrame != null && frame != null)
        {
            if (stationary)
            {
                actionBlocked = true;
                stationaryFrame.gameObject.SetActive(true);
                frame.gameObject.SetActive(false);
                if(graphics.GetComponent<Animator>()!=null)graphics.GetComponent<Animator>().enabled = false;
            }
            else
            {
                actionBlocked = false;
                stationaryFrame.gameObject.SetActive(false);
                frame.gameObject.SetActive(true);
            }
        } 
    }

    #region inventory

    public virtual void ReturnToInventory()
    {
        //before returning to inventory some objects needs to deassign some variables or disable lasers
        currentAssignedGridPlane.taken = false;
        gameObject.SetActive(false);
    }

    //is used by the sandbox inventory to delete the objects
    public void OnDestroy()
    {
        if(laserOutput!=null && laserOutput.laser!=null) Destroy(laserOutput.laser.gameObject);
    }

    #endregion

    #region detailed Node View

    public void OnTwoFingerTap()
    {
        //most of the need to rotate, if they need something else they just override
        if (!doubleClickActionBlocked && imageProcessingState == ImageProcessingState.Displaying)
        {
            TwoFingerTapAction();
        }
    }

    protected virtual void TwoFingerTapAction()
    {
        detailedNodeView.SetActive(true);
        if (IngameManager.Instance != null)
        {
            if (IngameManager.Instance.currentState != IngameManager.IngameManagerState.Review) IngameManager.Instance.PauseGame();
        }
        else if(TimeAttackManager.Instance != null)
        {
            if (TimeAttackManager.Instance.currentState != TimeAttackManager.TimeAttackState.Review) TimeAttackManager.Instance.LockPlayerController();
        }
    }

    //when we click the return Button in the detailedNode view
    public void OnReturnClicked()
    {
        detailedNodeView.SetActive(false);
        if (IngameManager.Instance != null)
        {
            if (IngameManager.Instance.currentState != IngameManager.IngameManagerState.Review) IngameManager.Instance.ResumeGame();
        }
        else if (TimeAttackManager.Instance != null)
        {
            if (TimeAttackManager.Instance.currentState != TimeAttackManager.TimeAttackState.Review) TimeAttackManager.Instance.UnlockPlayerController();
        }
    }

    #endregion

    #region graphics code

    protected void Grow()
    {
        graphics.GetComponent<Animator>().SetBool("LaserInput", true);
    }

    protected void Shrink()
    {
            graphics.GetComponent<Animator>().SetBool("LaserInput", false);
    }

    #endregion

    #region image processing code

    protected virtual void StartImageProcessing()
    {
        //starts the image processing, which calculates over several frames via an Enumerator

        //the outputImage is a copy of the bigger image at first, the smaller image gets scaled up
        if (inputImage2 == null)
        {
            outputImage = Instantiate(inputImage1);
        }
        else if(inputImage2.width> inputImage1.width)
        {
            outputImage = Instantiate(inputImage2);
            resolutionDifference = inputImage2.width / inputImage1.width;
        }
        else
        {
            outputImage = Instantiate(inputImage1);
            resolutionDifference = inputImage1.width / inputImage2.width;
        }

        imageProcessingState = ImageProcessingState.Processing;

        StartCoroutine("ImageProcessingEnumerator");
    }

    protected virtual void StopImageProcessing()
    {
        //is called when the laser leaves the node - > active image processing is stopped and the image is deleted
        imageProcessingState = ImageProcessingState.NoImage;
        StopCoroutine("ImageProcessingEnumerator");
    }

    IEnumerator ImageProcessingEnumerator()
    {
        for (int y = 0; y < outputImage.height; y++)
        {
            for (int x = 0; x < outputImage.width; x++)
            {
                outputImage.SetPixel(x, y, ProcessPixel(x,y));
            }
            if (y % imageProcessingTime == 0) yield return null;
        }
        outputImage.Apply();

        imageProcessingState = ImageProcessingState.Ready;
    }

    //this gets called on every pixel, every image processing block overrides this
    protected virtual Color ProcessPixel(int x, int y)
    {
        return new Color();
    }

    protected void UpdateOutputImageDisplayAndSendImageThroughLaser()
    {
        if (imageProcessingState != ImageProcessingState.Displaying)
        {
            if (imageProcessingState == ImageProcessingState.Ready)
            {
                laserOutput.laser.image = outputImage;
                laserOutput.active = true;

                debugImage.gameObject.SetActive(true);
                debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, outputImage.width, outputImage.height), new Vector2(0.5f, 0.5f));
                if (detailedNodeViewImage != null)
                {
                    detailedNodeViewImage.gameObject.SetActive(true);
                    detailedNodeViewImage.sprite = debugImage.sprite;
                }
                imageProcessingState = ImageProcessingState.Displaying;
            }
            else
            {
                debugImage.gameObject.SetActive(false);
                laserOutput.active = false;
                if (detailedNodeViewImage != null) detailedNodeViewImage.gameObject.SetActive(false);
            }
        }
    }



    #endregion

    # region laser logic code
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
                if (Vector3.Angle(laser.transform.forward, laserInput.transform.forward) < laserInputMaxIncidenceAngle)
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

        //dieses frame für den check im nächsten frame speichern
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

    #endregion

    #region smooth movement code

    public virtual void OnMouseClick()
    {
        //most of the need to rotate, if they need something else they just override
        if (!actionBlocked)
        {
            Rotate();
        }
    }

    void SmoothMovementUpdate()
    {
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
    }

    void Rotate()
    {
        rotate = true;
        desiredRotation = desiredRotation*Quaternion.Euler(0, degreesToRotate, 0);
    }

    void PerformRotation()
    {
        if (desiredRotation.eulerAngles != transform.rotation.eulerAngles)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, desiredRotation, PlayerController.Instance.blockRotationSpeed * Time.deltaTime);

        }
        else
        {
            rotate = false;
        }
    }

    //this function allows our blocks to land softly on gridPlanes
    public void SetPositionToSnapTo(GridPlane gridPlane)
    {
        movementState = BlockMovementState.Dropping;
        if (currentAssignedGridPlane != null) currentAssignedGridPlane.taken = false;
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
        }
        else
        {
            movementState = BlockMovementState.Default;
        }

    }

    //this function allows our blockObject to move smoothly to our finger while dragging
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
