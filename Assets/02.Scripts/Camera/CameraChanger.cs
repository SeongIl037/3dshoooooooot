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
    // 총, 근접, 폭탄 위치 변경
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
            SetCamera(Type);
        }
        // tps
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Type = CameraType.TpsCamera;
            SetCamera(Type);
        }
        // quarter
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Type = CameraType.QuarterCamera;
            SetCamera(Type);
        }
    }
    // 카메라 위치 변경 및  크로스헤어 온오프 하기
    private void SetCamera(CameraType cameraType)
    {
        switch (Type)
        {
            case CameraType.FpsCamera:
            {
                TargetPosition = Position;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            }
            case CameraType.TpsCamera:
            {
                TargetPosition = TPSPosition;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            }
            case CameraType.QuarterCamera:
            {
                TargetPosition = QuarterPosition;
                Cursor.lockState = CursorLockMode.None;
                break;
            }
                
        }
        
        UIManager.instance.OnOffCrosshair();
    }
}
