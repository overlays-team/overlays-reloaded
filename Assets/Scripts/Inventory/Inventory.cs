using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public InventoryItem[] items;
    public InventoryButton[] inventoryButtons;
    public GridPlane inventoryGridPlane; //eine einfache wenn auch nicht so elegante Lösung - spart aber um die 70 Zeilen code
    
	void Awake () {

        foreach(InventoryButton button in inventoryButtons)
        {
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < items.Length; i++)
        {
            //instantiate the blockObjects we need for our Inventory if not infinite
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

            inventoryButtons[i].gameObject.SetActive(true);
            inventoryButtons[i].gameObject.transform.GetChild(0).GetComponent<Text>().text = items[i].blockAmount.ToString();
            inventoryButtons[i].gameObject.GetComponent<Image>().sprite = items[i].blockObjectPrefab.GetComponent<BlockObject>().inventoryIcon;
            inventoryButtons[i].inventory = this;
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
        blockObject.currentAssignedGridPlane = inventoryGridPlane;
        items[blockObject.inventoryIndex].ReturnBlockObject(blockObject);
        UpdateItems();
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
