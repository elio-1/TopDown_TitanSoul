using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement _playerMoveScript;
    public float _maxHealth = 100f;
    
    [HideInInspector] public float _currentHealth;
    [HideInInspector] public bool _isInvulnerable = false;
    

    private void Awake()
    {
        _playerMoveScript = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
        _currentHealth = _maxHealth;
    }
    private void Update()
    {
        if (Input.GetKeyDown("t"))
        {
        LoseHealth(5f);
        }
        if (_playerMoveScript._currentState == PlayerState.ROLL)
        {
            _isInvulnerable = true;
        }
        else
        {
            _isInvulnerable = false;
        }
    }

    public void LoseHealth(float damageTaken)
    {
        if (!_isInvulnerable)
        {
        _currentHealth -= damageTaken;
        //play damage Animation
        }
    }
}
