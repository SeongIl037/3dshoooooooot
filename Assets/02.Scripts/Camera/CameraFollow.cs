using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = CameraChanger.instance.TargetPosition.position;
    }
}
