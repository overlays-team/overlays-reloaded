using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    float timeOfLastClick;
    float clickTime;
    public float timeToHoldToInitiateHoldAction;

    public float blockRotationSpeed;
    public float blockDragSpeed;
    public float blockSnappingSpeed;
    public float blockLiftingHeight;

    BlockObject selectedBlockObject; // the object we move during our hold phase
    GridPlane lastHittedGridPlane;

    public Inventory inventory;

    public enum PlayerMode
    {
        Default, //we can click on an object to call it on mouse click action
        MouseHold // we did hold our mouse on the object so now we move it to another position, if we release the object we get back to Default mode
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
    void Start ()
    {
        playerMode = PlayerMode.Default;
	}

    // Update is called once per frame
    void Update()
    {
        switch (playerMode)
        {
            case PlayerMode.Default:

                //if we press the mouse button, we save the object we hitted with the raycast
                if (Input.GetMouseButtonDown(0))
                {
                    //Debug.Log("mouse clicked");
                    timeOfLastClick = Time.time;

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
                            //Debug.Log("step4");
                            hittedObject.OnMouseClick();
                            hittedObject = null;
                        }
                    }
                    clickTime = 0;
                }
                //otherwise we move the object with our mouse/hand while the mouse/finger is held Down
                else if (Input.GetMouseButton(0))
                {
                    clickTime = Time.time - timeOfLastClick;
                    if (clickTime >= timeToHoldToInitiateHoldAction)
                    {
                        if (hittedObject != null && !hittedObject.stationary)
                        {
                            selectedBlockObject = hittedObject;
                            playerMode = PlayerMode.MouseHold;
                        } 
                    }
                }
                break;

            case PlayerMode.MouseHold:


                //when we release the mouse, the object will be placed
                if (Input.GetMouseButtonUp(0))
                {
                    if (clickTime >= timeToHoldToInitiateHoldAction)
                    {
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        // Bit shift the index of the layer (9) to get a bit mask
                        int layerMask = 1 << 9;

                        if (Physics.Raycast(ray, out hit, 100, layerMask))
                        {
                            hittedPlane = hit.collider.gameObject.GetComponent<GridPlane>();
                            if (hittedPlane != null)
                            {
                                if (!hittedPlane.taken)
                                {
                                    //SnapPosition(selectedBlockObject,hittedPlane);
                                    selectedBlockObject.SnapToPosition(hittedPlane);
                                    if (lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();
                                }
                                else
                                {
                                    //SnapPosition(selectedBlockObject, selectedBlockObject.currentAssignedGridPlane);
                                    selectedBlockObject.SnapToPosition(selectedBlockObject.currentAssignedGridPlane);
                                    if (lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();
                                }

                            }
                            else
                            {
                                if (!selectedBlockObject.inInventory)
                                {
                                    //SnapPosition(selectedBlockObject, selectedBlockObject.currentAssignedGridPlane);
                                    selectedBlockObject.SnapToPosition(selectedBlockObject.currentAssignedGridPlane);             
                                }
                                else
                                {
                                    PutObjectBackToInventory();
                                }
                                if (lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();

                            }
                        }else
                        {
                            //SnapPosition(selectedBlockObject, selectedBlockObject.currentAssignedGridPlane);
                            selectedBlockObject.SnapToPosition(selectedBlockObject.currentAssignedGridPlane);
                            if (lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();
                        }
                    }


                    playerMode = PlayerMode.Default;
                    selectedBlockObject = null;
                    hittedObject = null;
                    clickTime = 0;
                }
                //welse while we hold the mouse button, the grids where we can position the Object, will be marked with a green or red halo
                else if (Input.GetMouseButton(0))
                {
                    clickTime = Time.time - timeOfLastClick;

                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // Bit shift the index of the layer (9) to get a bit mask
                    int layerMask = 1 << 9;

                    if (Physics.Raycast(ray, out hit, 100, layerMask))
                    {
                        selectedBlockObject.SetMovePosition(hit.point + (hit.collider.gameObject.transform.up*blockLiftingHeight));

                        hittedPlane = hit.collider.gameObject.GetComponent<GridPlane>();
                        if (hittedPlane != null)
                        {
                            if(lastHittedGridPlane!= hittedPlane)
                            {
                                if(lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();
                                if(hittedPlane!= selectedBlockObject.currentAssignedGridPlane) hittedPlane.ShowHalo();
                                lastHittedGridPlane = hittedPlane;
                            }
                        }else
                        {
                            if (lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();
                        }
                    }else
                    {
                        if (lastHittedGridPlane != null) lastHittedGridPlane.HideHalo();
                    }
                }
          
                break;
        }
        
    }

    public void GetObjectFromInventory(BlockObject selectedBlock)
    {
        this.selectedBlockObject = selectedBlock;
        playerMode = PlayerMode.MouseHold;


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


}
