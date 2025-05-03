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
    public Transform FPSPosition;
    public Transform TPSPosition;
    public Transform QuarterPosition;
    public Transform TargetPosition;
    public Transform Position;

    private void Start()
    {
        Type = CameraType.FpsCamera;
        TargetPosition = Position;
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
            TargetPosition = Position;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Type = CameraType.TpsCamera;
            TargetPosition = TPSPosition;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Type = CameraType.QuarterCamera;
            TargetPosition = QuarterPosition;
        }
    }
}
