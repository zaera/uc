using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingsManager : MonoBehaviour {

    public Slider textureQualitySlider;
    public Slider musicVolumeSlider;
    public Button applyButton;

    public AudioSource musicSource;
    public GameSettings gameSettings;

    void OnEnable()
    {
        gameSettings = new GameSettings();

        textureQualitySlider.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });

        LoadSettings();
    }

    public void OnTextureQualityChange()
    {
        gameSettings.textureQuality = textureQualitySlider.value;
    }

    public void OnMusicVolumeChange()
    {
        musicSource.volume = gameSettings.musicVolume = musicVolumeSlider.value;
    }

    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));

        musicVolumeSlider.value = gameSettings.musicVolume;
        textureQualitySlider.value = gameSettings.textureQuality;
    }

}
