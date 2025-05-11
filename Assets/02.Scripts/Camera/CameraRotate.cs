using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraRotate : MonoBehaviour
{
    // 카메라 각도는 0도에서부터 시작한다고 기준을 세운다.
    private float _rotationX = 0f;
    private float _rotationY = 0f;
    public PlayerRotate PlayerRotation;
    public Vector3 QuarterViewOffset =  new Vector3(45f, -90f, 0f);
    // 카메라 회전 스크립트

    private void Update()
    {
        RotateCamera();
    }
    // 카메라 회전주기
    private void RotateCamera()
    {
        if (CameraChanger.instance.Type == CameraType.FpsCamera||CameraChanger.instance.Type == CameraType.TpsCamera)
        {
            // Todo : 마우스 좌표계와 화면 좌표계의 차이점을 알고, 잘작동 하도록 아래 한줄의 코드를 수정한다.
            _rotationX = PlayerRotation.RotationX;
            _rotationY = PlayerRotation.RotationY;
            _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

            transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0f);
            
           CameraChanger.instance.FPSPosition.eulerAngles = transform.eulerAngles;
        }

        else if (CameraChanger.instance.Type == CameraType.QuarterCamera)
        {
            transform.eulerAngles = QuarterViewOffset;
        }
    }
}
