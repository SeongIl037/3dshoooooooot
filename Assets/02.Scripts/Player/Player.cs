using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Networking;

public enum WeaponType
{
    Gun,
    Melee,
    Grandae
    
}
public class Player : MonoBehaviour,IDamageable
{
    public float WheelCheck = 0;
    public Rig PlayerRig;
    public WeaponType CurrentWeapon;
    public Animator PlayerMask;
    [SerializeField]
    private PlayerStatSO MoveDatas;
    // 무브 데이터
    public float WalkSpeed => MoveDatas.WalkSpeed;
    public float RunSpeed => MoveDatas.RunSpeed;
    public float JumpPower => MoveDatas.JumpPower;
    public float DashPower => MoveDatas.DashPower;
    public float DashTime => MoveDatas.DashTime;
    public float ClimbSpeed => MoveDatas.ClimbSpeed;
    public float DashStamina => MoveDatas.DashStamina;
    public int StaminaMax => MoveDatas.StaminaMax;
    public int MaxJumpCount => MoveDatas.MaxJumpCount;
    public int Health{get; private set;}
    // 발사 데이터
    [SerializeField]
    private PlayerFireSO FireDatas;
    public int MaxBombCount => FireDatas.MaxBombCount;
    public int BulletMaxCount => FireDatas.BulletMaxCount;
    public float ReloadTime => FireDatas.ReloadTime;
    public float FireCooldown => FireDatas.FireCooldown;
    public float ThrowPower => FireDatas.ThrowPower;
    public float ThrowPowerMax => FireDatas.ThrowPowerMax;
    [Header("stamina")] 
    public float Stamina = 0f;
    
    private void Start()
    {
        CurrentWeapon = WeaponType.Gun;
        Health = MoveDatas.Health;
    }

    private void Update()
    {
        MouseChange();
    }

    public void SetWeapon(WeaponType weapon)
    {
        CurrentWeapon = weapon;

        switch (weapon)
        {
            case WeaponType.Gun:
            {
                PlayerRig.weight = 1;
                PlayerMask.SetLayerWeight(3, 0f);
                break;
            }
            case WeaponType.Melee:
            {
                PlayerRig.weight = 0;
                PlayerMask.SetLayerWeight(3, 0.8f);
                break;
            }
            case WeaponType.Grandae:
            {
                PlayerRig.weight = 0;
                PlayerMask.SetLayerWeight(3, 0.8f);
                break;
            }
        }
        
        UIManager.instance.UIWeaponChange(weapon);
    }
    // 마우스 휠처리
    private void MouseChange()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        WheelCheck -= wheel;
        WheelCheck = Mathf.Clamp(WheelCheck, 0, 2);
    }
    // private void WeaponChange()
    // {
    //     if(Input.GetKeyDown(KeyCode.Alpha1) || WheelCheck <= (int)WeaponType.Gun)
    //     {
    //         CurrentWeapon = WeaponType.Gun;
    //         WheelCheck = (int)WeaponType.Gun;
    //         PlayerRig.weight = 1;
    //         PlayerMask.SetLayerWeight(3, 0f);
    //         UIManager.instance.UIWeaponChange(CurrentWeapon);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.Alpha2) || WheelCheck <= (int)WeaponType.Melee)
    //     {
    //         CurrentWeapon = WeaponType.Melee;
    //         WheelCheck = (int)WeaponType.Melee;
    //         PlayerRig.weight = 0;
    //         PlayerMask.SetLayerWeight(3, 0.8f);
    //         
    //         UIManager.instance.UIWeaponChange(CurrentWeapon);
    //     }
    //     else if(Input.GetKeyDown(KeyCode.Alpha3) ||  WheelCheck <= (int)WeaponType.Grandae)
    //     {
    //         CurrentWeapon = WeaponType.Grandae;
    //         WheelCheck = (int)WeaponType.Grandae;
    //         PlayerRig.weight = 0;
    //         PlayerMask.SetLayerWeight(3, 0.8f);
    //         
    //         UIManager.instance.UIWeaponChange(CurrentWeapon);
    //     }
    // }
    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        UIManager.instance.HealthSliderRefresh(UIManager.instance.PlayerHealthSlider, Health);
        // 피격 이펙트
        UIManager.instance.PlayerHit();
        if (Health <= 0)
        {
            GameManager.instance.OverGame();
        }
    }
    public void StaminaRecovery()
    {
        Stamina += Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, StaminaMax);
        UIManager.instance.SliderRefresh(UIManager.instance.StaminaBar, Stamina);
    }
    public void StaminaDecrease()
    {
        Stamina -= Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, StaminaMax);
        UIManager.instance.SliderRefresh(UIManager.instance.StaminaBar, Stamina);
    }
    
}
