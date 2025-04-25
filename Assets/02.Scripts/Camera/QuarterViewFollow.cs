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

    private void FollowPlayer()
    {
        transform.position = Target.transform.position + Offset;
    }
}
