using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public PlayerStatSO MoveDatas;
    
    [Header("Movement")]
    public float MoveSpeed = 5f;
    private bool _isRunning = false;
    [Header("Dash")]
    public bool _isDashing = false;
    private float _dashTimer = 0.5f;
    private Vector3 _dashDirection;
    [Header("Jump")]
    private int _jumpCount = 0;
    private bool _isJumping = false;
    [Header("Gravity")]
    private const float GRAVITY = -9.8f; // 중력
    private float _yVelocity = 0f; // 중력가속도
    [Header("stamina")]
    public float Stamina = 0f;
    [Header("climb")]
    private bool _isClimbing = false;
    private CharacterController _characterController;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {

        Move();
        
        StaminaIncrease();
        
        if(_characterController.isGrounded)
        {
            Running();
            Dashing();   
        }
        
        Jump();
        WallClimb(); 

    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        dir = dir.normalized;
        // 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);
        
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

    }
    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            _jumpCount = 0;
        }
        
        if (Input.GetButtonDown("Jump") && _jumpCount < MoveDatas.MaxJumpCount)
        {
            _yVelocity = MoveDatas.JumpPower;
            _jumpCount += 1;
        }
    }
    
    private void Running()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0)
        {
            _isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Stamina <= 0)
        {
            _isRunning = false;
        }
        
        if (_isRunning)
        {
            MoveSpeed = MoveDatas.RunSpeed;
            StaminaDecrease();
        }
        else if(_isRunning == false)
        {
            MoveSpeed = MoveDatas.WalkSpeed;
        }

    }

    private void Dashing()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isDashing == false && Stamina > MoveDatas.DashStamina)
        {
            Stamina -= MoveDatas.DashStamina;
            
            _isDashing = true;
            _dashTimer = MoveDatas.DashSpeed;

            _dashDirection = Camera.main.transform.forward;
            _dashDirection.y = 0;
            _dashDirection.Normalize();
            UIManager.instance.StaminaRefresh(Stamina);
            
        }
        
        if (_isDashing)
        {
            _characterController.Move(_dashDirection * MoveDatas.DashPower * Time.deltaTime);
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0)
            {
                _isDashing = false;
            }
        }
    }

    private void WallClimb()
    {
        if (_characterController.collisionFlags == CollisionFlags.Sides && Input.GetButton("Jump") && Stamina > 0)
        {
            _yVelocity = MoveDatas.ClimbSpeed;
            _isClimbing = true;
            StaminaDecrease();
        }
        else 
        {
            _yVelocity += GRAVITY * Time.deltaTime;
            _isClimbing = false;
        }
    }

    private void StaminaDecrease()
    {
        Stamina -= Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, MoveDatas.StaminaMax);
        UIManager.instance.StaminaRefresh(Stamina);
    }

    private void StaminaIncrease()
    {
        if (_isDashing || _isJumping || _isRunning || _isClimbing)
        {
            return; 
        } 
        Stamina += Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, MoveDatas.StaminaMax);
        UIManager.instance.StaminaRefresh(Stamina);
    }
    
}
