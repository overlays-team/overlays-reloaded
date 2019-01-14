using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class BloomSlider : MonoBehaviour
{
    public PostProcessingProfile postProcessingProfile;
    public Text percentageText;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = GameDataEditor.Instance.GetBloomSetting();
    }

    // Update is called once per frame
    void Update()
    {
        percentageText.text = (slider.value / slider.maxValue * 100).ToString("F0");
    }

    public void SetBloom(float bloomValue)
    {
        GameDataEditor.Instance.SetBloomSetting(bloomValue);
    }
}