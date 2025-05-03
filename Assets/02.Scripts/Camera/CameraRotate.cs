using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraRotate : MonoBehaviour
{
    public float RotationSpeed = 15f;
    public GameObject Target;
    // 카메라 각도는 0도에서부터 시작한다고 기준을 세운다.
    private float _rotationX = 0f;
    private float _rotationY = 0f;
    // 카메라 회전 스크립트
    // 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.
    // 구현 순서
    // 1. 마우스 입력을 받는다
    // 2. 마우스 입력으로부터 회전시킬 방향을 만든다.
   
    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        if (CameraChanger.instance.Type == CameraType.FpsCamera||CameraChanger.instance.Type == CameraType.TpsCamera)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Todo : 마우스 좌표계와 화면 좌표계의 차이점을 알고, 잘작동 하도록 아래 한줄의 코드를 수정한다.
            _rotationX += mouseX * RotationSpeed * Time.deltaTime;
            _rotationY += -mouseY * RotationSpeed * Time.deltaTime;
            _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

            transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0f);
            
           CameraChanger.instance.FPSPosition.eulerAngles = transform.eulerAngles;
        }
        else if (CameraChanger.instance.Type == CameraType.QuarterCamera)
        {
            transform.eulerAngles = new Vector3(45f, -90f, 0f);
        }
    }
}
