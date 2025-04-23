using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Transform TPSTarget;
    private void Update()
    {
        transform.position = Target.position;
    }
}
