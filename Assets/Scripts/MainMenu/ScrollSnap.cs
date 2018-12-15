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

    public List<RectTransform> items;
    public Transform itemContainer;

    private RectTransform rect;

    private float focusItem;
    private float itemDistance;
    private int minItem;
    private bool dragging = false;
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

        itemDistance = items[1].anchoredPosition.x - items[0].anchoredPosition.x;
        Debug.Log("itemDistance: " + itemDistance);
        Debug.Log("item 1 x: " + items[1].GetComponent<RectTransform>().anchoredPosition.x);
        Debug.Log("item 2 x: " + items[0].GetComponent<RectTransform>().anchoredPosition.x);
    }

    // Update is called once per frame
    void Update()
    {
        selectedItem = (int) Mathf.Round(Mathf.Clamp(-scrollSlide.transform.position.x / itemDistance, 0, items.Count - 1));

        ScaleSelected();

        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                distances[i] = Mathf.Abs(rect.position.x - items[i].position.x);
            }

            float minDistance = Mathf.Min(distances);
            minItem = Array.FindIndex(distances, distance => distance == minDistance);

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
                    Debug.Log(minItem * -itemDistance);
                    LerpToPosition(minItem * -itemDistance);
                }
            }
        }
	}

    private void ScaleSelected()
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (i == selectedItem)
            {
                items[i].localScale = Vector3.Lerp(items[i].localScale, selectedScaleFactor, selectedScaleSpeed);
            }
            else
            {
                items[i].localScale = Vector3.Lerp(items[i].localScale, new Vector3(1, 1, 1), selectedScaleSpeed);
            }
        }
    }

    public void FocusLeftItem()
    {
        if (float.IsNaN(focusItem))
        {
            focusItem = (minItem - 1) * -itemDistance;       
        }
    }

    public void FocusRightItem()
    {
        if (float.IsNaN(focusItem))
        {
            focusItem = (minItem + 1) * -itemDistance;
        }
    }

    private void LerpToPosition(float position)
    {
        float newX = Mathf.Lerp(scrollSlide.anchoredPosition.x, position, Time.deltaTime * snapSpeed);

        Vector2 newPosition = new Vector2(newX, scrollSlide.anchoredPosition.y);

        scrollSlide.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }
}
