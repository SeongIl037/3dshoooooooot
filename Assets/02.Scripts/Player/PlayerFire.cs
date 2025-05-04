using System;
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
    public GameObject BombPrefab;
    private Camera _mainCamera;
    public ParticleSystem BulletEffect;

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
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Cursor.lockState = CursorLockMode.None; 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Cursor.lockState = CursorLockMode.Locked; 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Cursor.lockState = CursorLockMode.Confined; 
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            // IncreaseThrowPower();
            
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

    private void BulletFire()
    {
        // 2. 레이를 생성하고 발사 위치와 진행 방향을 설정
        Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
        // 3. 레이와 부딛힌 물체의 정보를 저장할 변수를 생성
        RaycastHit hitInfo = new RaycastHit();
        // 4. 레이를 발사한 다음,                    ㄴ 에 데이터가 있다면 피격 이펙트 생성(표시)
        bool isHit = Physics.Raycast(ray, out hitInfo);
        
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
}
