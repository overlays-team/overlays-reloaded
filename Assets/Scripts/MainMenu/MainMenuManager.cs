using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    public GameObject logo;
    public GameObject mainMenu;
    public GameObject modeSelect;
    public GameObject levelSelect;
    public GameObject about;
    public GameObject options;
    public GameObject tutorial;

    public enum MainMenuState { MainMenu, ModeSelect, LevelSelect, AboutPanel, OptionsPanel, TutorialsPanel };
    public MainMenuState currentState;

    // Use this for initialization
    void Start()
    {
        currentState = MainMenuState.MainMenu;
        SceneFader.Instance.FadeToClear();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        switch (currentState)
        {
            case MainMenuState.MainMenu:
                break;
            case MainMenuState.ModeSelect:
                ShowMainMenu();
                break;
            case MainMenuState.LevelSelect:
                ShowModeSelect();
                break;
            case MainMenuState.AboutPanel:
                ShowMainMenu();
                break;
            case MainMenuState.OptionsPanel:
                ShowMainMenu();
                break;
            case MainMenuState.TutorialsPanel:
                ShowModeSelect();
                break;
        }
    }

    public void ShowModeSelect()
    {
        currentState = MainMenuState.ModeSelect;
        HideAll();
        modeSelect.SetActive(true);
    }

    public void ShowLevelSelect()
    {
        currentState = MainMenuState.LevelSelect;
        HideAll();
        levelSelect.SetActive(true);
    }

    public void ShowMainMenu()
    {
        currentState = MainMenuState.MainMenu;
        HideAll();
        mainMenu.SetActive(true);
        logo.SetActive(true);
    }

    public void ShowAbout()
    {
        currentState = MainMenuState.AboutPanel;
        HideAll();
        about.SetActive(true);
    }

    public void ShowOptions()
    {
        currentState = MainMenuState.OptionsPanel;
        options.SetActive(true);
    }

    public void ShowTutorial()
    {
        currentState = MainMenuState.TutorialsPanel;
        tutorial.SetActive(true);
    }

    public void HideAll()
    {
        logo.SetActive(false);
        mainMenu.SetActive(false);
        modeSelect.SetActive(false);
        levelSelect.SetActive(false);
        about.SetActive(false);
        options.SetActive(false);
        tutorial.SetActive(false);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
