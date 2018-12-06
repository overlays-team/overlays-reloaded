using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInventar : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        Debug.Log("Pressed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        gameObject.GetComponentInParent<Inventar>().SetABlockIsChosen(false);
        Debug.Log("Released");
    }

    public bool isPressed()
    {
        return this.pressed;
    }
}
