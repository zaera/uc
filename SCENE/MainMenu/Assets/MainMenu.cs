using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void SetQuality(float qualityIndex)
    {
        switch((int)qualityIndex)
        {
            case 0:
                QualitySettings.SetQualityLevel(0);
                break;

            case 1:
                QualitySettings.SetQualityLevel(1);
                break;

            case 2:
                QualitySettings.SetQualityLevel(2);
                break;
        }
    }

}
