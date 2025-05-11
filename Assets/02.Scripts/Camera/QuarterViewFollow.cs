using System;
using UnityEngine;

public class QuarterViewFollow : MonoBehaviour
{
    public Vector3 Offset;
    public GameObject Target;
    
    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        FollowPlayer();
    }
    // 쿼터뷰 위치 플레이어 따라오기
    private void FollowPlayer()
    {
        transform.position = Target.transform.position + Offset;
    }
}
