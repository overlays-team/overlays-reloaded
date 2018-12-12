using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : BlockObject
{
    [SerializeField]
    LaserOutput laserOutput;

    //for image processing
    Texture2D inputImage;
    Texture2D outputImage;
    //testing
    public enum FilterColor {RED, GREEN, BLUE, NONE};
    public FilterColor filterMode = FilterColor.NONE;
    public GameObject laser;
    Animator animator;

    protected override void Start()
    {
        base.Start();
        laserOutput.active = false;
        animator = GetComponent<Animator>();
        switch (filterMode)
        {
            case FilterColor.RED:
                laser.GetComponent<LineRenderer>().SetColors(Color.red, Color.red);
                break;
            case FilterColor.GREEN:
                laser.GetComponent<LineRenderer>().SetColors(Color.green, Color.green);
                break;
            case FilterColor.BLUE:
                laser.GetComponent<LineRenderer>().SetColors(Color.blue, Color.blue);
                break;
            case FilterColor.NONE:
                laser.GetComponent<LineRenderer>().SetColors(Color.white, Color.white);
                break;
        }
    }

    protected override void Update()
    {
        //right now here, because doesn't see game object at the beginning
        
        base.Update();

        if (lasersChanged)
        {
            if (laserInputs[0].active)
            {
                inputImage = laserInputs[0].inputLaser.image;
                StartImageProcessing();
                //doesn't get immediate results
                laser.GetComponentInChildren<Animator>().SetBool("LaserInput", true);
                //laser.transform.GetComponentInChildren<Animator>().SetBool("LaserInput", true);
            }
            else
            {
                inputImage = null;
                StopImageProcessing();
                //doesn't get immediate results
                //laser.transform.GetComponentInChildren<Animator>().SetBool("LaserInput", false);
                laser.GetComponentInChildren<Animator>().SetBool("LaserInput", false);
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
        outputImage = Instantiate(inputImage);
        base.StartImageProcessing(); 
    }


    //könnte auch in die Vaterklasse verlagert werden, nur weiß ich nicht wie das schlau mit enumeratoren geht
    IEnumerator ImageProcessingEnumerator()
    {
        for (int y = 0; y < outputImage.height; y++)
        {
            for (int x = 0; x < outputImage.width; x++)
            {   
                //testing
                Color tempC = new Color(0f, 0f, 0f);
                Color cache = outputImage.GetPixel(x, y);

                switch (filterMode)
                {
                    case FilterColor.RED:
                        tempC.r = cache.r;
                        break;
                    case FilterColor.GREEN:
                        tempC.g = cache.g;
                        break;
                    case FilterColor.BLUE:
                        tempC.b = cache.b;
                        break;
                    case FilterColor.NONE:
                        tempC = cache;
                        break;
                }
                outputImage.SetPixel(x, y, tempC);
            }
            if (y % 10 == 0) yield return null;
        }
        outputImage.Apply();
        
        imageInProcess = false;
        imageReady = true;

        laserOutput.laser.image = outputImage;
    }


}
