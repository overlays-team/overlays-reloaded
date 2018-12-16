
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;
    public GameObject levelPrefab;
    public GameObject content;
    public RawImage scenePreview;
    public Button startButton;

    private ToggleGroup toggleGrp;
    private string selectedLevel;
    private Texture2D image;

    public void Start()
    {
        toggleGrp = GetComponent<ToggleGroup>();
        CreateTestLevelState();
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
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL1", true));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL2", true));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL3", true));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL4", true));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL5", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL6", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL7", false));
        GameDataEditor.Instance.data.levels.Add(new LevelData("LEVEL8", false));
    }

   /* private void LoadTestLevelStateFromFile()
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
*/

    private void LoadTestLevelStateFromGameDataEditor()
    {
        Debug.Log("そして、LoadFromGDE()");
        Debug.Log(GameDataEditor.Instance.data.levels.Count);

        for (int i = 0; i < GameDataEditor.Instance.data.levels.Count; i++)
        {
            string sceneID = GameDataEditor.Instance.data.levels[i].sceneID;

			GameObject level = Instantiate(levelPrefab, content.transform);
			RectTransform recttransform = level.GetComponent<RectTransform>();
            Toggle levelToggle = level.GetComponent<Toggle>();

            level.transform.GetComponentInChildren<Text>().text = GameDataEditor.Instance.data.levels[i].sceneID;
            levelToggle.name = GameDataEditor.Instance.data.levels[i].sceneID;
            levelToggle.interactable = GameDataEditor.Instance.data.levels[i].completed;
            levelToggle.onValueChanged.AddListener(delegate { SelectLevel(); });
            if (GameDataEditor.Instance.data.levels[i].completed)
            {
                levelToggle.group = toggleGrp;
            }
            level.GetComponent<LevelToggle>().levelName = sceneID;
        }
    }

    string GetSelectedLevel()
    {
        foreach (Toggle toggle in toggleGrp.ActiveToggles())
        {
            return toggle.GetComponent<LevelToggle>().levelName;
        }
        return null;
    }

    public void ChangeScene()
    {
        if (GetSelectedLevel() != null)
        {
            fader.FadeTo(selectedLevel);
        }
    }

    public void SelectLevel()
    {
        selectedLevel = GetSelectedLevel();
        if (selectedLevel != null)
        {
            startButton.interactable = true;
            LoadPreview();
        }
        else
        {
            scenePreview.texture = null;
            scenePreview.color = new Color(1, 1, 1, 0.05f);
        }
    }

    public void LoadPreview()
    {
     
        image = Resources.Load("LevelPreviews/" + selectedLevel) as Texture2D;

        Debug.Log(image);

        //GameObject rawImage = GameObject.Find("RawImage");
        //rawImage.GetComponent<RawImage>().texture = image;

        scenePreview.GetComponent<RawImage>().color = new Color(1, 1, 1, 1); 
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

    private void OnDisable()
    {
        foreach (Toggle toggle in toggleGrp.ActiveToggles())
        {
            toggle.isOn = false;
            toggle.GetComponent<LevelToggle>().SetAnimationState();
        }
    }
}