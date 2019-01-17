using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightSlider : MonoBehaviour
{
    public Text percentageText;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        percentageText.text = (slider.value / slider.maxValue * 100).ToString("F0");
    }
}