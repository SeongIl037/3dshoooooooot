using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform FPSTarget;
    public Transform TPSTarget;
    public Transform QuaterTarget;

    private void LateUpdate()
    {
        CameraTransition();
    }
    private void CameraTransition()
    {
        switch (CameraChanger.instance.Type)
        {
            case CameraType.FpsCamera:
                transform.position = FPSTarget.position;
                break;
            case CameraType.TpsCamera:
                transform.position = TPSTarget.position;
                break;
            case CameraType.QuarterCamera:
                transform.position = QuaterTarget.position;
                break;

        }
    }
}
