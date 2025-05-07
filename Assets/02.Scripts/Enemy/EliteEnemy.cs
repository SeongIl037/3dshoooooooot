using UnityEngine;

public class EliteEnemy : MonoBehaviour
{
    [SerializeField] private EliteSO _data;
    // 고정 값
    public int MaxHealth => _data.MaxHealth;
    public float MoveSpeed => _data.MoveSpeed;
    public float RunSpeed => _data.RunSpeed;
    public float FindDistance => _data.FindDistance;
    public float AttackDistance => _data.AttackDistance;
    public float AttackCooltime => _data.AttackCooltime;
    
    // 변동 값
    public int Health { get; private set; }
    public int Damage { get; private set; }
    
    // 셋팅
    private void OnEnable()
    {
        Health = _data.Health;
        Damage = _data.Damage;
    }
    
}
