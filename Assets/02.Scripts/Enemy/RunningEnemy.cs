using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class RunningEnemy : MonoBehaviour, IDamageable
{
    // 상태를 열거형으로 정의한다.
    public enum EnemyState
    {
        Idle,
        Patrol,
        Trace,
        Return,
        Attack,
        Damaged,
        Die
    }

    public HPBar _healthBar;
    // 상태를 지정한다.
    public EnemyState CurrentState = EnemyState.Idle;
    public float MoveSpeed = 5f;
    public int Health = 50;
    public int EnemyHealth = 50;
    private GameObject _player;
    private NavMeshAgent _agent;
    private CharacterController _characterController;
    // 거리 관련
    public float FindDistance = 5f;
    public float AttackDistance = 2.5f;
    
    //타이머 관련
    public float AttackCooltime = 2f;
    public float DeathTime = 2f;
    public float DamagedTime = 0.5f;
    private float _attackTimer = 0f;
        
    private void Start()
    { 
        HealthSet();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed;
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Idle:
            {
                Idle();
                break;
            }
            case EnemyState.Trace:
            {
                Trace();
                break;

            }
            case EnemyState.Attack:
            {
                Atttck();
                break;
            }
            
        }
    }

    public void TakeDamage(Damage damage)
    {
        Vector3 knockBackDirection = (transform.position - _player.transform.position).normalized;
        _characterController.Move( knockBackDirection * damage.KnockBack * Time.deltaTime);

        if (CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }
        
        Health -= damage.Value;
        Debug.Log(Health);
        _healthBar.HealthbarRefresh(Health);
        
        if (Health <= 0)
        {
            CurrentState = EnemyState.Die;
            Debug.Log($"상태전환 {CurrentState} -> Died");
            StartCoroutine(Die_Coroutine());
            return;
        }
        Debug.Log($"상태전환 {CurrentState} -> Damaged");
        
        CurrentState = EnemyState.Damaged;
        StartCoroutine(Damaged_Coroutine());

    }
    // 상태 함수들을 구현한다.
    private void Idle()
    {
        //필요 속성
        // 1. 플레이어 (위치)
        // 2. 
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Idle -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        } 
        // 행동 : 가만히 있는다
    }

    private void Trace()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("상태전환 : Trace -> Attack");
            CurrentState = EnemyState.Attack;
            return;
        }
        
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        //플레이어를 추적한다.
        // _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_player.transform.position);
    }
    
    private void Atttck()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환 : Trace -> Attack");
            CurrentState = EnemyState.Trace;
            return;
        }
        
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooltime)
        {
            Debug.Log("플레이어 공격!");
            // 공격한다.
            _attackTimer = 0;
            
            UIManager.instance.PlayerHit();
        }
    }
    private IEnumerator Damaged_Coroutine()
    {   
        _agent.isStopped = true;
        _agent.ResetPath();
        yield return new WaitForSeconds(DamagedTime);
        CurrentState = EnemyState.Trace;
    }

    
    private IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(DeathTime);
        
        Health = EnemyHealth;
        HealthSet();
        gameObject.SetActive(false);
        //죽는다.
    }
    private void HealthSet()
    {
        _healthBar.SetHealth(EnemyHealth);
        _healthBar.HealthbarRefresh(Health);
    }
}
