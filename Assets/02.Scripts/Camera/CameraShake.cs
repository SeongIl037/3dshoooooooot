using System;
using UnityEngine;
using MilkShake;

public class CameraShake : MonoBehaviour
{
    public Shaker MyShaker;
    public ShakePreset ShakeType;
    
    public Action Shake;

    private void Start()
    {
        ShakeCamera();
    }

    public void ShakeCamera()
    {
        MyShaker.Shake(ShakeType);
    }
}
