using System;
using UnityEngine;

public class PlayerWeaponChange : MonoBehaviour
{
    private Player _player;
    private int _weaponLength = 3;
    public GameObject Gun;
    public GameObject Knife;
    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        KeyboardWeaponChange();
        WeaponChange();
    }
    private void KeyboardWeaponChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _player.SetWeapon(WeaponType.Gun);
            Gun.SetActive(true);
            Knife.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _player.SetWeapon(WeaponType.Melee);
            Gun.SetActive(false);
            Knife.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _player.SetWeapon(WeaponType.Grandae);
        }
    }

    private void WeaponChange()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel > 0)
        {
            WeaponType weapon = (WeaponType)(((int)_player.CurrentWeapon + 1) % _weaponLength);
            _player.SetWeapon(weapon);
        }
        else if (wheel < 0)
        {
            int weapon = ((int)_player.CurrentWeapon - 1 + _weaponLength) % _weaponLength;
            _player.SetWeapon((WeaponType)weapon);
            
        }
    }
}
