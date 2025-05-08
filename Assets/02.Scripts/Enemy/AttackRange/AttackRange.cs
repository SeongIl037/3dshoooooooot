using System;
using UnityEngine;
using DG.Tweening;
public class AttackRange : MonoBehaviour
{
    public GameObject Explode;
    private EliteEnemy Elite;
    private float _attackRange = 7f;
    private void OnEnable()
    {
        Increase();
    }

    private void Start()
    {
        Elite = GetComponentInParent<EliteEnemy>();
    }

    private void Increase()
    {
        transform.DOScale(new Vector3(10,10,10), 2f).OnComplete(() =>
        {
            MakeEffect();
            Attack();
            transform.localScale = new Vector3(0, 0, 0);
            gameObject.SetActive(false);
        });
    }
    private void MakeEffect()
    {
        GameObject explode = Instantiate(Explode);
        explode.transform.position = transform.position;
    }
    private void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _attackRange, ~(1<<8));
        foreach (Collider hit in colliders)
        {
            if (hit.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                Damage damage = new Damage();
                damage.Value = Elite.AttackDamage;
                
                damageable.TakeDamage(damage);
            }
        }
    }
    
}
