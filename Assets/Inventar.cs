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
    private GameObject[] mirrors;


    /* [SerializeField]
    private GameObject mirrorBlockButtonPrefab;
    
    [SerializeField]
    private GameObject additiveBlockButtonPrefab;
    [SerializeField]
    private int additiveBlocksAmount; */


    public void Start()
    {
        GameObject wallBlockButtonPrefab = (GameObject)Resources.Load("Inventar/Buttons/WallBlockItem");
        this.wallBlockButton = Instantiate(wallBlockButtonPrefab);
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        InitializeWallBlockButton();
        InitializeMirrorButton();
        //InitializeAdditiveBlockButton();
        if (wallBlockButton != null)
        {
            Debug.Log("wallBlock Button is fine " + wallBlockButton);
        }
        else
        {
            Debug.Log("Something wrong wallBlock Button");
        }

    }

    private void InitializeWallBlockButton()
    {
        if (wallBlocksAmount != 0)
        {
            GameObject wallBlockButtonPrefab = (GameObject)Resources.Load("Inventar/Buttons/WallBlockItem"); //checked: it has been loaded
            GameObject wallBlockPrefab = (GameObject)Resources.Load("Inventar/Blocks/BlockObject");
            wallBlocks = new GameObject[wallBlocksAmount];


            wallBlockButton = Instantiate(wallBlockButtonPrefab) as GameObject; //checked: it has been assigned
 
            wallBlockButton.GetComponentInChildren<Text>().text = "" + wallBlocksAmount;
            wallBlockButton.transform.SetParent(gameObject.transform); //attaching this button as a child to the inventar object //maybe true or false in SetParent as an argument
            //wallBlockButton.transform.parent = gameObject.transform; 


            for (int i = 0; i < wallBlocksAmount; i++)
            {
                GameObject aNewWallBlock = GameObject.Instantiate((GameObject)Resources.Load("Inventar/Blocks/BlockObject"));
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
            //GameObject mirrorBlockPrefab = (GameObject)Resources.Load("Inventar/Blocks/Mirror");
            mirrors = new GameObject[mirrorsAmount];


            mirrorButton = Instantiate(mirrorButtonPrefab); 
 
            mirrorButton.GetComponentInChildren<Text>().text = "" + mirrorsAmount;
            mirrorButton.transform.SetParent(gameObject.transform); 

            for (int i = 0; i < mirrorsAmount; i++)
            {
                GameObject aNewMirror = GameObject.Instantiate((GameObject)Resources.Load("Inventar/Blocks/Mirror"));
                aNewMirror.transform.position = wallBlockButton.transform.position;
                aNewMirror.SetActive(false);

                mirrors[i] = aNewMirror;
            }

        }
    }

    public void chooseWallBlock()
    {
        Debug.Log("A wall block choosed");
        //check whether it's not 0 (if it's 0 te image should be dark - "deactivated")
        //if the block is set on the play field, make text of the button show -1 from current value

        int wallBlocksAvailable = int.Parse(wallBlockButton.GetComponent<GUIText>().text) ; //wallBlockButton.guiText ist veraltet
        wallBlocks[wallBlocksAvailable - 1].SetActive(true);

    }

    public void chooseMirrorBlock()
    {
        Debug.Log("A mirror block choosed");
    }

    public void chooseAdditiveBlock()
    {
        Debug.Log("An additive block choosed");
    }

}
