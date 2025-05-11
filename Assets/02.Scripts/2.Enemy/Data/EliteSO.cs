using UnityEngine;

[CreateAssetMenu(fileName = "EliteSO", menuName = "Scriptable Objects/EliteSO")]
public class EliteSO : ScriptableObject
{
    // 기본 Info
    public float MoveSpeed;
    public float RunSpeed;
    public int Health;
    public int Damage;
    public int MaxHealth;
    // 거리 관련 Info
    public float FindDistance;
    public float AttackDistance;
    // 시간관련 Info
    public float AttackCooltime;
}
