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
        Trace,
        Attack,
        Damaged,
        Die
    }
    // 데이터 SO
    [SerializeField] private EnemySO _enemyData;
    // 상태를 지정한다.
    public EnemyState CurrentState = EnemyState.Idle;
    public float MoveSpeed => _enemyData.MoveSpeed;
    public int EnemyMaxHealth => _enemyData.MaxHealth;
    public int EnemyDamage => _enemyData.Damage;
    // 거리 관련
    public float FindDistance => _enemyData.FindDistance;
    public float AttackDistance => _enemyData.AttackDistance;
    //타이머 관련
    public float AttackCooltime => _enemyData.AttackCooltime;
    public float DeathTime => _enemyData.DeathTime;
    // 변화하는 변수
    public int Health { get; private set;}
    public float DamagedTime = 0.5f;
    private float _attackTimer = 0f;
    // 컴포넌트 목록
    private CharacterController _characterController;
    private Animator _animator;
    private GameObject _player;
    private NavMeshAgent _agent;
    // 에너미 색 변경하기
    private EnemyHit _hit;
    // 오브젝트 풀 세팅
    private ObjectPool _objectPool;
    // 체력바 세팅    
    public HPBar _healthBar;
   
    // 체력 세팅
    private void OnEnable()
    {
        Health = EnemyMaxHealth;
        HealthSet();
    }

    private void Start()
    { 
        _hit = GetComponent<EnemyHit>();
        _objectPool = GameObject.FindGameObjectWithTag("CoinPool").GetComponent<ObjectPool>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed;
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
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
        _hit.HitFlash();
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
        //플레이어를 추적한다.
        _agent.SetDestination(_player.transform.position);
    }
    
    private void Atttck()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환 : Attck -> Trace");
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
            damage.Value = EnemyDamage;
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
        yield return new WaitForSeconds(DeathTime);
        DropCoins();
        gameObject.SetActive(false);
        CurrentState = EnemyState.Idle;  
        //죽는다.
    }
    private void HealthSet()
    {
        _healthBar.SetHealth(EnemyMaxHealth);
        _healthBar.HealthbarRefresh(Health); 
    }
    private void DropCoins()
    {
        _objectPool.MakeObject(transform.position);
    }
}
