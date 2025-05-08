using System;
using Unity.Mathematics;
using UnityEngine;

public class CoinSingle : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _position;
    private float _rotateSpeed = 180f;
    private float _amplitute = 0.2f;
    private float _frequency = 0.2f;
    private float _coinDistance = 3f;
    private float _coinSpeed = 10f;
    private bool _moving = false;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _position = transform.position;
    }
    
    private void Update()
    {
        
        MoveToPlayer();
        Rotation();
        Movement();
    }
    
    // 위아래로 움직임
    private void Movement()
    {
        if (_moving)
        {
            return;
        }
        float yOffset = Mathf.Sin(Time.deltaTime * _frequency) * _amplitute;
        transform.position = _position + new Vector3(0, yOffset, 0);
    }
    
    // 코인 회전
    private void Rotation()
    {
        transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
    }
    
    // 플레이어에게 빨려들어가기
    private void MoveToPlayer()
    {
        float distance = Vector3.Distance(_player.transform.position, transform.position);
        
        if (distance <= _coinDistance)
        {
            _moving = true;
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            transform.position += direction * _coinSpeed * Time.deltaTime;
        }
        else
        {
            _moving = false;
            _position = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);   
        }
    }
}
