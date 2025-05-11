using UnityEngine;

[CreateAssetMenu(fileName = "PlayerFireSO", menuName = "Scriptable Objects/PlayerFireSO")]
public class PlayerFireSO : ScriptableObject
{
    public int MaxBombCount;
    public int BulletMaxCount;
    public float ReloadTime;
    public float FireCooldown;
    public float ThrowPower;
    public float ThrowPowerMax;
}
