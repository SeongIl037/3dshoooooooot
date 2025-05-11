using System;
using UnityEngine;

public class PlayerBoom : MonoBehaviour
{
    public ObjectPool Pool;
    // 3. 발사 위치에 수류탄 생성하기
    // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기
    public int BombCount = 3;
    public float ThrowPlusPower;
    public Transform ThrowPosition;
    public Transform TargetPosition;
    
    private Animator _animator;
    private Player _player;
    private void Start()
    {
        _player = GetComponentInParent<Player>();
        _animator = GetComponent<Animator>();
        UIManager.instance.BombRefresh(BombCount,_player.MaxBombCount);
    }

    private void Update()
    {
        if (_player.CurrentWeapon != WeaponType.Grandae || BombCount <= 0)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("Granade");
        }
        if (Input.GetMouseButton(0))
        {
            IncreaseThrowPower();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _animator.SetTrigger("GranadeToIdle");
        }
    }
    private void BombFire()
    {
        if (BombCount <= 0)
        {
            return;
        }
        
        GameObject boom = Pool.MakeObject(ThrowPosition.position);
        
        Rigidbody bombRigidBody = boom.GetComponent<Rigidbody>();
        bombRigidBody.AddForce(TargetPosition.forward *  (_player.ThrowPower * ThrowPlusPower), ForceMode.Impulse);

        BombCount -= 1;
        UIManager.instance.BombRefresh(BombCount, _player.MaxBombCount);

        ThrowPlusPower = 0;
        UIManager.instance.ThrowBar.SetActive(false);
    }

    private void IncreaseThrowPower()
    {
        UIManager.instance.ThrowBar.SetActive(true);
        ThrowPlusPower += Time.deltaTime;
        ThrowPlusPower = Mathf.Clamp(ThrowPlusPower, 1, _player.ThrowPowerMax);
        UIManager.instance.SliderRefresh(UIManager.instance.ThrowPowerSlider, ThrowPlusPower);

    }

}
