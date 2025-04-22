using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Player _player;
    // 움직임
    private float _moveSpeed = 5f;
    private bool _isRunning = false;
    [Header("Dash")]
    public float DashTimer = 0.5f;
    private bool _isDashing = false;
    private Vector3 _dashDirection;
    //점프
    private int _jumpCount = 0;
    private bool _isJumping = false;
    // 중력
    private const float GRAVITY = -9.8f; // 중력
    private float _yVelocity = 0f; // 중력가속도
    //벽타기
    private bool _isClimbing = false;
    private CharacterController _characterController;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        dir = dir.normalized;
        // 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);
        
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;
        _characterController.Move(dir * _moveSpeed * Time.deltaTime);
        
        Jump();
        WallClimb(); 
        
        if(_characterController.isGrounded)
        {
            _jumpCount = 0;
            Running();
            Dashing();   
        }

        StaminaIncrease();
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _jumpCount < _player.MaxJumpCount)
        {
            _yVelocity = _player.JumpPower;
            _jumpCount += 1;
        }
    }
    
    private void Running()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _player.Stamina > 0)
        {
            _isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || _player.Stamina <= 0)
        {
            _isRunning = false;
        }
        
        if (_isRunning)
        {
            _moveSpeed = _player.RunSpeed;
            _player.StaminaDecrease();
        }
        else if(_isRunning == false)
        {
            _moveSpeed = _player.WalkSpeed;
        }

    }

    private void Dashing()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isDashing == false && _player.Stamina > _player.DashStamina)
        {
            _player.Stamina -= _player.DashStamina;
            
            _isDashing = true;
            DashTimer = _player.DashTime;

            _dashDirection = Camera.main.transform.forward;
            _dashDirection.y = 0;
            _dashDirection.Normalize();
            UIManager.instance.StaminaRefresh(_player.Stamina);
            
        }
        
        if (_isDashing)
        {
            _characterController.Move(_dashDirection * _player.DashPower * Time.deltaTime);
            DashTimer -= Time.deltaTime;
            if (DashTimer <= 0)
            {
                _isDashing = false;
            }
        }
    }

    private void WallClimb()
    {
        if (_characterController.collisionFlags == CollisionFlags.Sides )
        {
            if (Input.GetButton("Jump") && _player.Stamina > 0)
            {
                _yVelocity = _player.ClimbSpeed;
                _isClimbing = true;
                _player.StaminaDecrease();
            }
            else
            {
                _yVelocity += GRAVITY;
                _isClimbing = false;
            }
        
        }
        else
        {
            _isClimbing = false;
        }
    }

    private void StaminaIncrease()
    {
        if (_isDashing || _isJumping || _isRunning || _isClimbing)
        {
            return; 
        } 
        _player.StaminaRecovery();
    }
    
}
