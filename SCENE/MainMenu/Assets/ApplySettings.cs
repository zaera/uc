using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public class ApplySettings : MonoBehaviour {

    public GameSettings gameSettings;
   // public AudioSource musicSource;

    void OnEnable()
    {

        gameSettings = new GameSettings();

        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        QualitySettings.SetQualityLevel((int)gameSettings.textureQuality);

        //musicSource.volume = gameSettings.musicVolume;

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) SceneManager.LoadScene("Menu");
    }
}
