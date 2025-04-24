using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f; // 카메라와 회전속도가 똑같아야한다.
    private float _rotationX = 0f;
    
    private void Update()
    {
  
        float mouseX = Input.GetAxis("Mouse X");
        
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        
        transform.eulerAngles = new Vector3(0, _rotationX, 0);
    }
    
}
