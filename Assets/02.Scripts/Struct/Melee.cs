using UnityEngine;

public struct Melee 
{
    // 맞았는지 체크하기
    public bool Hit;
    // 맞은 위치, 안맞으면 range거리체크
    public Vector3 Position;
    public  float Distance;
    public float Angle;
}
