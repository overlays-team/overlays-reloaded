using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightSlider : MonoBehaviour
{
    public Text sliderText;
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        sliderText.text = slider.value.ToString("F2");
    }
}