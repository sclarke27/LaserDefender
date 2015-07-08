using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsPanel : MonoBehaviour
{

    public static OptionsPanel optionsPanel;

    private GameData gameData;

    // player pref UI Fields
    public Slider musicVolSlider;
    public Slider sfxVolSlider;
    public Toggle useAIToggle;
    public GameAnalytics gameAnalytics;
    

    void Awake()
    {
        
        if (optionsPanel == null)
        {
            DontDestroyOnLoad(gameObject);
            optionsPanel = this;
        }
        else if (this != optionsPanel)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        PopulateDefaultValues();
    }


    void Update()
    {
        
    }

    void PopulateDefaultValues()
    {
        if (gameData != null)
        {
            musicVolSlider.value = gameData.GetMusicVolume();
            sfxVolSlider.value = gameData.GetSFXVolume();
            useAIToggle.isOn = gameData.GetAIEnabled();
        }
    }

    public void SaveSelectedOptions()
    {
        gameData.SetMusicVolume(musicVolSlider.value);
        gameData.SetSFXVolume(sfxVolSlider.value);
        gameData.SetAIEnabled(useAIToggle.isOn);
        PopulateDefaultValues();
        ShowOptionsPanel(false);
    }

    public void ShowOptionsPanel(bool showPanel)
    {
        if (showPanel)
        {
            gameAnalytics.LogScreen("Options Panel - Show");
        }
        else
        {
            gameAnalytics.LogScreen("Options Panel - Hide");
        }
        
        gameObject.SetActive(showPanel);
        PopulateDefaultValues();
    }


}
