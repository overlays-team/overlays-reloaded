using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public GameObject inventoryButtonPrefab;
    public InventoryItem[] items;
    private InventoryButton[] inventoryButtons;
    public GridPlane inventoryGridPlane; //eine einfache wenn auch nicht so elegante Lösung - spart aber um die 70 Zeilen code
    
	void Awake () {

        inventoryButtons = new InventoryButton[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            //instantiate the blockObjects we need for our Inventory
            for (int n = 0; n < items[i].blockAmount; n++)
            {
                GameObject block = Instantiate(items[i].blockObjectPrefab);
                block.SetActive(false);
                BlockObject blockObject = block.GetComponent<BlockObject>();
                blockObject.currentAssignedGridPlane = inventoryGridPlane;
                blockObject.inInventory = true;
                items[i].AddBlockObject(blockObject);
            }
            

            //now instantiate all the buttons we need
            GameObject button = Instantiate(inventoryButtonPrefab);
            button.transform.SetParent(transform);
            button.transform.GetChild(0).GetComponent<Text>().text = items[i].blockAmount.ToString();
            button.GetComponent<Image>().sprite = items[i].icon;
            InventoryButton buttonScript = button.GetComponent<InventoryButton>();
            buttonScript.inventory = this;
            inventoryButtons[i] = buttonScript;
        }
    }

    public void TakeItemFromInventory(InventoryButton clickedButton)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (inventoryButtons[i] == clickedButton )
            {
                if (items[i].blockAmount > 0)
                {
                    PlayerController.Instance.GetObjectFromInventory(items[i].RemoveBlockObject());
                    UpdateItems();
                } 
            }
        }
    }

    public void  ReturnItemToInventory(BlockObject blockObject)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].CheckIfBlockBelongsToThisItem(blockObject))
            {
                items[i].ReturnBlockObject(blockObject);
                UpdateItems();
            }
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
