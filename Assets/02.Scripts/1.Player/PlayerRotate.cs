using Unity.VisualScripting;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f; // 카메라와 회전속도가 똑같아야한다.
    public float RotationX {get; private set;} = 0;
    public float RotationY { get; private set;}= 0;
    public GameObject Player;
    private void Update()
    {
        if (CameraChanger.instance.Type == CameraType.QuarterCamera)
        {
            RotateTowardMouse();
        }
        else
        {
         
            float mouseX = Input.GetAxis("Mouse X");
        
            RotationX += mouseX * RotationSpeed * Time.deltaTime;
        
            transform.eulerAngles = new Vector3(0, RotationX, 0);
        
            float mouseY = Input.GetAxis("Mouse Y");
        
            RotationY += mouseY * RotationSpeed * Time.deltaTime;
        
            Player.transform.eulerAngles = new Vector3(-RotationY,0, 0); 

        } 
    }
    
    private void RotateTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // 바닥은 y = 0 기준
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 hitPoint = ray.GetPoint(rayDistance);
            Vector3 direction = (hitPoint - transform.position).normalized;
            direction.y = 0f; // 수평 방향만 고려

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            }
        }
    }

}
