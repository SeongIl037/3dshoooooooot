using UnityEngine;

public class Player : MonoBehaviour,IDamageable
{
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
    public float Health = 100f;

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
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
