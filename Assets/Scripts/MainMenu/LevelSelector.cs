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
    public Button playButton;

    private ToggleGroup toggleGrp;
    private string selectedLevel;
    private Texture2D image;

    public void Start()
    {
        toggleGrp = GetComponent<ToggleGroup>();
        LoadLevelStateFromGameDataEditor();   
    }


    private void LoadLevelStateFromGameDataEditor()
    {
        Debug.Log(GameDataEditor.Instance.data.levels.Count);

        for (int i = 0; i < GameDataEditor.Instance.data.levels.Count; i++)
        {
            string sceneID = GameDataEditor.Instance.data.levels[i].sceneID;

			GameObject level = Instantiate(levelPrefab, content.transform);
			RectTransform recttransform = level.GetComponent<RectTransform>();
            Toggle levelToggle = level.GetComponent<Toggle>();

            //load data
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
            playButton.interactable = true;
            LoadPreview();
        }
        else
        {
            scenePreview.texture = null;
            playButton.interactable = false;
            scenePreview.color = new Color(1, 1, 1, 0.05f);
        }
    }

    public void LoadPreview()
    {    
        image = Resources.Load("LevelPreviews/" + selectedLevel) as Texture2D;
        scenePreview.GetComponent<RawImage>().color = new Color(1, 1, 1, 1); 
        scenePreview.GetComponent<RawImage>().texture = image;
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