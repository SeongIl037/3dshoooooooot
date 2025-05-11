using System;
using UnityEngine;
using DG.Tweening;
public class AttackRange : MonoBehaviour
{
    public GameObject Explode;
    private GameObject _player;
    public GameObject Range;
    private EliteEnemy Elite;
    private float _attackRange = 7f;
    private Vector3 _rangePosition;
    // 엘리트 몬스터의 공격력을 받아오기 위함
    private void Start()
    {
        Elite = GetComponentInParent<EliteEnemy>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    // 범위 표시하기
    private void MakeAttackRange()
    {
        GameObject range = Instantiate(Range);
        range.transform.position = _player.transform.position;
        _rangePosition = range.transform.position;
        range.transform.DOScale(new Vector3(10, 10, 10), 2f).OnComplete(()=>
        {
            range.SetActive(false);
        });
    }
    // 범위 내 터지는 효과 넣기
    private void MakeEffect()
    {
        GameObject explode = Instantiate(Explode);
        explode.transform.position = _rangePosition;
    }
    // 데미지 받게 만들기
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
        MakeEffect();
    }
    
}
