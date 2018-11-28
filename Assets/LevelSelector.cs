
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelector : MonoBehaviour
{


    public SceneFader fader;

    public GameObject levelPrefab;
    public GameObject content;

    private string selectedLevel;

    private Texture2D image;
    public RawImage scenePreview;
    public Button startButton;


    public void Start()
    {
        CreateTestLevelState();

        //LoadTestLevelStateFromFile();
        LoadTestLevelStateFromGameDataEditor();
    }


    /*
    public void Select(string levelName)
    {
        fader.FadeTo(levelName);
    }
    */


    /*
    public void SelectLevel(Button btn)
    {
        Debug.Log(btn.name);

        fader.FadeTo(btn.name);
    }
    */



    private void CreateTestLevelState()
    {
        Debug.Log("こんにちは、CreateTestLevelState()");

        GameDataEditor.Instance.data.levels.Add(new LevelData("level1", true));
        GameDataEditor.Instance.data.levels.Add(new LevelData("level2", true));
        GameDataEditor.Instance.data.levels.Add(new LevelData("level3", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("level4", false));

        GameDataEditor.Instance.data.levels.Add(new LevelData("level5", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("level6", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("level7", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("level8", false));

    }


    private void LoadTestLevelStateFromFile()
    {
        Debug.Log("そして、LoadFromFile()");

        //debug: loading state with original io class
        LevelSelectorIO io = new LevelSelectorIO();
        io.LoadData();
        GameDataEditor.Instance.data = io.data;
        Debug.Log(GameDataEditor.Instance.data.levels.Count);


        for (int i = 0; i < GameDataEditor.Instance.data.levels.Count; i++)
        {
            //Debug.Log(GameDataEditor.Instance.data.levels[i].sceneID + "," + GameDataEditor.Instance.data.levels[i].completed);

            GameObject level = Instantiate(levelPrefab);
            level.transform.parent = content.transform;
            level.transform.GetChild(0).GetComponent<Text>().text = GameDataEditor.Instance.data.levels[i].sceneID;
            level.GetComponent<Button>().name = GameDataEditor.Instance.data.levels[i].sceneID;
            level.GetComponent<Button>().interactable = GameDataEditor.Instance.data.levels[i].completed;
        }
    }




    private void LoadTestLevelStateFromGameDataEditor()
    {
        Debug.Log("そして、LoadFromGDE()");
        Debug.Log(GameDataEditor.Instance.data.levels.Count);

        for (int i = 0; i < GameDataEditor.Instance.data.levels.Count; i++)
        {
            //Debug.Log(GameDataEditor.Instance.data.levels[i].sceneID + "," + GameDataEditor.Instance.data.levels[i].completed);

            //int currentLevel = i + 1;
            //string sceneName = "Level" + currentLevel;
            //string buttonText = "Level " + currentLevel;
            string sceneID = GameDataEditor.Instance.data.levels[i].sceneID;

            GameObject level = Instantiate(levelPrefab);
            level.transform.parent = content.transform;
            level.transform.GetChild(0).GetComponent<Text>().text = GameDataEditor.Instance.data.levels[i].sceneID;
            level.GetComponent<Button>().name = GameDataEditor.Instance.data.levels[i].sceneID;
            level.GetComponent<Button>().interactable = GameDataEditor.Instance.data.levels[i].completed;

            level.GetComponent<Button>().onClick.AddListener(delegate { Select(sceneID); });
            //level.GetComponent<Button>().onClick.AddListener(delegate { Select(GameDataEditor.Instance.data.levels[i].sceneID); });
        }
    }


    public void ChangeScene()
    {
        Debug.Log(selectedLevel);

        if (selectedLevel != null)
        {
            Debug.Log("clicked: " + "changeScene()");
            fader.FadeTo(selectedLevel);
        }
    }


    public void Select(string levelName)
    {

        this.selectedLevel = levelName;
        Debug.Log("has set: " + selectedLevel);

        startButton.interactable = true;
        LoadPreview();
    }


   
    public void LoadPreview()
    {
     
        image = Resources.Load("LevelPreviews/" + selectedLevel) as Texture2D;

        Debug.Log(image);

        //GameObject rawImage = GameObject.Find("RawImage");
        //rawImage.GetComponent<RawImage>().texture = image;

        scenePreview.GetComponent<RawImage>().texture = image;
    }



    public void Save()
    {

        /*
        //debug
        for (int i = 0; i < GameDataEditor.Instance.data.levels.Count; i++)
        {
            Debug.Log(GameDataEditor.Instance.data.levels[i].sceneID + "," + GameDataEditor.Instance.data.levels[i].completed);
        }


        //save state with GameDataEditor
        GameDataEditor.Instance.SaveData();
        */


        //debug: saveing state
        LevelSelectorIO io = new LevelSelectorIO(GameDataEditor.Instance.data);
        io.SaveData();

    }




}