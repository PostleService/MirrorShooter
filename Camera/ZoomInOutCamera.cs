using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInOutCamera : MonoBehaviour
{
    public float ZoomStep = 0.2f;
    public Vector2 ZoomLimits = new Vector2(8,16);
    public float CurrentZoomLimit;

    private void Awake()
    {
        CurrentZoomLimit = GetComponent<Camera>().orthographicSize;
    }

    private void OnEnable()
    {
        PlayerControls.OnZoomChange += ZoomInOut;
    }

    private void OnDisable()
    {
        PlayerControls.OnZoomChange -= ZoomInOut;
    }

    private void ZoomInOut(float aFloat)
    {
        float NewZoom = CurrentZoomLimit + ZoomStep * aFloat;

        if (NewZoom > ZoomLimits.x && NewZoom < ZoomLimits.y)
        {
            GetComponent<Camera>().orthographicSize += ZoomStep * aFloat;
            CurrentZoomLimit += ZoomStep * aFloat;
        }
        else 
        {
            if (NewZoom <= ZoomLimits.x)
            {
                GetComponent<Camera>().orthographicSize = ZoomLimits.x;
                CurrentZoomLimit = ZoomLimits.x;
            }

            else if (NewZoom >= ZoomLimits.y)
            {
                GetComponent<Camera>().orthographicSize = ZoomLimits.y;
                CurrentZoomLimit = ZoomLimits.y;
            }
        }
    }

}
