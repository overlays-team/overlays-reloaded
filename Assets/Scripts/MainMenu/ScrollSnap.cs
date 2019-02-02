using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSnap : MonoBehaviour
{
    public RectTransform scrollSlide;
    public int snapSpeed;
    public float selectedScaleSpeed;
    public Vector3 selectedScaleFactor;
    public int selectedItem;
    public int lastSelectedItem;
    public List<RectTransform> items;
    public Transform itemContainer;

    private RectTransform rect;
    private float focusItem;
    private float itemDistance;
    private bool dragging = false;
    private Vector2 dragStartPos;
    private Vector2 dragEndPos;
    private float[] distances;

	// Use this for initialization
	void Start ()
    {
        focusItem = float.NaN;

        rect = GetComponent<RectTransform>();

        for (int i = 0; i < itemContainer.childCount; i++)
        {
            items.Add(itemContainer.GetChild(i).GetComponent<RectTransform>());
        }

        distances = new float[items.Count];

        if(items.Count>1)itemDistance = items[1].anchoredPosition.x - items[0].anchoredPosition.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ScaleSelected();

        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                distances[i] = Mathf.Abs(rect.position.x - items[i].position.x);
            }

            float minDistance = Mathf.Min(distances);
            selectedItem = Array.FindIndex(distances, distance => distance == minDistance);

            if (!dragging)
            {
                if (!float.IsNaN(focusItem))
                {
                    LerpToPosition(focusItem);
                    if (Mathf.Abs(Mathf.Abs(scrollSlide.anchoredPosition.x) - Mathf.Abs(focusItem)) < 1)
                    {
                        focusItem = float.NaN;
                    }
                }
                else
                {
                    LerpToPosition(selectedItem * -itemDistance);
                }
            }

            for(int i = 0; i < items.Count; i++)
            {
                if (items[i].tag == "TutorialImage" && i == selectedItem)
                {
                    items[i].SetSiblingIndex(items.Count - 1);
                }
            }
        }
	}

    private void ScaleSelected()
    {
        int itemsLength = items.Count;
        for(int i = 0; i < itemsLength; i++)
        {
            if (i == selectedItem)
            {
                if(!items[i].localScale.Equals(selectedScaleFactor))
                {
                    if (Vector3.Distance(items[i].localScale, selectedScaleFactor) > 0.1f)
                    {
                        items[i].localScale = Vector3.Lerp(items[i].localScale, selectedScaleFactor, selectedScaleSpeed);
                    }
                    else
                    {
                        items[i].localScale = selectedScaleFactor;
                    }
                }
            }
            else
            {
                if (!items[i].localScale.Equals(Vector3.one))
                {
                    if (Vector3.Distance(items[i].localScale, Vector3.one) > 0.1f)
                    {
                        items[i].localScale = Vector3.Lerp(items[i].localScale, Vector3.one, selectedScaleSpeed);
                    }
                    else
                    {
                        items[i].localScale = Vector3.one;
                    }
                }
            }
        }
    }

    public void FocusLeftItem()
    {
        if (float.IsNaN(focusItem))
        {
            focusItem = (selectedItem - 1) * -itemDistance;       
        }
    }

    public void FocusRightItem()
    {
        if (float.IsNaN(focusItem))
        {
            focusItem = (selectedItem + 1) * -itemDistance;
        }
    }
    
    private void LerpToPosition(float position)
    {
        float newX = Mathf.Lerp(scrollSlide.anchoredPosition.x, position, Time.deltaTime * snapSpeed);

        if(Mathf.Abs(scrollSlide.anchoredPosition.x - position) < 0.1)
        {
            scrollSlide.anchoredPosition = new Vector2(position, scrollSlide.anchoredPosition.y);
        }

        if(scrollSlide.anchoredPosition.x == position)
        {
            lastSelectedItem = selectedItem;
        }
        else
        {
            Vector2 newPosition = new Vector2(newX, scrollSlide.anchoredPosition.y);
            scrollSlide.anchoredPosition = newPosition;
        }
    }

    public void StartDrag()
    {
        dragging = true;
        dragStartPos = Input.mousePosition;
    }

    public void EndDrag()
    {
        dragging = false;
        dragEndPos = Input.mousePosition;
        
        float dragDistance = dragStartPos.x - dragEndPos.x;

        if(lastSelectedItem == selectedItem)
        {
            if (dragDistance > 0 && dragDistance < itemDistance * 0.7)
            {
                FocusRightItem();
            }
            else if (dragDistance < 0 && Mathf.Abs(dragDistance) < itemDistance * 0.7)
            {
                FocusLeftItem();
            }
        }
    }
}
