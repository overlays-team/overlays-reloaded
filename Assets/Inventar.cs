using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventar : MonoBehaviour {
    
    [SerializeField]
    private int wallBlocksAmount;
    private GameObject wallBlockButton;
    private GameObject[] wallBlocks;

    
    [SerializeField]
    private int mirrorsAmount;
    private GameObject mirrorButton;
    private BlockObject[] mirrors;


    /* [SerializeField]
    private GameObject mirrorBlockButtonPrefab;
    
    [SerializeField]
    private GameObject additiveBlockButtonPrefab;
    [SerializeField]
    private int additiveBlocksAmount; */


    public void Start()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        InitializeWallBlockButton();
        InitializeMirrorButton();
        //InitializeAdditiveBlockButton();

    }

    private void InitializeWallBlockButton()
    {

        if (wallBlocksAmount != 0)
        {
            GameObject wallBlockButtonPrefab = (GameObject)Resources.Load("Inventar/Buttons/WallBlockItem"); //checked: it has been loaded
            GameObject wallBlockPrefab = (GameObject)Resources.Load("Inventar/Blocks/BlockObject");
            wallBlocks = new GameObject[wallBlocksAmount];

            wallBlockButton = GameObject.Instantiate(wallBlockButtonPrefab); //checked: it has been assigned
            wallBlockButton.GetComponent<Button>().onClick.AddListener( delegate { chooseWallBlock(); } );

            wallBlockButton.GetComponentInChildren<Text>().text = "" + wallBlocksAmount;
            wallBlockButton.transform.SetParent(gameObject.transform); //attaching this button as a child to the inventar object //maybe true or false in SetParent as an argument
            //wallBlockButton.transform.parent = gameObject.transform; 
            

            for (int i = 0; i < wallBlocksAmount; i++)
            {
                GameObject aNewWallBlock = Instantiate(wallBlockPrefab);
                //FIXME: This doesn't work:
                aNewWallBlock.transform.position = wallBlockButton.transform.position;

                aNewWallBlock.SetActive(false);

                wallBlocks[i] = aNewWallBlock;
            }

        }
    }

    private void InitializeMirrorButton()
    {
        if (mirrorsAmount != 0)
        {
            GameObject mirrorButtonPrefab = (GameObject)Resources.Load("Inventar/Buttons/MirrorBlockItem"); //checked: it has been loaded
            BlockObject mirrorBlockPrefab = (BlockObject)Resources.Load("Inventar/Blocks/Mirror");
            mirrors = new BlockObject[mirrorsAmount];


            mirrorButton = Instantiate(mirrorButtonPrefab);
            mirrorButton.GetComponent<Button>().onClick.AddListener( delegate { chooseMirror(); } );
 
            mirrorButton.GetComponentInChildren<Text>().text = "" + mirrorsAmount;
            mirrorButton.transform.SetParent(gameObject.transform); 

            for (int i = 0; i < mirrorsAmount; i++)
            {
                BlockObject aNewMirror = (BlockObject)Instantiate(mirrorBlockPrefab);
                aNewMirror.transform.position = wallBlockButton.transform.position;
                aNewMirror.gameObject.SetActive(false);

                mirrors[i] = aNewMirror;
            }

        }
    }

    public void chooseWallBlock()
    {
        Debug.Log("A wall block choosed");
        //check whether it's not 0 (if it's 0 te image should be dark - "deactivated")
        //if the block is set on the play field, make text of the button show -1 from current value


        if (wallBlockButton != null)
        {
            Debug.Log("It's all fine with the wall block button");
        } else
        {
            Debug.Log("Something wrong");
        }

        int wallBlocksAvailable = int.Parse(wallBlockButton.GetComponentInChildren<Text>().text) ; //wallBlockButton.guiText ist veraltet
        if (wallBlocksAvailable >= 1)
        {
            GameObject currentWallBlock = wallBlocks[wallBlocksAvailable - 1];
            currentWallBlock.SetActive(true);

            //Here I should begin to drag an object FIXME: I should give a BlockObject as a parameter instead of a GameObject
            GameObject.Find("PlayerController").GetComponent<PlayerController>().setSelectedBlockObject(currentWallBlock);

            String newAmountOfWallBlocksAvailable = "" + (wallBlocksAvailable - 1);
            wallBlockButton.GetComponentInChildren<Text>().text = newAmountOfWallBlocksAvailable;
        }
        
    }

    public void chooseMirror()
    {
        Debug.Log("A mirror block choosed");

        if (mirrorButton != null)
        {
            Debug.Log("It's all fine with the mirror button");
        }
        else
        {
            Debug.Log("Something wrong with the mirror button");
        }

        int mirrorsAvailable = int.Parse(mirrorButton.GetComponentInChildren<Text>().text); //wallBlockButton.guiText ist veraltet
        if (mirrorsAvailable >= 1)
        {
            BlockObject currentMirror = mirrors[mirrorsAvailable - 1];
            mirrors[mirrorsAvailable - 1].gameObject.SetActive(true);

            //Here I should begin to drag an object
            GameObject.Find("PlayerController").GetComponent<PlayerController>().setSelectedBlockObject(currentMirror);

            String newAmountOfMirrorsAvailable = "" + (mirrorsAvailable - 1);
            mirrorButton.GetComponentInChildren<Text>().text = newAmountOfMirrorsAvailable;
        }
            
    }

    public void chooseAdditiveBlock()
    {
        Debug.Log("An additive block choosed");
    }

}
