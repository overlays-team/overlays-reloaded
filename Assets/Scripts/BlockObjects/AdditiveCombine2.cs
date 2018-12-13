using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveCombine2 : BlockObject {

    //takes 2 lasers as Input and gives one as output

    [SerializeField]
    LaserOutput laserOutput;

    //for image processing
    Texture2D inputImage1;
    Texture2D inputImage2;
    Texture2D outputImage;
    Animator animator;
    public GameObject graphic;

    protected override void Start()
    {
        base.Start();
        laserOutput.active = false;
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (lasersChanged)
        {
            if (laserInputs[0].active && laserInputs[1].active)
            {
                inputImage1 = laserInputs[0].inputLaser.image;
                inputImage2 = laserInputs[1].inputLaser.image;
                //doesn't get results
                //animator.SetBool("LaserInput", true);
                graphic.GetComponent<Animator>().SetBool("LaserInput", true);
                Debug.Log("Two laser inputs");
                StartImageProcessing();
                
            }
            else
            {
                inputImage1 = null;
                inputImage2 = null;
                //doesn't get results
                //animator.SetBool("LaserInput", false);
                graphic.GetComponent<Animator>().SetBool("LaserInput", false);
                StopImageProcessing();
            }

        }

        if (imageReady)
        {
            laserOutput.active = true;
            
            //added with 13-graphics update
            debugImage.gameObject.SetActive(true);
            debugImage.sprite = Sprite.Create(outputImage, new Rect(0, 0, outputImage.width, outputImage.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            debugImage.gameObject.SetActive(false);
            laserOutput.active = false;
        }
    }

    protected override void StartImageProcessing()
    {
        outputImage = Instantiate(inputImage1); // wir erstellen uns ein neues output Image - welches eine Kopie eines Inputs ist, wird soweiso gleih überschrieben - könnte man schlauer lösen
        if (inputImage1.width != inputImage2.width) Debug.Log("different resolutions!");

        base.StartImageProcessing();
    }

    IEnumerator ImageProcessingEnumerator()
    {
        for (int y = 0; y < outputImage.height; y++)
        {
            for (int x = 0; x < outputImage.width; x++)
            {
                outputImage.SetPixel(x, y,
                   new Color(
                       1 - (1 - inputImage1.GetPixel(x, y).r) * (1 - inputImage2.GetPixel(x, y).r) / 1,
                       1 - (1 - inputImage1.GetPixel(x, y).g) * (1 - inputImage2.GetPixel(x, y).g) / 1,
                       1 - (1 - inputImage1.GetPixel(x, y).b) * (1 - inputImage2.GetPixel(x, y).b) / 1
                   ));
            }
            if (y % 10 == 0) yield return null;
        }
        outputImage.Apply();

        imageInProcess = false;
        imageReady = true;

        laserOutput.laser.image = outputImage;
    }
}
