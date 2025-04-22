using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject ExplosionEffectPrefab;
    
    // 목표 : 마우스의 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶다
    //1. 수류탄 오브젝트 만들기
    // 충돌했을 때 
    private void OnCollisionEnter(Collision other)
    {
        GameObject effect = Instantiate(ExplosionEffectPrefab);
        effect.transform.position = transform.position;
        
        gameObject.SetActive(false);
    }
}
