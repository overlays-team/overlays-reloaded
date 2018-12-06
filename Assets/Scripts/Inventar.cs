using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventar : MonoBehaviour
{

    private Camera mainCamera;

    [SerializeField]
    private List<GameObject> buttons;

    [SerializeField]
    private int wallBlocksAmount;
    private GameObject wallBlockButton;
    private GameObject wallBlockPrefab;
    
    [SerializeField]
    private int mirrorsAmount;
    private GameObject mirrorButton;
    private GameObject mirrorPrefab;

    [SerializeField]
    private int additiveBlocksAmount;
    private GameObject additiveBlockButtonPrefab;
    private GameObject additiveBlockPrefab;

    private bool aBlockIsChosen;

    public void SetABlockIsChosen(bool isChosen)
    {
        this.aBlockIsChosen = isChosen;
    }

    public void Start()
    {
        //finds the FIRST one in the scene, should be the main camera
        mainCamera = FindObjectOfType<Camera>();
        InitializeButtons();
    }

    public void Update()
    {
        //For further refactoring:
        /* foreach (GameObject button in buttons)
        {
            if (button.GetComponent<ButtonInventar>().isPressed())
            {
                
            }
        } */

        if (wallBlockButton.GetComponent<ButtonInventar>().isPressed() && aBlockIsChosen == false)
        {
            chooseWallBlock();
        }
        if (mirrorButton.GetComponent<ButtonInventar>().isPressed() && aBlockIsChosen == false)
        {
            chooseMirror();
        }
    }

    public void LayAnItemBackIntoInventar(String blockName)
    {
        switch(blockName)
        {
            case "Mirror": changeMirrorButton(); break;
            case "WallBlock": changeWallBlockButton(); break;
        }
    }

    private void changeMirrorButton()
    {
        int mirrorsAvailable = int.Parse(mirrorButton.GetComponentInChildren<Text>().text);
        mirrorsAvailable--;
        mirrorButton.GetComponentInChildren<Text>().text = "" + mirrorsAvailable;
    }

    private void changeWallBlockButton()
    {
        int wallBlocksAvailable = int.Parse(wallBlockButton.GetComponentInChildren<Text>().text);
        wallBlocksAvailable--;
        wallBlockButton.GetComponentInChildren<Text>().text = "" + wallBlocksAvailable;
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
            wallBlockPrefab = (GameObject)Resources.Load("Inventar/Blocks/BlockObject");

            wallBlockButton = GameObject.Instantiate(wallBlockButtonPrefab); //checked: it has been assigned

            //wallBlockButton.GetComponent<Button>().onClick.AddListener( delegate { chooseWallBlock(); } ); //so war es funktionierend mit dem Click

            wallBlockButton.GetComponentInChildren<Text>().text = "" + wallBlocksAmount;
            wallBlockButton.transform.SetParent(gameObject.transform); //attaching this button as a child to the inventar object

            //Doesn't work, because Blocks can be set only on GridPlanes with enabled MeshRendererd
            //createNewGridPlane(wallBlockButton.transform.position);

        }
    }

    private void InitializeMirrorButton()
    {
        if (mirrorsAmount != 0)
        {
            GameObject mirrorButtonPrefab = (GameObject)Resources.Load("Inventar/Buttons/MirrorBlockItem"); //checked: it has been loaded
            mirrorPrefab = (GameObject)Resources.Load("Inventar/Blocks/Mirror");
            //mirrors = new GameObject[mirrorsAmount];


            mirrorButton = Instantiate(mirrorButtonPrefab);
            // mirrorButton.GetComponent<Button>().onClick.AddListener(delegate { chooseMirror(); }); //so war es funktionierend mit dem Click

            mirrorButton.GetComponentInChildren<Text>().text = "" + mirrorsAmount;
            mirrorButton.transform.SetParent(gameObject.transform);
        }
    }

    public void chooseWallBlock()
    {
        Debug.Log("A wall block choosed");
        aBlockIsChosen = true;
            
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
            //GameObject currentWallBlock = wallBlocks[wallBlocksAvailable - 1];
            GameObject aNewWallBlock = Instantiate(wallBlockPrefab);
            aNewWallBlock.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        
            //Here I begin to drag an object
            GameObject.Find("PlayerController").GetComponent<PlayerController>().setSelectedBlockObject(aNewWallBlock.GetComponent<BlockObject>());

            String newAmountOfWallBlocksAvailable = "" + (wallBlocksAvailable - 1);
            wallBlockButton.GetComponentInChildren<Text>().text = newAmountOfWallBlocksAvailable;
        }

}

public void chooseMirror()
    {
        Debug.Log("A mirror block choosed");
        aBlockIsChosen = true;

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

            GameObject aNewMirror = Instantiate(mirrorPrefab);
            aNewMirror.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

            //Here I should begin to drag an object
            GameObject.Find("PlayerController").GetComponent<PlayerController>().setSelectedBlockObject(aNewMirror.GetComponent<BlockObject>());

            String newAmountOfMirrorsAvailable = "" + (mirrorsAvailable - 1);
            mirrorButton.GetComponentInChildren<Text>().text = newAmountOfMirrorsAvailable;
        }
            
    }

    public void chooseAdditiveBlock()
    {
        Debug.Log("An additive block choosed");
    }

    /* private void createNewGridPlane(Vector3 atPlace)
    {
        GameObject newGridPlane = (GameObject)Instantiate(gridPlanePrefab);
        newGridPlane.GetComponent<MeshRenderer>().enabled = false;
        newGridPlane.transform.position = atPlace;
    } */

    /* private void InitializeMirrorButton()
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
     } */


}
