using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletPointManager : MonoBehaviour
{
    public GameObject tutorialModeScrollSnap;
    public Sprite empty;
    public Sprite filled;

    private ScrollSnap scrollSnap;
    private int selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        scrollSnap = tutorialModeScrollSnap.GetComponent<ScrollSnap>();
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
                current.GetComponent<Image>().sprite = filled;
            }
            else
            {
                current.GetComponent<Image>().sprite = empty;
            }
        }
    }
}
