using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeSelect : MonoBehaviour
{
    public MainMenuManager mainMenu;
    public Text selectedModeText;
    public Text selectedModeDescription;
    public ScrollSnap modeScrollSnap;

    public string[] modeTexts;
    public string[] modeDescriptions;

	// Use this for initialization
	void Start () {
        SetTexts();
    }
	
	// Update is called once per frame
	void Update () {
        SetTexts();            
	}

    public void SelectMode()
    {
        switch (modeScrollSnap.selectedItem)
        {
            case 0:
                mainMenu.ShowLevelSelect();
                break;
            case 1:
                SceneFader.Instance.FadeTo("TimeAttack");
                break;
            case 2:
                SceneFader.Instance.FadeTo("Sandbox");
                break;
            case 3:
                mainMenu.ShowTutorial();
                break;
            default:
                Debug.Log("Not a valid mode");
                break;
        }
    }

    void SetTexts()
    {
        selectedModeText.text = modeTexts[modeScrollSnap.selectedItem];
        selectedModeDescription.text = modeDescriptions[modeScrollSnap.selectedItem];
    }
}
