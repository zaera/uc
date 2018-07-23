using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCamera : MonoBehaviour {

    public GameObject[] cameras;
    public int activeCamera;

	
	void Start () {
        activeCamera = 0;
        foreach (GameObject cam in cameras)
        {
            cam.SetActive(false); //Отключение всех камер
        }
        cameras[activeCamera].SetActive(true);
	}
	

	void Update () {

        if (Input.GetKeyDown(KeyCode.C))
        {
            Swap_Cam();
        }
	}

    void Swap_Cam()
    {
        cameras[activeCamera].SetActive(false); //Выключение активной камеры

        if (activeCamera + 1 >= cameras.Length)
        {
            activeCamera = 0;
        }

        else {
            activeCamera++;
        }

        cameras[activeCamera].SetActive(true);
    }
}
