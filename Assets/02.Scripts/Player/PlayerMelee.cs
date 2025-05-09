using System;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public float Radius = 3; // 반지름
    public float Angle = 30; //범위
    public int MeleeDamage = 20;
    private float _attackTime = 1f;
    private bool _canAttack = false;
    private float _timer;
    private Player _player; // 플레이어 무기 종류 받아오기
    private Animator _animator;
    public ParticleSystem SlashEffect;
    private void Start()
    {
        _player = GetComponentInParent<Player>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_player.CurrentWeapon != WeaponType.Melee)
        {
            return;
        }
        
        CanAttackCheck();

        if (_canAttack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SlashEffect.Play();
                _animator.SetTrigger("Melee");
            }
        }
    }

    private void CanAttackCheck()
    {
        if (_canAttack)
        {
            return;
        }
        _timer += Time.deltaTime;
        
        if (_timer >= _attackTime)
        {
            _canAttack  = true;
            _timer = 0;
        }        
    }
    private void MeleeAttack()
    {
        Debug.Log("ggggggggggggggggggggggggg");
        // 주변 범위 체크하기
        Collider[] hits = Physics.OverlapSphere(transform.position, Radius ,~(1<<3));
        
        // 데미지 주기
        foreach (Collider hit in hits)
        {
            Vector3 target = (hit.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, target);
            
            
            if (angle <= Angle)
            {
                Damage damage = new Damage();
                damage.Value = MeleeDamage;
                damage.From = this.gameObject;
                damage.KnockBack = 20;
                
                Debug.Log($"hit with {hit.name}");
                
                IDamageable damageable = hit.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                }    
                
            }
        }
        
    }
}
