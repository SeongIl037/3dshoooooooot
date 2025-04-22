using System;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    
    // 3. 발사 위치에 수류탄 생성하기
    // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기
    
    // 필요 속성
    // - 발사 위치
    public GameObject FirePosition;
    public GameObject BombPrefab;
    public float ThrowPower = 15;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    private void Update()
    {
        // 2. 오른쪽 버튼 입력 받기
        if (Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(BombPrefab);
            bomb.transform.position = FirePosition.transform.position;
            
            Rigidbody bombRigidBody = bomb.GetComponent<Rigidbody>();
            bombRigidBody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
            bombRigidBody.AddTorque(Vector3.one);
        }
    }
    
}
