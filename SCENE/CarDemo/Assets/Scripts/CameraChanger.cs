using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour {

    public GameObject [] cameras;
    public int activeCamera;

    void Start()
    {
        activeCamera = 0;

        foreach(GameObject cam in cameras)
        {
            cam.SetActive(false);
        }
        cameras[activeCamera].SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            ChangeCamera();
    }

    void ChangeCamera()
    {
        cameras[activeCamera].SetActive(false);

        if (activeCamera + 1 >= cameras.Length)
            activeCamera = 0;
        else
            activeCamera++;

        cameras[activeCamera].SetActive(true);
    }

}
