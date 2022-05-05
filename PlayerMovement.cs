using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState 
{
    IDLE,
    RUN,
    SPRINT,
    ROLL
}

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [HideInInspector] public PlayerState _currentState;
    
    [Header("Run and Sprint")]
    [SerializeField] private float _runSpeed =10f;
    [SerializeField] private float _sprintMultplier = 1.5f;
    [SerializeField] private float _sprintStaminaCost = 1;
    [SerializeField] private float _minStaminaRequiredToSprintAgain = 15;

    [Header("Roll")]
    [SerializeField] private float _rollMultplier = 1.5f;
    [SerializeField] private float _rollForce = 10f;
    [SerializeField] private float _rollDuration = 1;
    [SerializeField] private float _rollStaminaCost = 20;
    [SerializeField] AnimationCurve _rollCurve;

    [Header("Stamina")]
    [SerializeField] private float maxStamina =100;
    [SerializeField] private float _staminaRegen = 15 ;
    [SerializeField] private float _idleStaminaRegen = 30;
    [HideInInspector] public float currentStamina;


    #region Private & Protected
    private bool _isRolling = false;
    private float _rolltimer = 0f;
    private bool _isSprinting = false;
    private bool _isMoving = false;
    private Vector2 _rollDirection;
    private Vector2 _facingDirection = new Vector2();
    private Vector2 _direction;
    #endregion
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        currentStamina = maxStamina;
        TransitionToState(PlayerState.IDLE);
    }
    private void Update()
    {
        StateUpdate();
        
        if (Mathf.Abs(_direction.x) > 0.1f || Mathf.Abs(_direction.y) > 0.1f)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
    }
    private void FixedUpdate()
    {
        StateFixedUpdate();    
    }
    void EnterState()
    {
        switch (_currentState)
        {
            case PlayerState.IDLE:
                break;

            case PlayerState.RUN:
                break;

            case PlayerState.SPRINT:
                if (_isMoving)
                {
                    animator.SetBool("isSprinting", true);
                }
               
                    break;

            case PlayerState.ROLL:
                currentStamina -= _rollStaminaCost;
                _rolltimer = _rollDuration;
                if (_direction.magnitude > 0)
                {
                _rollDirection = _direction;
                }
                else
                {
                    _rollDirection = _facingDirection;

                }
                _isRolling = true;
                animator.SetBool("IsRolling", true);
                animator.SetFloat("MoveSpeedX", _rollDirection.normalized.x);
                animator.SetFloat("MoveSpeedY", _rollDirection.normalized.y);
                break;

            default:
                break;
        }
    }

    void StateUpdate()
    {
        StaminaRegen(!_isRolling && !_isSprinting);

        switch (_currentState)
        {
            case PlayerState.IDLE:
                PlayerRollInput();
                PlayerSprintInput();
                SetDirection();
                if (_isMoving)
                {
                    TransitionToState(PlayerState.RUN);
                }
                break;

            case PlayerState.RUN:
                PlayerRollInput();
                SetDirection();
                PlayerSprintInput();
                if (!_isMoving)
                {
                    TransitionToState(PlayerState.IDLE);
                }
                break;
                
            case PlayerState.SPRINT:
                SetDirection();
                PlayerRollInput();
                PlayerSprintInput();
                if(!_isMoving)
                {
                    animator.SetBool("isSprinting", false);
                }
                else
                {
                    currentStamina -= _sprintStaminaCost * Time.deltaTime;
                }
                break;

            case PlayerState.ROLL:
                _rolltimer -= Time.deltaTime;
                    
                if (_rolltimer<0)
                {
                    TransitionToState(PlayerState.IDLE);
                }
                break;

            default:
                break;
        }
    }
    void StateFixedUpdate()
    {
        switch (_currentState)
        {
            case PlayerState.IDLE:
                rb.velocity = _direction.normalized * _runSpeed * Time.deltaTime;
                break;
            case PlayerState.RUN:
                rb.velocity = _direction.normalized * _runSpeed * Time.deltaTime;
                break;
            case PlayerState.SPRINT:
                rb.velocity = _direction.normalized * _runSpeed * _sprintMultplier* Time.deltaTime;
                break;
            case PlayerState.ROLL:
                rb.velocity = _rollDirection.normalized * _rollForce * _rollMultplier * Time.deltaTime*_rollCurve.Evaluate(_rolltimer/_rollDuration);
                break;
            default:
                break;
        }
    }
    void ExitState()
    {
        switch (_currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.RUN:
                
                break;
            case PlayerState.SPRINT:
                _isSprinting = false;
                animator.SetBool("isSprinting", false);
                break;
            case PlayerState.ROLL:
                rb.velocity = Vector2.zero;
                _rollDirection = Vector2.zero;
                _isRolling = false;
                animator.SetBool("IsRolling", false);
                break;
            default:
                break;
        }
    }
    public void TransitionToState(PlayerState newState)
    {
        ExitState();
        _currentState = newState;
        EnterState();
    }

    void PlayerRollInput()
    {
        if (currentStamina > _rollStaminaCost)
        {
            if ((Input.GetButtonDown("Jump") && !_isRolling))
            {
                TransitionToState(PlayerState.ROLL);
            }
        }
    }
    void PlayerSprintInput()
    { 
        if (!_isRolling && _isMoving && currentStamina >= _minStaminaRequiredToSprintAgain)
        {
            if (Input.GetButton("Fire3"))
            {
                TransitionToState(PlayerState.SPRINT);
                
            }
        }  
        if (Input.GetButtonUp("Fire3") || currentStamina <= 0.5f)
        {
            TransitionToState(PlayerState.IDLE);
        }


    }

    void SetDirection()
    {
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");
        if (_direction.magnitude > 0)
        {
            _facingDirection = _direction;
            animator.SetFloat("MoveSpeedX", _direction.x);
            animator.SetFloat("MoveSpeedY", _direction.y);
        }
        else
        {
            animator.SetFloat("MoveSpeedX", _facingDirection.x*.15f);
            animator.SetFloat("MoveSpeedY", _facingDirection.y*.15f);
        }
    }

    void StaminaRegen( bool IsPossible)
    {
        if (IsPossible)
        {
            if (currentStamina >= 0 && currentStamina <= 100)
            {
                if (_currentState == PlayerState.IDLE)
                {
                    currentStamina += _idleStaminaRegen * Time.deltaTime;
                }
                else
                {
                currentStamina += _staminaRegen * Time.deltaTime;
                }
            }
            if (currentStamina >= 100)
            {
                currentStamina = 100;
            }
            if (currentStamina <= 0)
            {
                currentStamina = 0;
            }
        }
    }
    
}