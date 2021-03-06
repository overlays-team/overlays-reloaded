﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelector : MonoBehaviour
{
    public GameObject levelTogglePrefab;
    public GameObject content;
    public RawImage scenePreview;
    public Text previewLevelName;
    public Text previewLevelDescription;
    public Transform previewRating;
    public Button playButton;
    public GameObject levelSelectWarning;

    private ToggleGroup toggleGrp;
    private LevelData selectedLevel;
    private Texture2D image;

    public void Start()
    {
        toggleGrp = GetComponent<ToggleGroup>();
        LoadLevelStateFromGameDataEditor();   
    }

    //load leve state from GameDataEditor singleton and generate buttons for levels dynamically.
    private void LoadLevelStateFromGameDataEditor()
    {
        for (int i = 0; i < GameDataEditor.Instance.data.levels.Count; i++)
        {
			GameObject levelToggle = Instantiate(levelTogglePrefab, content.transform);
            levelToggle.transform.GetComponentInChildren<Text>().text = GameDataEditor.Instance.data.levels[i].sceneID;
            levelToggle.GetComponent<LevelToggle>().level = GameDataEditor.Instance.data.levels[i];

            Toggle levelToggleComponent = levelToggle.GetComponent<Toggle>();
            levelToggleComponent.transform.GetChild(0).GetComponent<Text>().text = GameDataEditor.Instance.data.levels[i].name;
            levelToggleComponent.interactable = GameDataEditor.Instance.data.levels[i].completed;
            levelToggleComponent.onValueChanged.AddListener(delegate { SelectedLevelChanged(); });
            if (GameDataEditor.Instance.data.levels[i].completed)
            {
                levelToggleComponent.group = toggleGrp;
            }else
            {
                levelToggleComponent.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }
    }


    LevelData GetSelectedLevel()
    {
        foreach (Toggle toggle in toggleGrp.ActiveToggles())
        {
            return toggle.GetComponent<LevelToggle>().level;
        }
        return null;
    }

    //for button selected (click, tap)
    public void SelectedLevelChanged()
    {
        selectedLevel = GetSelectedLevel();
        if (selectedLevel != null)
        {
            playButton.interactable = true;
            previewLevelName.text = selectedLevel.name;
            previewLevelDescription.text = "\"" + selectedLevel.description + "\"";
            SetImagePreview();
            SetPreviewRating();
            levelSelectWarning.SetActive(false);
        }
        else
        {
            playButton.interactable = false;
            previewLevelName.text = "";
            previewLevelDescription.text = "";
            scenePreview.texture = null;
            scenePreview.color = new Color(1, 1, 1, 0.05f);
            ResetPreviewRating();
            levelSelectWarning.SetActive(true);
        }
    }

    //set stars
    public void SetPreviewRating()
    {
        ResetPreviewRating();
        for (int i = 0; i < selectedLevel.score; i++)
        {
            previewRating.GetChild(i).gameObject.SetActive(true);
        }
    }

    //reset stars
    public void ResetPreviewRating()
    {
        foreach (Transform child in previewRating)
        {
            child.gameObject.SetActive(false);
        }
    }

    //change(set) preview image
    public void SetImagePreview()
    {    
        image = Resources.Load("LevelPreviews/" + selectedLevel.previewImage) as Texture2D;
        scenePreview.GetComponent<RawImage>().color = new Color(1, 1, 1, 1); 
        scenePreview.GetComponent<RawImage>().texture = image;
    }

    //go to selected level
    public void ChangeScene()
    {
        if (GetSelectedLevel() != null)
        {
            SceneFader.Instance.FadeTo(selectedLevel.sceneID);
        }
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