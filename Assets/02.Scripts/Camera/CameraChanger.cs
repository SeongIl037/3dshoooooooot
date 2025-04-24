using System;
using UnityEngine;

public enum CameraType
{
    FpsCamera,
    TpsCamera,
    QuarterCamera
}
public class CameraChanger : Singletone<CameraChanger>
{
    public CameraType Type;

    private void Start()
    {
        Type = CameraType.FpsCamera;
    }

    private void Update()
    {
        ChangeCamera();
    }
    private void ChangeCamera()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Type = CameraType.FpsCamera;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Type = CameraType.TpsCamera;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Type = CameraType.QuarterCamera;
        }
    }
}
