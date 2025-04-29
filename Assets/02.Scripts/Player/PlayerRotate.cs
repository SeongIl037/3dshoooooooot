using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f; // 카메라와 회전속도가 똑같아야한다.
    private float _rotationX = 0f;
    private float _rotationY = 0f;
    public GameObject Player;
    private void Update()
    {
  
        float mouseX = Input.GetAxis("Mouse X");
        
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        
        transform.eulerAngles = new Vector3(0, _rotationX, 0);
        
        float mouseY = Input.GetAxis("Mouse Y");
        
        _rotationY += mouseY * RotationSpeed * Time.deltaTime;
        
        Player.transform.localEulerAngles = new Vector3(-_rotationY,0, 0); 
    }
    
}
