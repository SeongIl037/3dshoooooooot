using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Scriptable Objects/PlayerStat")]
public class PlayerStatSO : ScriptableObject
{
    public int Health;
    public float WalkSpeed;
    public float RunSpeed;
    public float JumpPower;
    public float DashPower;
    public float DashTime;
    public float ClimbSpeed;
    public float DashStamina;
    public int StaminaMax;
    public int MaxJumpCount;
}
