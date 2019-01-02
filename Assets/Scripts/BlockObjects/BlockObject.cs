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
     * 
     * Jedes Kind übernimmt die LaserInputs und inputImage 1 bis 4 und outputImage 1 bis 4 von der Vaterklasse, 
     * das blockObjekt hatt defaultmäßig nur einen Laser Output- kinder die mehr wollen, überschreiben
     */

    [Header("Gameplay")]
    [Tooltip("if this is true - then we cant move the object from its position - good for some puzzle objects like walls etc")]
    public bool stationary = false;
    [Tooltip("if this is true we cant perform the onClickAction - rotate for all cases so far")]
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
    protected Vector3 heightCorrector; //Vector der jeweils die Hälfte der Höhe des Objektes beträgt, um ihn auf Planes auf korrekter Höhe aufstellen zu können
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

    [Tooltip("default nur ein output, falls ein kind mehr braucht, kann es sich selber welche setzen")]
    [SerializeField]
    protected LaserOutput laserOutput;

    protected List<Laser> inputLasers;
    bool[] activeLasersLastFrame;
    Texture2D[] imagesLastFrame;

    protected int laserInputMaxIncidenceAngle = 5; //was ist der größte Winkel unter dem ein Laser in einen LaserInputEinfallen kann und trotzdem akzeptiert wird

    protected bool lasersChanged; //did the laserInputs change last frame?
    #endregion

    #region graphics 
    [Header("Graphics")]
    [SerializeField]
    [Tooltip("muss nich bei jedem BlockObjekt assignt sein, wird nicht von jedem genutzt")]
    protected GameObject graphics;
    [SerializeField]
    [Tooltip("muss nich bei jedem BlockObjekt assignt sein, wird nicht von jedem genutzt")]
    protected LineRenderer frame;
    [Tooltip("das Bild, welches auf dem BlockObjekt zu sehen ist")]
    public Image debugImage;

    #endregion

    #region variables only only used by invenory objects
    [HideInInspector]
    public bool inInventory = false; //if its in the inventory it wont perform the standard start function
    [HideInInspector]
    public int inventoryIndex; //index zu welchem Invenotry item/button dieser BLock gehört, damit er seinen Weg zurückfindet
    [Tooltip("the icon which will represent this blockObject in the inventory")]
    public Sprite inventoryIcon;
    #endregion

    #region  image processing

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


    #endregion

    protected virtual void Start ()
    {
        #region position set up
        //setze den hieght Correktor - dieser sorgt dafür, dass alle Blocks jeweils mit ihrer Unterseite auf einem Feld aufliegen und nicht mittendrinn sind
        heightCorrector = currentAssignedGridPlane.transform.up;
        heightCorrector *= transform.localScale.y / 2;
        if (!inInventory)
        {
            transform.position = currentAssignedGridPlane.transform.position + heightCorrector;
            currentAssignedGridPlane.taken = true;
        }
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

    protected virtual void Update()
    {
        SmoothMovementUpdate();
        UpdateLaserInputs();
        //reposition the image shown above our object
        if (debugImage != null) debugImage.transform.parent.gameObject.transform.up = Camera.main.transform.up;

        //every child decides here what to do with their lasers
    }

    public virtual void ReturnToInventory()
    {
        //before returning to inventory some objects needs to deassign some variables or disable lasers
        currentAssignedGridPlane.taken = false;
        gameObject.SetActive(false);
    }

    public virtual void OnMouseClick()
    {
        //most of the need to rotate, if they need something else they just override
        if (!actionBlocked)
        {
            Rotate();
        } 
    }

    #region detailed Node View

    public void OnTwoFingerTap()
    {
        //most of the need to rotate, if they need something else they just override
        if (!doubleClickActionBlocked)
        {
            TwoFingerTapAction();
        }
    }

    protected virtual void TwoFingerTapAction()
    {
        detailedNodeView.SetActive(true);
        PlayerController.Instance.Disable();
    }

    //when we click the return Button in the detailedNode view
    public void OnReturnClicked()
    {
        detailedNodeView.SetActive(false);
        PlayerController.Instance.Enable();
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
        //startet das Image Processing welches über mehrere Frames in dem Enumerator läuft
        outputImage = Instantiate(inputImage1);
        imageProcessingState = ImageProcessingState.Processing;

        StartCoroutine("ImageProcessingEnumerator");
    }

    protected virtual void StopImageProcessing()
    {
        //is called when the lasr leaves the node - > active image processing is stopped and the image is deleted
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
            if (y % 10 == 0) yield return null;
        }
        outputImage.Apply();

        imageProcessingState = ImageProcessingState.Ready;
    }

    //this gets called on every pixel, every image processing block overrides this
    protected virtual Color ProcessPixel(int x, int y)
    {
        return new Color();
    }

    protected void UpdateOutputImageDisplayAndSendThroughLaser()
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
                if (Vector3.Angle(laser.laserOutput.forward, laserInput.transform.forward) < laserInputMaxIncidenceAngle)
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
    public void SetPositionToSnapTo(GridPlane gridPlane)
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
