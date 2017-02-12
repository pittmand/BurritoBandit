using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    /// MENU ELEMENTS ///
    // File
    public Button button_Apply;
    public Button button_Cancel;

    // Video
    public Toggle toggle_Fullscreen;
    public Dropdown dropdown_Resolution;
    public Dropdown dropdown_TextureQuality;
    public Dropdown dropdown_ShadowResolution;
    public Dropdown dropdown_Antialiasing;
    public Dropdown dropdown_vSync;

    // Audio
    public Slider slider_Volume_Master;
    public Slider slider_Volume_Music;
    public Slider slider_Volume_FX;

    /// SYSTEM INFO ///
    // Video
    public Resolution[] resolutions;

    // Audio
    public AudioSource audioSource;

    /// APP INFO ///
    // Settings
    public GameSettings gameSettings;



    /// UNITY EVENTS ///
    
    void Awake()
    {
        // Init settings
        gameSettings = new GameSettings();

        // Make Persistant
        Object.DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        findResolutions();

        // Load settings file
        LoadSettings();

        // Bind delegates
        button_Apply.onClick.AddListener(delegate { OnClick_Apply(); });
        button_Cancel.onClick.AddListener(delegate { OnClick_Cancel(); });
        toggle_Fullscreen.onValueChanged.AddListener(delegate { OnChange_Fullscreen(); });
        dropdown_Resolution.onValueChanged.AddListener(delegate { OnChange_Resolution(); });
        dropdown_TextureQuality.onValueChanged.AddListener(delegate { OnChange_TextureQuality(); });
        dropdown_ShadowResolution.onValueChanged.AddListener(delegate { OnChange_ShadowResolution(); });
        dropdown_Antialiasing.onValueChanged.AddListener(delegate { OnChange_Antialiasing(); });
        dropdown_vSync.onValueChanged.AddListener(delegate { OnChange_vSync(); });
        slider_Volume_Master.onValueChanged.AddListener(delegate { OnChange_Volume(); });
        slider_Volume_Music.onValueChanged.AddListener(delegate { OnChange_Volume(); });
        slider_Volume_FX.onValueChanged.AddListener(delegate { OnChange_Volume(); });
    }



    /// RESOLUTION ///
    
    internal void findResolutions()
    {
        // Find resolutions
        resolutions = Screen.resolutions;
        dropdown_Resolution.options.Clear();
        foreach (Resolution resolution in resolutions)
        {
            dropdown_Resolution.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
        dropdown_Resolution.RefreshShownValue();
    }

    internal int findCurrentResolutionIndex()
    {
        for (int i=0; i<resolutions.Length; ++i)
        {
            Resolution resolution = resolutions[i];
            if (resolution.width == gameSettings.resolution_Width && resolution.height == gameSettings.resolution_Height && resolution.refreshRate == gameSettings.resolution_refreshRate)
                return i;
        }

        // Invalid settings: load current
        setResolutionSettings(Screen.currentResolution);

        for (int i = 0; i < resolutions.Length; ++i)
        {
            Resolution resolution = resolutions[i];
            if (resolution.width == gameSettings.resolution_Width && resolution.height == gameSettings.resolution_Height && resolution.refreshRate == gameSettings.resolution_refreshRate)
                return i;
        }

        // Should never get called
        return 0;
    }

    internal void setResolutionSettings(Resolution res)
    {
        gameSettings.resolution_Width = res.width;
        gameSettings.resolution_Height = res.height;
        gameSettings.resolution_refreshRate = res.refreshRate;
    }




    /// FILE ACCESS ///
    
    internal string settings_FilePath()
    {
        return Application.persistentDataPath + "/settings.json";
    }

    internal void LoadSettings()
    {
        // Test if file has been created
        if (File.Exists(settings_FilePath()))
        {
            // Read file
            string data = File.ReadAllText(settings_FilePath());
            gameSettings = JsonUtility.FromJson<GameSettings>(data);

            // Update UI
            toggle_Fullscreen.isOn = gameSettings.fullscreen;
            dropdown_Resolution.value = findCurrentResolutionIndex();
            dropdown_Resolution.RefreshShownValue();
            dropdown_TextureQuality.value = gameSettings.textureQuality;
            dropdown_TextureQuality.RefreshShownValue();
            dropdown_ShadowResolution.value = gameSettings.shadowResolution;
            dropdown_ShadowResolution.RefreshShownValue();
            dropdown_Antialiasing.value = (int)Mathf.Log(gameSettings.antialiasing, 2);
            dropdown_Antialiasing.RefreshShownValue();
            dropdown_vSync.value = gameSettings.vSync;
            dropdown_vSync.RefreshShownValue();
            slider_Volume_Master.value = gameSettings.volumeMaster;
            slider_Volume_Music.value = gameSettings.volumeMusic;
            slider_Volume_FX.value = gameSettings.volumeFX;

            // Update Game
            Resolution res = resolutions[dropdown_Resolution.value];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
            QualitySettings.shadowResolution = (ShadowResolution)gameSettings.shadowResolution;
            QualitySettings.masterTextureLimit = gameSettings.textureQuality;
            QualitySettings.antiAliasing = gameSettings.antialiasing;
            QualitySettings.vSyncCount = gameSettings.vSync;
            //audioSource.volume = gameSettings.volumeMaster;
            // = gameSettings.volumeMusic;
            // = gameSettings.volumeFX;
        }
        else
        {
            // Set loaded resolution and fullscreen state
            toggle_Fullscreen.isOn = gameSettings.fullscreen = Screen.fullScreen;
            setResolutionSettings(Screen.currentResolution);
            dropdown_Resolution.value = findCurrentResolutionIndex();
            dropdown_Resolution.RefreshShownValue();
            
            // Create default settings file
            SaveSettings();
        }
    }

    internal void SaveSettings()
    {
        string data = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(settings_FilePath(), data);
    }




    /// SETTINGS EVENTS ///

    public void OnClick_Apply()
    {
        SaveSettings();
    }

    public void OnClick_Cancel()
    {
        LoadSettings();
    }

    public void OnChange_Fullscreen()
    {
        bool b = toggle_Fullscreen.isOn;
        gameSettings.fullscreen = b;
        Screen.fullScreen = b;
    }

    public void OnChange_Resolution()
    {
        Resolution res = resolutions[dropdown_Resolution.value];
        setResolutionSettings(res);
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void OnChange_TextureQuality()
    {
        int i = dropdown_TextureQuality.value;
        gameSettings.textureQuality = i;
        QualitySettings.masterTextureLimit = i; 
    }

    public void OnChange_ShadowResolution()
    {
        int i = dropdown_ShadowResolution.value;
        gameSettings.shadowResolution = i;
        QualitySettings.shadowResolution = (ShadowResolution)i;
    }

    public void OnChange_Antialiasing()
    {
        int i = (int)Mathf.Pow(2, dropdown_Antialiasing.value);
        gameSettings.antialiasing = i;
        QualitySettings.antiAliasing = i;
    }

    public void OnChange_vSync()
    {
        int i = dropdown_vSync.value;
        gameSettings.vSync = i;
        QualitySettings.vSyncCount = i;
    }

    public void OnChange_Volume()
    {
        gameSettings.volumeMaster = slider_Volume_Master.value;
        gameSettings.volumeMusic = slider_Volume_Music.value;
        gameSettings.volumeFX = slider_Volume_FX.value;

        //audioSource.volume = gameSettings.volumeMaster;
    }
}
