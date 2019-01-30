using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollIndicator : MonoBehaviour
{
    public GameObject tutorialModeScrollSnap;
    public GameObject indicatorPrefab;
    public Sprite emptyGraphic;
    public Sprite filledGraphic;

    public Transform imagesContainer;
    private ScrollSnap scrollSnap;
    private int selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        scrollSnap = tutorialModeScrollSnap.GetComponent<ScrollSnap>();
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        for (int i = 0; i < imagesContainer.childCount; i++)
        {
            Instantiate(indicatorPrefab, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        selectedItem = scrollSnap.selectedItem;
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform current = gameObject.transform.GetChild(i);
            if (i == selectedItem)
            {
                current.GetComponent<Image>().sprite = filledGraphic;
            }
            else
            {
                current.GetComponent<Image>().sprite = emptyGraphic;
            }
        }
    }
}
