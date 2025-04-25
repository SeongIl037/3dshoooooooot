using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Barrel : MonoBehaviour,IDamageable
{   
    private Player _player;
    private int _barrelHealth = 50;
    public GameObject ExplosionVFX;
    private int _explosionDamages = 50;
    private float _explosionForce = 10;
    private bool _isExploded = false;    
    // 범위
    private float _radius = 5f;
    private Rigidbody _rigidbody;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void TakeDamage(Damage damage)
    {
        if (_isExploded)
        {
            return;
        }
        _barrelHealth -= damage.Value;

        if (_barrelHealth <= 0)
        {
            CheckAround();
            StartCoroutine(DestroyBarrel());
        }
    }

    // 폭발 범위 감지하기
    private void CheckAround()
    {
        // 주변 물체 체크하기
        Collider[] around = Physics.OverlapSphere(this.transform.position, _radius, ~(1<<10));
        
        Damage damage = new Damage();
        damage.Value = _explosionDamages;
        damage.From = this.gameObject;
        damage.KnockBack = 20;
        foreach (Collider check in around)
        {
            if (check.gameObject == this.gameObject)
                continue;
            
            IDamageable damageable = check.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        } 
        Collider[] drums = Physics.OverlapSphere(this.transform.position, _radius, 1<<10);
        foreach (Collider drumc in drums)
        {
            if (drumc.TryGetComponent(out Barrel barrel))
            {
                barrel.Explode();
            }
        }
    }
    // 사라지기
    private IEnumerator DestroyBarrel()
    {
        _isExploded = true;
        
        GameObject vfx = Instantiate(ExplosionVFX);
        vfx.transform.position = this.transform.position;
        
        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
    }

    private void Explode()
    { 
        _rigidbody.AddExplosionForce(_explosionForce, transform.position, _radius, 3f, ForceMode.Impulse);
        _rigidbody.AddTorque(transform.position, ForceMode.Impulse);
    }
    
}
