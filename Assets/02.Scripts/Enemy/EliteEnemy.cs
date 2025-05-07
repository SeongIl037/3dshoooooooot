using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EliteEnemy : MonoBehaviour, IDamageable
{
    public enum EnemyState
    {
        Idle,
        Trace,
        Attack,
        Run,
        Die,
        Hit
    }
    
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
    public float RunDistance { get; private set; } = 5f;
    private bool _isAttack = false;
    // 현재 상태
    public EnemyState CurrentState;
    // component 받아올 목록
    private EnemyHit _hit;
    private Animator _animator;
    public HPBar HealthBar;
    private CharacterController _characterController;
    private GameObject _player;
    private NavMeshAgent _agent;
    // 공격 범위
    public GameObject Range;
    // 타이머
    private float _attackTimer;
    
    // 셋팅
    private void OnEnable()
    {
        Health = _data.Health;
        Damage = _data.Damage;
        CurrentState = EnemyState.Idle;
    }

    private void Start()
    {
        _hit = GetComponent<EnemyHit>();
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed;
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
                Attack();
                break;
            }
            case EnemyState.Run:
            {
                Run();
                break;
            }
            case EnemyState.Hit:
            {
                Hit();
                break;
            }
        }
    }
    // 체력 감소 (죽음 측정)
    public void TakeDamage(Damage damage)
    {
        if (CurrentState == EnemyState.Hit || CurrentState == EnemyState.Die)
        {
            return;
        }
        Health -= damage.Value;
        StartCoroutine(_hit.HitFlash());
        _animator.SetTrigger("Hit");
        HealthBar.HealthbarRefresh(Health);
        
        if (Health <= 0)
        {
            _animator.SetTrigger("Die");
            CurrentState = EnemyState.Die;
            Debug.Log($"상태전환 {CurrentState} -> Died");
            // StartCoroutine(Die_Coroutine());
            return;
        }
        Debug.Log($"상태전환 {CurrentState} -> Damaged");
        
        CurrentState = EnemyState.Hit;

        // StartCoroutine(Damaged_Coroutine());

    }
    // 제 자리에 가만히 서 있음
    private void Idle()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= FindDistance)
        {
            CurrentState = EnemyState.Trace;
            _animator.SetTrigger("IdleToFollow");
        }
    }
    // player에게 다가옴
    private void Trace()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= AttackDistance)
        {
            CurrentState = EnemyState.Attack;
            _animator.SetTrigger("TraceToAttack");
        }

        if (Vector3.Distance(transform.position, _player.transform.position) <= RunDistance)
        {
            CurrentState = EnemyState.Run;
            _animator.SetTrigger("TraceToRun");
        }
        _characterController.Move(transform.position - _player.transform.position);
    }

    private void Run()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= AttackDistance)
        {
            CurrentState = EnemyState.Attack;
            _animator.SetTrigger("RunToAttack");
        }

        if (Vector3.Distance(transform.position, _player.transform.position) > RunDistance)
        {
            CurrentState = EnemyState.Trace;
            _animator.SetTrigger("RunToTrace");
        }
        _agent.speed = RunSpeed;
        _characterController.Move(transform.position - _player.transform.position);
    }
    // 사정거리 안에 들어오면 범위 공격을 함
    private void Attack()
    {
        if (!_isAttack)
        {
            StartCoroutine(Attack_Coroutine());
        }
    }

    private IEnumerator Attack_Coroutine()
    {
        _isAttack = true;
        _agent.isStopped = true;
        Instantiate(Range);
        Range.transform.position = _player.transform.position;
        yield return new WaitForSeconds(AttackCooltime);
        CurrentState = EnemyState.Trace;
        _agent.isStopped = false;
        _isAttack = false;
    }
    // 체력이 다 달면 죽음, -> 폭발
    private IEnumerator Die()
    {
        yield return null;
    }
    // 맞으면 제자리에 잠시 멈춤 (넉백되지 않음)
    private void Hit()
    {

    }
    private void DropCoins()
    {
        
    }
}
