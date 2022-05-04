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
    
    [SerializeField] private float _runSpeed =10f;
    [SerializeField] private float _sprintMultplier = 1.5f;
    [SerializeField] private float _rollMultplier = 1.5f;
    [SerializeField] private float _rollForce = 10f;
    [SerializeField] private float _rollDuration = 1;
    
    private bool _isRolling = false;
    private float _rolltimer = 0f;
    private bool _isSprinting = false;
    private bool _isMoving = false;
    private Vector2 _rollDirection;

    public PlayerState _currentState;
    private Vector2 _direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        TransitionToState(PlayerState.IDLE);
    }
    private void Update()
    {
        
        StateUpdate();
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");
        if (_currentState != PlayerState.ROLL)
            {
            animator.SetFloat("MoveSpeedX", _direction.normalized.x);
            animator.SetFloat("MoveSpeedY", _direction.normalized.y);
            }
        if (Mathf.Abs(_direction.x) > 0.1f || Mathf.Abs(_direction.y) > 0.1f)
        {
            _isMoving = true;
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
                _isSprinting = true;
                animator.SetBool("isSprinting", true);
                break;

            case PlayerState.ROLL:
                _rolltimer = _rollDuration;
                _rollDirection = _direction;
                _isRolling = true;
                animator.SetBool("IsRolling", true);
                break;

            default:
                break;
        }
    }
    void StateUpdate()
    {
        
        switch (_currentState)
        {
            case PlayerState.IDLE:
                PlayerRollInput();
                PlayerSprintInput();
                if (_isMoving)
                {
                    TransitionToState(PlayerState.RUN);
                }
                break;

            case PlayerState.RUN:
                PlayerRollInput();
                PlayerSprintInput();
                if (!_isMoving)
                {
                    TransitionToState(PlayerState.IDLE);
                }
                break;
                
            case PlayerState.SPRINT:
                PlayerRollInput();
                PlayerSprintInput();
                break;

            case PlayerState.ROLL:
                _rolltimer -= Time.deltaTime;
                    animator.SetFloat("MoveSpeedX", _rollDirection.normalized.x);
                    animator.SetFloat("MoveSpeedY", _rollDirection.normalized.y);
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
                rb.velocity = _rollDirection.normalized * _rollForce * _rollMultplier * Time.deltaTime;
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
        if ((Input.GetButtonDown("Jump") && !_isRolling) || _isSprinting && Input.GetButtonDown("Jump") && !_isRolling)
        {
            TransitionToState(PlayerState.ROLL);
        }
    }
    void PlayerSprintInput()
    {
        if (!_isRolling && _isMoving)
        {
            if (Input.GetButton("Fire3"))
            {
                TransitionToState(PlayerState.SPRINT);

            }
        }  
        if (Input.GetButtonUp("Fire3"))
        {
            TransitionToState(PlayerState.IDLE);
        }       
    }
}