using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Out : MonoBehaviour {

    Texture2D inputImage;
    public Image debugImage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void SetInputImage(Texture2D inputImage)
    {
        this.inputImage = inputImage;
        debugImage.sprite = Sprite.Create(inputImage, new Rect(0, 0, inputImage.width, inputImage.height), new Vector2(0.5f, 0.5f));
    }
}
