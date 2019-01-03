﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool isEnabled = true; //in the pause and when in detailed node view we disable the playerController
    float timeOfLastMouseDown;
    float clickTime;

    public float timeToHoldToInitiateHoldAction;

    public float blockRotationSpeed;
    public float blockDragSpeed;
    public float blockSnappingSpeed;
    public float blockLiftingHeight;

    BlockObject selectedBlockObject; // the object we move during our hold phase
    GridPlane lastHittedGridPlane;

    public Inventory inventory;

    [Header("Camera Movement")]
    public bool cameraMovementEnabled = false;
    public GameObject cameraHolder;
    [Tooltip("Bounds in x and y - 0.1 means when we have a block in our hand and reach the border 10% of our screen - our camera will move")]
    public float autoMovementBorder;
    int autoMovementBorderUp;
    int autoMovementBorderRight;
    int autoMovementBorderLeft;
    int autoMovementBorderDown;
    bool mouseWasOutsideAutoMovementBorder; // this bool prevends the camera sliding downwards when we pick up an item from the inventory 

    public float movementSpeed = 10f;
    public float moveCameraTreshold; //what distance must our finger go in 1 frame, that we start moving the camera
    Vector2 lastMousePosition;

    //pinchZoom - for development puposes it woks with mouse wheel on pc, on touch, its the normal 2 finger zoom
    public float zoomSpeed = 0.5f;
    public float minHeight = 10f;
    public float maxHeight = 20f;
    public float zoomTreshhold; //what distance must the 2 fingers go between 2 frames to start zooming
    float distanceBetweenFingersLastFrame;
    bool didWeTouchLastFrame; //we need this for android- because it does not have a hover mouse for starting the camera movement
    
    

    public enum PlayerMode
    {
        Default, //we can click on an object to call it on mouse click action
        MouseHoldMoveBlock, // we did hold our mouse on the object so now we move it to another position, if we release the object we get back to Default mode
        MouseHoldDragCamera,
        TwoFingerZoom
    }

    PlayerMode playerMode;

    BlockObject hittedObject; //object which we hitted with the mouse
    GridPlane hittedPlane;

    public static PlayerController Instance;

    //Singletoncode
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance); // es kann passieren wenn wir eine neue Scene laden dass immer noch eine Instanz existiert
        }
        else
        {
            Instance = this;
        }
    }



    // Use this for initialization
    void Start()
    {
        playerMode = PlayerMode.Default;

        //for camera movement
        lastMousePosition = Input.mousePosition;
        autoMovementBorderUp = (int)(Screen.height * (1-autoMovementBorder));
        autoMovementBorderRight = (int)(Screen.width * (1 - autoMovementBorder));
        autoMovementBorderLeft = (int)(Screen.width * autoMovementBorder);
        autoMovementBorderDown = (int)(Screen.height * autoMovementBorder);
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {


            switch (playerMode)
            {
                case PlayerMode.Default:

                    //for development purposes
                    if (cameraMovementEnabled)
                    {
                        cameraHolder.transform.position = new Vector3(cameraHolder.transform.position.x, Mathf.Clamp(cameraHolder.transform.position.y - Input.GetAxis("Mouse ScrollWheel")*zoomSpeed/10 * cameraHolder.transform.position.y, minHeight, maxHeight), cameraHolder.transform.position.z);
                    }

                    //if we press the mouse button, we save the object we hitted with the raycast
                    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2)) //wir setzen touch count auf 1, weil sich unser programm sonst nicht entscheiden kann zwischen 2 Fingern
                    {    
                        timeOfLastMouseDown = Time.time;

                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        // Bit shift the index of the layer (9) to get a bit mask
                        int layerMask = 1 << 10;

                        if (Physics.Raycast(ray, out hit, 100, layerMask))
                        {
                            hittedObject = hit.collider.gameObject.GetComponent<BlockObject>();
                        }       
                    }
                    //if we release the mouse key before timeToHoldToInitiateHoldAction - we call the onMouseClickAction of the hittedObject
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (clickTime <= timeToHoldToInitiateHoldAction)
                        {
                            if (hittedObject != null)
                            {
                                if(Input.touchCount==1) hittedObject.OnMouseClick();
                                hittedObject = null;
                            }
                        }
                        clickTime = 0;
                    }
                    //otherwise we move the object with our mouse/hand while the mouse/finger is held Down
                    else if (Input.GetMouseButton(0))
                    {
                        clickTime = Time.time - timeOfLastMouseDown;

                        if(Input.touchCount == 2)
                        {
                            //check if we want to zoom, if our 2 fingers are changing distance between them
                            float distanceBetweenFingersThisFrame = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                            if(distanceBetweenFingersLastFrame!=0 && Mathf.Abs(distanceBetweenFingersThisFrame / distanceBetweenFingersLastFrame) >= zoomTreshhold) playerMode = PlayerMode.TwoFingerZoom;
                            //if not, check if we want to get the detailed node view of an image, for this we send 2 raycasts, on of this fingers must work
                            else
                            {
                                 hittedObject.OnTwoFingerTap();
                            }
                        }
                        else
                        {
                            if (clickTime >= timeToHoldToInitiateHoldAction)
                            {
                                if (hittedObject != null && !hittedObject.stationary)
                                {
                                    selectedBlockObject = hittedObject;
                                    playerMode = PlayerMode.MouseHoldMoveBlock;
                                }
                            }else if (cameraMovementEnabled)
                            {
                                /*we can also move the camera while haveing the finger over a block if we are fast enough,
                                 * but for this in touch we need to make sure, that we touched the display last frame, bacause 
                                 * if not, we will use our old position for determining the finger movement, 
                                 * which could be on the other side of the sreen, thus enabling camera movement on the 
                                 * first frame of our touch down*/

                                if (hittedObject != null && didWeTouchLastFrame)
                                {
                                    if (Vector2.Distance(Input.mousePosition, lastMousePosition) / Screen.width * 1000 > moveCameraTreshold) playerMode = PlayerMode.MouseHoldDragCamera;
                                }
                                else if (hittedObject == null)
                                {
                                    playerMode = PlayerMode.MouseHoldDragCamera;
                                }
                            }
                        }
                    }
                    //for development instead of 2 finger tap - middle mous button
                    else if (Input.GetMouseButtonUp(2))
                    {
                        if(hittedObject != null)
                        {
                            hittedObject.OnTwoFingerTap();
                            hittedObject = null;
                        }
                    }
                    break;

                case PlayerMode.MouseHoldMoveBlock:
                   
                    //when camera movement enabled we move when we hold the block to the sides
                    if (cameraMovementEnabled)
                    {
                        if (mouseWasOutsideAutoMovementBorder)
                        {
                            Vector3 camMove = Vector3.zero;
                            if (Input.mousePosition.x > autoMovementBorderRight) camMove += Vector3.right;
                            else if (Input.mousePosition.x < autoMovementBorderLeft) camMove += -Vector3.right;
                            if (Input.mousePosition.y > autoMovementBorderUp) camMove += Vector3.forward;
                            else if (Input.mousePosition.y < autoMovementBorderDown) camMove += -Vector3.forward;

                            cameraHolder.transform.position += camMove * movementSpeed / 50 * cameraHolder.transform.position.y;
                        }
                        else
                        {
                            if (Input.mousePosition.x < autoMovementBorderRight
                            && Input.mousePosition.x > autoMovementBorderLeft
                            && Input.mousePosition.y < autoMovementBorderUp
                            && Input.mousePosition.y > autoMovementBorderDown) mouseWasOutsideAutoMovementBorder = true;
                        }
                        
                    }

                    //when we release the mouse, the object will be placed
                    if (Input.GetMouseButtonUp(0))
                    {

                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        // Bit shift the index of the layer (9) to get a bit mask
                        int layerMask = 1 << 9;

                        if (Physics.Raycast(ray, out hit, 100, layerMask))
                        {
                            if (lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();
                            hittedPlane = hit.collider.gameObject.GetComponent<GridPlane>();
                            if (hittedPlane != null)
                            {
                                if (!hittedPlane.taken || hittedPlane == selectedBlockObject.currentAssignedGridPlane)
                                {
                                    selectedBlockObject.SetPositionToSnapTo(hittedPlane);
                                }
                                else
                                {
                                    if (!selectedBlockObject.inInventory)
                                    {
                                        selectedBlockObject.SetPositionToSnapTo(selectedBlockObject.currentAssignedGridPlane);
                                    }
                                    else
                                    {
                                        PutObjectBackToInventory();
                                    }
                                }

                            }
                            else
                            {
                                if (!selectedBlockObject.inInventory)
                                {
                                    selectedBlockObject.SetPositionToSnapTo(selectedBlockObject.currentAssignedGridPlane);
                                }
                                else
                                {
                                    PutObjectBackToInventory();
                                }
                            }


                        }
                        else
                        {
                            Debug.Log("raycast didnt hit anything, try scaling the background plane up");
                            if (lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();
                        }



                        playerMode = PlayerMode.Default;
                        selectedBlockObject = null;
                        hittedObject = null;
                        clickTime = 0;
                    }
                    //welse while we hold the mouse button, the grids where we can position the Object, will be marked with a green or red halo
                    else if (Input.GetMouseButton(0))
                    {
                        clickTime = Time.time - timeOfLastMouseDown;

                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        // Bit shift the index of the layer (9) to get a bit mask
                        int layerMask = 1 << 9;

                        if (Physics.Raycast(ray, out hit, 100, layerMask))
                        {
                            selectedBlockObject.SetMovePosition(hit.point + (hit.collider.gameObject.transform.up * blockLiftingHeight));

                            hittedPlane = hit.collider.gameObject.GetComponent<GridPlane>();

                            if (hittedPlane != null)
                            {
                                if (lastHittedGridPlane != hittedPlane)
                                {
                                    if (lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();
                                    if (hittedPlane != selectedBlockObject.currentAssignedGridPlane)
                                    {
                                        hittedPlane.ShowHalo();
                                    }
                                    lastHittedGridPlane = hittedPlane;
                                }
                            }
                            else
                            {
                                if (lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();
                                lastHittedGridPlane = null;
                            }
                        }
                        else
                        {
                            if (lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();
                        }
                    }

                    break;

                case PlayerMode.MouseHoldDragCamera:
                    Debug.Log("drag camera");
                    Vector2 currentPos = Input.mousePosition;
                    Vector3 moveVector = new Vector3(lastMousePosition.x - currentPos.x, 0, lastMousePosition.y - currentPos.y);

                    cameraHolder.transform.position += moveVector * movementSpeed / 2000  * cameraHolder.transform.position.y;

                    if (Input.GetMouseButtonUp(0)) playerMode = PlayerMode.Default;

                    break;

                case PlayerMode.TwoFingerZoom:

                    if (Input.touchCount != 2)
                    {
                        playerMode = PlayerMode.Default;
                    }
                    else
                    {
                        float distanceBetweenFingersThisFrame = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                        
                        float difference = 1-(distanceBetweenFingersLastFrame / distanceBetweenFingersThisFrame);
                        if (difference > 0) Debug.Log("zoom in");
                        else if (difference < 0) Debug.Log("zoom out");
                        else Debug.Log("no zoom");

                        cameraHolder.transform.position = new Vector3(
                                                                      cameraHolder.transform.position.x, 
                                                                      Mathf.Clamp(cameraHolder.transform.position.y - difference * zoomSpeed, minHeight, maxHeight), 
                                                                      cameraHolder.transform.position.z
                                                                     );
                    }

                    break;
            }

            lastMousePosition = Input.mousePosition;
            if(Input.GetMouseButton(0)) didWeTouchLastFrame = true;
            else didWeTouchLastFrame = false;

            if (Input.touchCount == 2) distanceBetweenFingersLastFrame = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            else distanceBetweenFingersLastFrame = 0;
        }
    }

    public void GetObjectFromInventory(BlockObject selectedBlock)
    {
        this.selectedBlockObject = selectedBlock;
        playerMode = PlayerMode.MouseHoldMoveBlock;
        mouseWasOutsideAutoMovementBorder = false; //set to false on the beginning, so the camera does not move downwards when we  pcik up an item


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Bit shift the index of the layer (9) to get a bit mask
        int layerMask = 1 << 9;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            selectedBlockObject.transform.position = hit.point + (hit.collider.gameObject.transform.up * blockLiftingHeight);
        }

        selectedBlockObject.gameObject.SetActive(true);
    }

    public void PutObjectBackToInventory()
    {
        selectedBlockObject.ReturnToInventory();
        inventory.ReturnItemToInventory(selectedBlockObject);
        selectedBlockObject = null;
        playerMode = PlayerMode.Default;
    }

    public void Disable()
    {
        isEnabled = false;
    }

    public void Enable()
    {
        isEnabled = true;
    }


}
