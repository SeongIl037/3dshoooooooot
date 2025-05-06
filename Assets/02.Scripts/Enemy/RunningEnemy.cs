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
    private MaterialPropertyBlock _block;
    private int _colorID;
    public SkinnedMeshRenderer[] ZombieSkinnedMeshRenderers;

    private GameObject _player;
    private NavMeshAgent _agent;
    private CharacterController _characterController;
    private Animator _animator;
    // 거리 관련
    public float FindDistance = 5f;
    public float AttackDistance = 2.5f;
    
    //타이머 관련
    public float AttackCooltime = 2f;
    public float DeathTime = 2f;
    public float DamagedTime = 0.5f;
    private float _attackTimer = 0f;
    private ObjectPool _objectPool;
    
    private void Start()
    { 
        HealthSet();
        ZombieSkinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        _objectPool = GameObject.FindGameObjectWithTag("CoinPool").GetComponent<ObjectPool>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed;
        _colorID = Shader.PropertyToID("_BaseColor");
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _block = new MaterialPropertyBlock();
        _animator = GetComponentInChildren<Animator>();   
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
        StartCoroutine(HitFlash());
        _animator.SetTrigger("Hit");
        _healthBar.HealthbarRefresh(Health);
        
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
    // 상태 함수들을 구현한다.
    private void Idle()
    {

        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Idle -> Trace");
            CurrentState = EnemyState.Trace;
            _animator.SetTrigger("IdleToMove");
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
            _animator.SetTrigger("MoveToAttackDelay");
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
            Damage damage = new Damage();
            damage.Value = 10;
            damage.From = this.gameObject;
            damage.KnockBack = 0;
            _player.GetComponent<Player>().TakeDamage(damage);
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
        CurrentState = EnemyState.Idle;
        yield return new WaitForSeconds(DeathTime);
        DropCoins();
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
    private IEnumerator HitFlash()
    {
        foreach (SkinnedMeshRenderer skin in ZombieSkinnedMeshRenderers )
        {
            skin.GetPropertyBlock(_block);
            _block.SetColor(_colorID, Color.red);
            skin.SetPropertyBlock(_block);
        }
        yield return new WaitForSeconds(0.2f);
        
        foreach (SkinnedMeshRenderer skin in ZombieSkinnedMeshRenderers )
        {
            skin.GetPropertyBlock(_block);
            _block.SetColor(_colorID, Color.white);
            skin.SetPropertyBlock(_block);
        }
    }

    private void DropCoins()
    {
        _objectPool.MakeObject(transform.position);
    }
}
