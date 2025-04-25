using System;
using UnityEngine;

public class Drum : MonoBehaviour
{
    public GameObject ExplosionVFXPrefab;
    public int Health = 20;
    public int Damage = 100;
    public float ExplodeRange = 5f;
    private Rigidbody _rigidbody;
    private bool _isDead = false;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;

        if (Health <= 0)
        {
            Explode();
        }
    }

    public void Explode()
    {
        _isDead = true;
        
        if (ExplosionVFXPrefab != null)
        {
            GameObject explosion = Instantiate(ExplosionVFXPrefab);
            explosion.transform.position = this.transform.position;       
        }
        _rigidbody.AddForce(Vector3.up * 200, ForceMode.Impulse);
        _rigidbody.AddTorque(Vector3.up * 200, ForceMode.Impulse);
        
        Collider[] colls = Physics.OverlapSphere(this.transform.position, ExplodeRange, ~(1<<10));
        //                                                                                    ㄴ 9번 빼고 다 켜겠다
        
        // Collider[] colls = Physics.OverlapSphere(this.transform.position, ExplodeRange, ~ LayerMask.NameToLayer("Drum"));
        foreach (Collider coll in colls)
        {
            if(coll.TryGetComponent(out IDamageable damageable))
            {
                Damage damage = new Damage();
                damage.Value = Damage;
                
                damageable.TakeDamage(damage);
            }
        }
        
        Collider[] drums = Physics.OverlapSphere(this.transform.position, ExplodeRange,1<<10);
        //                                                                                     ㄴ 10번 비트를 키겠다(1로 바꾸겠다).
        foreach (Collider drumc in drums)
        {
            if(drumc.TryGetComponent(out Drum drum))
            {
                drum.Explode();
            }
        }
        // 유니티는 레이어를 넘버링하는게 아니라 비트로 관리한다.
        // 2진수로 표현한다.
        
    }
}
