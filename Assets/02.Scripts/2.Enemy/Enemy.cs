using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
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
    public HPBar HealthBar;
    public Location LocationData;
    public List<Vector3> PatrolPoint = new List<Vector3>();
    private EnemyHit _hit;
    
    // 상태를 지정한다.
    public EnemyState CurrentState = EnemyState.Idle;
    public float MoveSpeed = 3.3f;
    public int Health = 100;
    public int MaxHealth = 100;
    private GameObject _player;
    private CharacterController _characterController;
    private NavMeshAgent _agent;
    private Vector3 _startPosition;
    private Vector3 goalDir;
    private Animator _animator;
    // 거리 관련
    public float FindDistance = 5f;
    public float AttackDistance = 2.5f;
    public float ReturnDistance = 2.5f;
    public float IdleDistance = 0.2f;
    public float PatrolDistance = 0.2f;
    
    //타이머 관련
    public float AttackCooltime = 2f;
    public float DeathTime = 2f;
    public float DamagedTime = 0.5f;
    public float PatrolTime = 2f;
    private float _attackTimer = 0f;
    private float _patrolTimer = 0f;
    private ObjectPool _objectPool;
    private void Start()
    {
        _hit = GetComponent<EnemyHit>();
        _objectPool = GameObject.FindGameObjectWithTag("CoinPool").GetComponent<ObjectPool>();
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed;
        _animator = GetComponentInChildren<Animator>();
        HealthSet();
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();

        SetPatrol();
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
            case EnemyState.Return:
            {
                Return();
                break;
            }
            case EnemyState.Attack:
            {
                Attack();
                break;
            }
            case EnemyState.Die:
            {
                Die();
                break;
            }
            case EnemyState.Patrol:
            {
                Patrol();
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
        _hit.HitFlash();
        _animator.SetTrigger("Hit");
        HealthBar.HealthbarRefresh(Health);
        
        if (Health <= 0)
        {
            _animator.SetTrigger("Die");
            CurrentState = EnemyState.Die;
            Debug.Log($"상태전환 {CurrentState} -> Died");
            StartCoroutine(Die_Coroutine());
            return;
        }
        Debug.Log($"상태전환 {CurrentState} -> Damaged");
        
        CurrentState = EnemyState.Damaged;

        StartCoroutine(Damaged_Coroutine());
        
    }
    // 0 : 대기
    // 1 : 추적
    // 2 : 복귀
    // 3 : 공격
    // 4 : 피격
    // 5 : 사망
    // 상태 함수들을 구현한다.
    private void Idle()
    {
        //필요 속성
        // 1. 플레이어 (위치)
        // 2. 
        _patrolTimer += Time.deltaTime;
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Idle -> Trace");
            CurrentState = EnemyState.Trace;
            _animator.SetTrigger("IdleToMove");
            return;
        }

        if (_patrolTimer >= PatrolTime)
        {
            Debug.Log("상태전환 : Idle -> Patrol");
            CurrentState = EnemyState.Patrol;
            _animator.SetTrigger("IdleToMove");
            _patrolTimer = 0f;
        }
        // 행동 : 가만히 있는다
    }

    private void Trace()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) > ReturnDistance)
        {
            Debug.Log("상태전환 : Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }
        
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("상태전환 : Trace -> Attack");
            CurrentState = EnemyState.Attack;
            _animator.SetTrigger("MoveToAttackDelay");
            return;
        }
        
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        //플레이어를 추적한다.
        // _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_player.transform.position);
    }

    private void Return()
    {
        if (Vector3.Distance(transform.position, _startPosition) <= IdleDistance)
        {
            Debug.Log("상태전환 : Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            _animator.SetTrigger("MoveToIdle");
            return;
        }
        
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Return -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }
        
        Vector3 dir = (_startPosition - transform.position).normalized;
        //플레이어를 추적한다.
        // _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        // 처음 자리로 돌아간다.,
        _agent.SetDestination(_startPosition);
    }

    public void Attack()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환 : Trace -> Attack");
            CurrentState = EnemyState.Trace;
            
            _animator.SetTrigger("AttackDelayToMove");
            return;
        }
        
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooltime)
        {
            Debug.Log("플레이어 공격!");
            // 공격한다.
            _attackTimer = 0;
            
            _animator.SetTrigger("AttackDelayToAttack");
            // 플레이어 체력 감소
            Damage damage = new Damage();
            damage.Value = 10;
            damage.From = this.gameObject;
            damage.KnockBack = 0;
            _player.GetComponent<Player>().TakeDamage(damage);
        }
        
    }
    private IEnumerator Damaged_Coroutine()
    {   
    //     // 공격을 당한다.
    //     _damageTimer += Time.deltaTime;
    //     if (_damageTimer >= DamagedTime)
    //     {
    //         _damageTimer = 0;
    //         Debug.Log("상태전환 : Damaged -> Trace");
    //         CurrentState = EnemyState.Trace;
    //     }
    _agent.isStopped = true;
    _agent.ResetPath();
    yield return new WaitForSeconds(DamagedTime);
    Debug.Log("");
    CurrentState = EnemyState.Trace;
    }

    private void Die()
    {
        _agent.isStopped = true;
    }
    
    private IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(DeathTime);
        Health = MaxHealth;
        HealthSet();
        DropCoins();
        CurrentState = EnemyState.Idle;
        gameObject.SetActive(false);
        //죽는다.
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Patrol -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }
        
        if (Vector3.Distance(transform.position, goalDir) <= PatrolDistance)
        {
            int randomIndex = UnityEngine.Random.Range(0, PatrolPoint.Count);
            goalDir = PatrolPoint[randomIndex];
            Debug.Log(goalDir);
            return;
        }
        _characterController.Move((goalDir - transform.position).normalized * MoveSpeed * Time.deltaTime);

    }

    private void SetPatrol()
    {
        _startPosition = transform.position;
        
        goalDir = _startPosition;
        PatrolPoint[0] = _startPosition;
        
        for (int i = 1; i < PatrolPoint.Count; i++)
        {
            PatrolPoint[i] = LocationData.PatrolLocation[i];
        }
    }

    private void HealthSet()
    {
        HealthBar.SetHealth(MaxHealth);
        HealthBar.HealthbarRefresh(Health);
    }


    private void DropCoins()
    {
        _objectPool.MakeObject(transform.position);
    }
}
