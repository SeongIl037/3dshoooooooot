using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using Collider = UnityEngine.Collider;

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
    public int AttackDamage { get; private set; }
    public float RunDistance { get; private set; } = 10f;
    private bool _isAttack = false;
    // 현재 상태
    public EnemyState CurrentState;
    // component 받아올 목록
    private EnemyHit _hit;
    private Animator _animator;
    public HPBar HealthBar;
    private GameObject _player;
    private NavMeshAgent _agent;
    public Light[] ExplodeLights;
    public ParticleSystem[] ExplodeParticles;
    // 공격 범위
    public float ExplodeRadius = 10;
    // 타이머
    private float _attackTimer;
    
    // 셋팅
    private void OnEnable()
    {
        Health = _data.Health;
        AttackDamage = _data.Damage;
        HealthBar.SetHealth(Health);
        CurrentState = EnemyState.Idle;
    }

    private void Start()
    {
        _hit = GetComponent<EnemyHit>();
        _animator = GetComponentInChildren<Animator>();
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
        HealthBar.HealthbarRefresh(Health);
        
        if (Health <= 0)
        {
            _animator.SetTrigger("Die");
            CurrentState = EnemyState.Die;
            Debug.Log($"상태전환 {CurrentState} -> Died");
            _agent.isStopped = true;
            StartCoroutine(Explode());
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
            Debug.Log("Idle -> trace");
            _animator.SetTrigger("IdleToTrace");
        }
    }
    // player에게 다가옴
    private void Trace()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= AttackDistance)
        {
            Debug.Log("trace -> attack");
            CurrentState = EnemyState.Attack;
            _animator.SetTrigger("TraceToAttack");
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) <= RunDistance)
        {
            Debug.Log("trace -> run");
            CurrentState = EnemyState.Run;
            _animator.SetTrigger("TraceToRun");
            return;
        }

        _agent.speed = MoveSpeed;
        _agent.SetDestination(_player.transform.position);
    }

    private void Run()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= AttackDistance)
        {
            CurrentState = EnemyState.Attack;
            _animator.SetTrigger("RunToAttack");
            Debug.Log("run -> Attack");
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) > RunDistance)
        {
            CurrentState = EnemyState.Trace;
            _animator.SetTrigger("RunToTrace");
            Debug.Log("run -> trace");
            return;
        }
        _agent.speed = RunSpeed;
        _agent.SetDestination(_player.transform.position);
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
        yield return new WaitForSeconds(1f);
        _animator.SetTrigger("Attack");
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
        _animator.SetTrigger("Hit");
        StartCoroutine(Hit_Coroutine());
    }

    private IEnumerator Hit_Coroutine()
    {
        _agent.isStopped = true;
        yield return new WaitForSeconds(0.2f);
        _agent.isStopped = false;
        CurrentState = EnemyState.Trace;
    }
    private void DropCoins()
    {
        
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < ExplodeLights.Length; j++)
            {
                ExplodeLights[j].range += 0.5f;
            }
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < ExplodeParticles.Length; i++)
        {
            ExplodeParticles[i].Play();
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, ExplodeRadius);
        
        foreach (Collider hit in hits)
        {
            Damage damage = new Damage();
            damage.Value = AttackDamage;
            if (hit.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    
}
