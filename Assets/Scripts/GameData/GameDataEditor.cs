using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.PostProcessing;

public class GameDataEditor : MonoBehaviour
{
    public static GameDataEditor Instance;
    public string dataFilePath = "/gameData.json";
    public string defaultDataPath = "Data/DefaultGameData";
    public bool loadFromDefault;
    public GameData data;

    public string dirtyWordsPath = "Data/dirtyWords";
    public DirtyWords dirtyWords;
    //private readonly string  dirtyWordsPathForInitialization = "/dirtyWordsCreation.json";
    //private DirtyWordsInitializer dirtyWordsInitializer;

    public PostProcessingProfile postProcessingProfile;

    private void Awake()
    {
        ForceSingletonPattern();

        if(string.IsNullOrEmpty(dataFilePath))
        {
            dataFilePath = "/gameData.json";
        }

        if(loadFromDefault)
        {
            LoadDefaultData();
        }
        else
        {
            if (File.Exists(getFilePath()))
            {
                LoadData();
            }
            else
            {
                LoadDefaultData();
            }
        }

        //now on working, shuya
        //InitializeDirtyWordsJson();
        LoadDirtyWords();
        //PrintDirtyWords();
    }

    private void Start()
    {
        SetInitBloomSetting();
    }

    private void Update()
    {
        if(Input.GetKey("space"))
        {
            //ScreenCapture.CaptureScreenshot("screenshot.png", 1);
            //Debug.Log("Screen captured");

            ScreenCapture.CaptureScreenshot("Assets/Resources/LevelPreviews/screenshot.png", 1);
            Debug.Log("Screen captured");
        }
    }

    void ForceSingletonPattern()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationPause(bool isPaused)
    {
        SaveData();
    }

    public string getFilePath()
    {
        return Application.persistentDataPath + dataFilePath;
    }


    public void LoadData()
    {
        string dataAsJson = File.ReadAllText(getFilePath());
        data = JsonUtility.FromJson<GameData>(dataAsJson);
    }

    public void LoadDefaultData()
    {
        TextAsset defaultGameData = Resources.Load(defaultDataPath) as TextAsset;
        data = JsonUtility.FromJson<GameData>(defaultGameData.text);
    }

    //sh
    /*
    private string GetFilePathForDirtyWordInitialization()
    {
        return Application.persistentDataPath + dirtyWordsPathForInitialization;
    }
    */

    private void LoadDirtyWords()
    {
        TextAsset dirtyWordsJson = Resources.Load(dirtyWordsPath) as TextAsset;
        dirtyWords = JsonUtility.FromJson<DirtyWords>(dirtyWordsJson.text);
    }

    private void PrintDirtyWords()
    {
        int scope = 3; //because there are to much 
        int size = dirtyWords.wordsList.Count;
        //scope = size;
        Debug.Log(size);
        for (int i = 0; i < scope; i++)
        {
            Debug.Log(dirtyWords.wordsList[i]);
        }
    }

    /*
    private void InitializeDirtyWordsJson()
    {
        string savePath = GetFilePathForDirtyWordInitialization();
        dirtyWordsInitializer = new DirtyWordsInitializer();
        //dirtyWordsInitializer.SetDirtyWords();
        dirtyWordsInitializer.SetDirtyWordsList();

        string dataString = JsonUtility.ToJson(dirtyWordsInitializer);
        File.WriteAllText(savePath, dataString);

        Debug.Log("DirtyWordsJson has been initialized : " + savePath);
    }
    */

    /*
    private void HashCodeTest()
    {
        string testString1 = "test";
        Debug.Log(testString1.GetHashCode());
        string testString2 = "test";
        Debug.Log(testString2.GetHashCode());
    }
    */


    public void SaveData()
    {
        string dataString = JsonUtility.ToJson(data);
        File.WriteAllText(getFilePath(), dataString);
    }

    public void ResetData()
    {
        LoadDefaultData();
        SaveData();
    }

    private void PrintGameData()
    {
        Debug.Log("Showing Game Data: ");
        Debug.Log("Level Count(): " + GameDataEditor.Instance.data.levels.Count);
        for (int i = 0; i < GameDataEditor.Instance.data.levels.Count; i++)
        {
            Debug.Log(GameDataEditor.Instance.data.levels[i].completed);
            Debug.Log(GameDataEditor.Instance.data.levels[i].sceneID);
        }
    }

    public void SetInitBloomSetting()
    {
        float bloomValue = data.bloomSetting;
        BloomModel.Settings bloomSettings = postProcessingProfile.bloom.settings;
        bloomSettings.bloom.intensity = bloomValue;
        postProcessingProfile.bloom.settings = bloomSettings; 
    }

    public void SetBloomSetting(float bloomValue)
    {
        data.bloomSetting = bloomValue;
        BloomModel.Settings bloomSettings = postProcessingProfile.bloom.settings;
        bloomSettings.bloom.intensity = bloomValue;
        postProcessingProfile.bloom.settings = bloomSettings;
        data.bloomSetting = bloomValue;
    }

    public float GetBloomSetting()
    {
        return postProcessingProfile.bloom.settings.bloom.intensity;
    }
}
