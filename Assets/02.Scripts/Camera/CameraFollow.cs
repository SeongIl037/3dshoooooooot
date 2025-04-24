using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{


    private void Update()
    {
        CameraTransition();
    }
    private void CameraTransition()
    {
        switch (CameraChanger.instance.Type)
        {
            case CameraType.FpsCamera:
                transform.position = CameraChanger.instance.TargetPosition.position;
                break;
            case CameraType.TpsCamera:
                transform.position = CameraChanger.instance.TargetPosition.position;
                break;
            case CameraType.QuarterCamera:
                transform.position = CameraChanger.instance.TargetPosition.position;
                break;

        }
    }
}
