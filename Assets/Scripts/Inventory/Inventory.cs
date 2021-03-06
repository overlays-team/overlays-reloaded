﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IPointerDownHandler
{

    public InventoryItem[] items;
    public InventoryButton[] inventoryButtons;
    public GridPlane inventoryGridPlane; 

    [SerializeField]
    [Tooltip("in sandbox mode we can scroll through the inventory and instantiate the buttons when we take them, in time attack we only have mrirrors")]
    private InventoryMode inventoryMode = InventoryMode.Normal;

    [Header("only needed if scrollable/sandboxMode on")]
    //whis variables are needed to determine if thw player wants to scroll the inventory(in sandbox mode) or just take an item from it
    public RectTransform rectTransform;
    bool mouseDown = false;
    Vector2 lastMousePosition;
    InventoryButton selectedButton;

   

    enum InventoryMode
    {
        Normal,
        Sandbox,
        TimAttack
    }


    void Awake () {
        
        foreach(InventoryButton button in inventoryButtons)
        {
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (inventoryMode == InventoryMode.Normal)
            {
                //instantiate the blockObjects we need for our Inventory if not sandbox
                for (int n = 0; n < items[i].blockAmount; n++)
                {
                    GameObject block = Instantiate(items[i].blockObjectPrefab);
                    block.SetActive(false);
                    BlockObject blockObject = block.GetComponent<BlockObject>();
                    blockObject.currentAssignedGridPlane = inventoryGridPlane;
                    blockObject.inInventory = true;
                    blockObject.inventoryIndex = i;
                    items[i].AddBlockObject(blockObject);
                }
            }
            

            inventoryButtons[i].gameObject.SetActive(true);
            if (inventoryMode == InventoryMode.Normal) inventoryButtons[i].gameObject.transform.GetChild(0).GetComponent<Text>().text = items[i].blockAmount.ToString();
            else inventoryButtons[i].gameObject.transform.GetChild(0).GetComponent<Text>().text = "∞";
            inventoryButtons[i].gameObject.GetComponent<Image>().sprite = items[i].blockObjectPrefab.GetComponent<BlockObject>().inventoryIcon;
            inventoryButtons[i].inventory = this;
        }
    }

    private void Update()
    {
        if (mouseDown)
        {    
            //terminating condition
            if(Input.mousePosition.y > rectTransform.rect.height*2 || Input.GetMouseButtonUp(0))
            {
                mouseDown = false;
                PlayerController.Instance.enabled = true;
            }

            //do we move our finger to the side -> inventory scroll
            // do we move our finger up? -> get Item from Inventory

            Vector2 mouseChange = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - lastMousePosition;

            if (mouseChange.y > 1 && selectedButton != null && Mathf.Abs(mouseChange.x)<mouseChange.y)
            {
                //if we move our finger up 
                mouseDown = false;
                PlayerController.Instance.enabled = true;
                TakeItemFromInventory(selectedButton);
                selectedButton = null;
            }
            else if ( mouseChange.x > 1||mouseChange.x < -1)
            {
                selectedButton = null;

                rectTransform.anchoredPosition = new Vector2
                (
                    Mathf.Clamp
                    (
                        rectTransform.anchoredPosition.x + mouseChange.x,
                        rectTransform.rect.xMin+(Screen.width/transform.parent.transform.localScale.y),
                        rectTransform.rect.xMax 
                    ),
                    rectTransform.anchoredPosition.y
                 );
            }
            else
            {
                //if our finger stays at the same position
            }

            lastMousePosition = Input.mousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDown();
    }

    void OnPointerDown()
    {
        if (inventoryMode == InventoryMode.Sandbox)
        {
            PlayerController.Instance.enabled = false;
            mouseDown = true;
            lastMousePosition = Input.mousePosition;
        }   
    }

    //this method is called by the button, it messages the inventory that it was clicked
    public void PlayerSelectedThisButton(InventoryButton clickedButton)
    {
        if (inventoryMode != InventoryMode.Sandbox)
        {
            TakeItemFromInventory(clickedButton);

        }
        else
        {
            selectedButton = clickedButton;
            OnPointerDown();
        }
    }

    void TakeItemFromInventory(InventoryButton clickedButton)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (inventoryButtons[i] == clickedButton)
            {
                if (inventoryMode == InventoryMode.Normal)
                {
                    if (items[i].blockAmount > 0)
                    {
                        PlayerController.Instance.GetObjectFromInventory(items[i].RemoveBlockObject());
                        IngameManager.Instance.RaiseMoves();
                        UpdateItems();
                    }
                }
                else
                {
                    BlockObject block = Instantiate(items[i].blockObjectPrefab).GetComponent<BlockObject>();
                    block.currentAssignedGridPlane = inventoryGridPlane;
                    block.inInventory = true;
                    PlayerController.Instance.GetObjectFromInventory(block);
                   if(IngameManager.Instance!=null) IngameManager.Instance.RaiseMoves();
                }

            }
        }
    }

    public void  ReturnItemToInventory(BlockObject blockObject)
    {
        if (inventoryMode == InventoryMode.Normal)
        {
            blockObject.currentAssignedGridPlane = inventoryGridPlane;
            items[blockObject.inventoryIndex].ReturnBlockObject(blockObject);
            UpdateItems();
        }
        else
        {
            Destroy(blockObject.gameObject);
        }
        
    }

    //updates the counter on the buttons
    void UpdateItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            inventoryButtons[i].transform.GetChild(0).GetComponent<Text>().text = items[i].blockAmount.ToString();
        }
    }
}
