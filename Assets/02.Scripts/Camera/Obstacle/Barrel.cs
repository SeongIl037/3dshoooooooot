using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Barrel : MonoBehaviour
{   
    private Player _player;
    private int _barrelHealth = 100;
    public GameObject ExplosionVFX;
    private int _explosionDamages = 50;
    private float _explosionForce = 700;
    
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
        Collider[] around = Physics.OverlapSphere(this.transform.position, _radius);
        
        foreach (Collider check in around)
        {
            if (check.CompareTag("Player"))
            {
                _player.Health -= _explosionDamages;
                Debug.Log(_player.Health);
            }
            else if (check.CompareTag("Enemy"))
            {
                check.GetComponent<Enemy>().Health -= _explosionDamages;
                Debug.Log(check.GetComponent<Enemy>().Health);
            }

            Rigidbody rb = check.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce,transform.position,_radius);
            }

        }

        _rigidbody.AddTorque(transform.position);
        
    }
    // 사라지기
    private IEnumerator DestroyBarrel()
    {
        GameObject vfx = Instantiate(ExplosionVFX);
        vfx.transform.position = this.transform.position;
        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
    }
    
}
