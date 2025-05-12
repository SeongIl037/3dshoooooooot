using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public float MoveSpeed;
    public int Health;
    public int MaxHealth;
    public int Damage;
    public float FindDistance;
    public float AttackDistance;
    public float AttackCooltime;
    public float DeathTime;
    
}
