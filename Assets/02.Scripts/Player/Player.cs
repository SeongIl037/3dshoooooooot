using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerStatSO MoveDatas;
    // 기본 데이터
    public float WalkSpeed => MoveDatas.WalkSpeed;
    public float RunSpeed => MoveDatas.RunSpeed;
    public float JumpPower => MoveDatas.JumpPower;
    public float DashPower => MoveDatas.DashPower;
    public float DashTime => MoveDatas.DashTime;
    public float ClimbSpeed => MoveDatas.ClimbSpeed;
    public float DashStamina => MoveDatas.DashStamina;
    public int StaminaMax => MoveDatas.StaminaMax;
    public int MaxJumpCount => MoveDatas.MaxJumpCount;

    [Header("stamina")] 
    public float Stamina = 0f;
    public void StaminaRecovery()
    {
        Stamina += Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, StaminaMax);
        UIManager.instance.StaminaRefresh(Stamina);
    }
    public void StaminaDecrease()
    {
        Stamina -= Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, StaminaMax);
        UIManager.instance.StaminaRefresh(Stamina);
    }
}
