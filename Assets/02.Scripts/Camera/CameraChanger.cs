using System;
using System.Diagnostics;
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
    private Player _player;
    
    private void Start()
    {
        Type = CameraType.FpsCamera;
        TargetPosition = Position;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        CameraPosition();
        ChangeCamera();
    }

    private void CameraPosition()
    {
        if (Type != CameraType.FpsCamera)
        {
            return;
        }
        if (_player.CurrentWeapon == WeaponType.Grandae || _player.CurrentWeapon == WeaponType.Melee)
        {
            TargetPosition = FPSPosition;
        }
        else
        {
            TargetPosition = Position;
        }
    }
    private void ChangeCamera()
    {
        // fps
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Type = CameraType.FpsCamera;
            TargetPosition = Position;
        }

        // tps
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Type = CameraType.TpsCamera;
            TargetPosition = TPSPosition;
        }

        // 
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Type = CameraType.QuarterCamera;
            TargetPosition = QuarterPosition;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
