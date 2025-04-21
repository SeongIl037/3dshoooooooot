using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Scriptable Objects/PlayerStat")]
public class PlayerStatSO : ScriptableObject
{
    public float WalkSpeed;
    public float RunSpeed;
    public float JumpPower;
    public float DashPower;
    public float DashSpeed;
    public float ClimbSpeed;
    public float DashStamina;
    public float Stamina;
    public float StaminaMax;
    public int MaxJumpCount;
}
