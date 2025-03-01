using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerScript.OnPlayerTP += ChangeMainCamera;
    }
    private void OnDisable()
    {
        PlayerScript.OnPlayerTP -= ChangeMainCamera;
    }

    public void ChangeMainCamera()
    {
        Camera[] Cameras = GameObject.FindObjectsOfType<Camera>();
        foreach (Camera cam in Cameras)
        {
            if (cam.tag == "SecondaryCamera") { cam.tag = "MainCamera"; }
            else { cam.tag = "SecondaryCamera"; }
        }
    }


}
