using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MeleeAction : MonoBehaviour
{
    private Transform _transform;
    private GameObject _player;
    private float _start = -70f;
    private float _end = 30f;
    private float _actionSpeed = 0.5f;
    private float _actionTimer = 0;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _transform = this.transform;
    }

    private void Update()
    {
        _transform.rotation = _player.transform.rotation;
        
        if (Input.GetMouseButton(0))
        {
            Action();
        }
    }

    private void Action()
    {
        
        if (_player.GetComponent<Player>().CurrentWeapon != WeaponType.Melee)
        {
            return;
        }
        float start = _player.transform.rotation.y +_start;
        float end = _player.transform.rotation.y + _end;
        _transform.rotation = new Quaternion(_transform.rotation.x, start, _transform.rotation.z,0);
        _transform.DORotate(new Vector3(_transform.rotation.x, end, _transform.rotation.z), _actionSpeed).SetEase(Ease.OutCubic);

    }
    //
    // private IEnumerator Action_Coroutine()
    // {
    //     for (int i = 0; i < _actionSpeed; i++)
    //     {
    //         _actionTimer += Time.deltaTime;
    //         _transform.eulerAngles = Vector3.Lerp(new Vector3(0,_start,0), new Vector3(0,_end,0), _actionSpeed / _actionTimer);
    //         
    //         yield return null;
    //     }
    //     _transform.eulerAngles = new Vector3(0, 0, 0);
    // }
}
