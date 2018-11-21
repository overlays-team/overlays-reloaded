
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelector : MonoBehaviour
{


    public SceneFader fader;

    public GameObject levelPrefab;
    public GameObject content;


    public void Start()
    {
        CreateTestLevelState();

        //LoadTestLevelStateFromFile();
        LoadTestLevelStateFromGameDataEditor();
    }


    public void Select(string levelName)
    {
        fader.FadeTo(levelName);
    }


    public void SelectLevel(Button btn)
    {
        Debug.Log(btn.name);

        fader.FadeTo(btn.name);
    }



    private void CreateTestLevelState()
    {
        Debug.Log("こんにちは、CreateTestLevelState()");

        GameDataEditor.Instance.data.levels.Add(new LevelData("あ", true));
        GameDataEditor.Instance.data.levels.Add(new LevelData("い", true));
        GameDataEditor.Instance.data.levels.Add(new LevelData("う", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("け", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("Level1", true));
        GameDataEditor.Instance.data.levels.Add(new LevelData("Level2", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("Level3", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("Level4", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("TestLevel01", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("TestLevel02", false));

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

            GameObject level = Instantiate(levelPrefab);
            level.transform.parent = content.transform;
            level.transform.GetChild(0).GetComponent<Text>().text = GameDataEditor.Instance.data.levels[i].sceneID;
            level.GetComponent<Button>().name = GameDataEditor.Instance.data.levels[i].sceneID;
            level.GetComponent<Button>().interactable = GameDataEditor.Instance.data.levels[i].completed;
        }
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