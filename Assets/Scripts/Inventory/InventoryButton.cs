using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//simple button, which tells the inventory when it is pressed
public class InventoryButton : MonoBehaviour, IPointerDownHandler
{
    public Inventory inventory;

    public void OnPointerDown(PointerEventData eventData)
    {
        //inventory.TakeItemFromInventory(this); old way

        inventory.PlayerSelectedThisButton(this);
    }
}
