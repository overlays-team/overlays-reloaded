using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class BloomSlider : MonoBehaviour
{
    public PostProcessingProfile postProcessingProfile;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = postProcessingProfile.bloom.settings.bloom.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBloom(float value)
    {
        BloomModel.Settings bloomSettings = postProcessingProfile.bloom.settings;
        bloomSettings.bloom.intensity = value;
        postProcessingProfile.bloom.settings = bloomSettings;
    }
}
