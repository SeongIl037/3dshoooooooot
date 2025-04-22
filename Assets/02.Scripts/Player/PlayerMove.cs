using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
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
    
    [Header("Movement")]
    public float MoveSpeed = 5f;
    private bool _isRunning = false;
    [Header("Dash")]
    public float _dashTimer = 0.5f;
    private bool _isDashing = false;
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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        dir = dir.normalized;
        // 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);
        
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        
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
        if (Input.GetButtonDown("Jump") && _jumpCount < MaxJumpCount)
        {
            _yVelocity = JumpPower;
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
            MoveSpeed = RunSpeed;
            StaminaDecrease();
        }
        else if(_isRunning == false)
        {
            MoveSpeed = WalkSpeed;
        }

    }

    private void Dashing()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isDashing == false && Stamina > DashStamina)
        {
            Stamina -= DashStamina;
            
            _isDashing = true;
            _dashTimer = DashTime;

            _dashDirection = Camera.main.transform.forward;
            _dashDirection.y = 0;
            _dashDirection.Normalize();
            UIManager.instance.StaminaRefresh(Stamina);
            
        }
        
        if (_isDashing)
        {
            _characterController.Move(_dashDirection * DashPower * Time.deltaTime);
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0)
            {
                _isDashing = false;
            }
        }
    }

    private void WallClimb()
    {
        if (_characterController.collisionFlags == CollisionFlags.Sides )
        {
            if (Input.GetButton("Jump") && Stamina > 0)
            {
                _yVelocity = ClimbSpeed;
                _isClimbing = true;
                StaminaDecrease();
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

    private void StaminaDecrease()
    {
        Stamina -= Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, StaminaMax);
        UIManager.instance.StaminaRefresh(Stamina);
    }

    private void StaminaIncrease()
    {
        if (_isDashing || _isJumping || _isRunning || _isClimbing)
        {
            return; 
        } 
        Stamina += Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, StaminaMax);
        UIManager.instance.StaminaRefresh(Stamina);
    }
    
}
