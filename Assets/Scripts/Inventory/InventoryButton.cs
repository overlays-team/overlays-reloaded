using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//simple button, which tells the inventory when it is pressed
public class InventoryButton : MonoBehaviour, IPointerDownHandler
{
    public Inventory inventory;

    [Tooltip("this automaticly changes the inventory icon size on tablet")]
    public LayoutElement layoutElement;

    private void Start()
    {
        if (Screen.height *1f/ Screen.width < 1.5)
        {
            if (layoutElement != null)
            {
                layoutElement.minHeight = 120;
                layoutElement.minWidth = 120;
                transform.parent.GetComponent<HorizontalLayoutGroup>().spacing = 70;
            }
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //inventory.TakeItemFromInventory(this); old way

        inventory.PlayerSelectedThisButton(this);
    }
}
