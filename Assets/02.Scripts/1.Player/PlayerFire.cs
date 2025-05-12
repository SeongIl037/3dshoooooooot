using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public int BulletCount = 50;
    public float FireCooldown = 0.5f;
    public float Reloading = 0;
        
    private bool _reload = false;
    // 필요 속성
    // - 발사 위치
    public GameObject FirePosition;
    private Camera _mainCamera;
    public ParticleSystem BulletEffect;
    public ParticleSystem MuzzleFlash;
    // 피 이펙트 생성
    public ObjectPool BloodEffect;
    private Ray _rayPosition;
    private Player _player;
    public Lazer Lazer;
    
    private Animator _animator;

    public int ZoomInSize = 15;
    public int ZoomOutSize = 60;
    public GameObject UI_SniperZoom;
    public GameObject UI_Crosshair;
    private bool _zoomMode = false;
    private void Start()
    {
        _player = GetComponent<Player>();
        _animator = GetComponentInChildren<Animator>();
        UIManager.instance.BulletRefresh(BulletCount,_player.BulletMaxCount);

        _mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // 줌 인 아웃
        if (Input.GetMouseButtonDown(1))
        {
            ZoomInOut();
        }
        // 총일 경우에만 발사 하게하기
        if (_player.CurrentWeapon != WeaponType.Gun)
        {
            return;
        }
        // 발사 쿨타임
        if (FireCooldown < _player.FireCooldown)
        {
            FireCooldown += Time.deltaTime;
        }
        
        // 총 발사
        if (Input.GetMouseButton(0))
        {
            ReloadCancel();
            if (FireCooldown >= _player.FireCooldown && BulletCount > 0)
            {
                _animator.SetTrigger("Shot");
                MuzzleFlash.Play();
                _mainCamera.GetComponent<CameraShake>().ShakeCamera();
                BulletFire();
            }
        }
        // 리로딩
        if (Input.GetKeyDown(KeyCode.R)&& BulletCount < _player.BulletMaxCount)
        {
            _reload = true;
        }

        if (_reload)
        {
            Reload();
        }
        
        //폭탄
        // 2. 오른쪽 버튼 입력 받기
        
    }
    // 카메라 모드에 따른 공격방향 설정
    private void RaySetting()
    {
        // 쿼터뷰 모드일 때에는 캐릭터가 향하는 방향으로 발사한다.
        if (CameraChanger.instance.Type == CameraType.QuarterCamera)
        {
            _rayPosition = new Ray(transform.position, transform.forward);
        }
        else
        {
            _rayPosition = new  Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }
        
    }
    private void BulletFire()
    {
        RaySetting();
        RaycastHit hitInfo = new RaycastHit();

        bool isHit = Physics.Raycast(_rayPosition, out hitInfo);
        Lazer.LazerShoot(FirePosition.transform.position, hitInfo.point);

        if (isHit)
        {
            BulletEffect.transform.position = hitInfo.point; // 피격 이펙트 생성
            BulletEffect.transform.forward = hitInfo.normal;
            BulletEffect.Play();
            BulletCount -= 1;
            FireCooldown = 0;
            // 게임 수학 : 선형대수학, 기하학
            UIManager.instance.BulletRefresh(BulletCount,_player.BulletMaxCount);
            // 총알을 맞은 친구가 Idamageable 구현체라면..
            // if(hitInfo.collider.TryGetComponent<IDamageable>(out damagable in IDamageable)) -> IDamagealbe이 있다면
            IDamageable damageable = hitInfo.collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Damage damage = new Damage();
                damage.Value = 10;
                damage.From = this.gameObject;
                damage.KnockBack = 50;
                
                damageable.TakeDamage(damage);
                    
            }

            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                BloodEffect.MakeObject(hitInfo.point);
            }
            
        }
    }
    private void Reload()
    {
        Reloading += Time.deltaTime;
        UIManager.instance.ReloadBarRefresh(Reloading, _player.ReloadTime, true);
        if (Reloading >= _player.ReloadTime)
        {
            BulletCount = _player.BulletMaxCount;
            UIManager.instance.BulletRefresh(BulletCount, _player.BulletMaxCount);
            Reloading = 0;
            _reload = false;
            UIManager.instance.ReloadBarRefresh(Reloading,_player.ReloadTime,false);
        }
    }

    private void ReloadCancel()
    {
        if (!_reload)
        {
            return;
        }
        Reloading = 0;
        _reload = false;
        UIManager.instance.ReloadBarRefresh(Reloading,_player.ReloadTime,false);
    }

    private void ZoomInOut()
    {
        _zoomMode = !_zoomMode;
        if (_zoomMode)
        {
            UI_SniperZoom.SetActive(true);
            UI_Crosshair.SetActive(false);
            _mainCamera.fieldOfView = ZoomInSize;
        }
        else
        {
            UI_SniperZoom.SetActive(false);
            UI_Crosshair.SetActive(true);
            _mainCamera.fieldOfView = ZoomOutSize;
        }

    }
}
