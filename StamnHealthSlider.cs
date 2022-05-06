using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StamnHealthSlider : MonoBehaviour
{
    [SerializeField] Slider _staminaBar;
    [SerializeField] Slider _healthBar;
    [SerializeField] PlayerMovement _playerStamina;
    [SerializeField] PlayerHealth _playerHealth;
    private void Awake()
    {
        _healthBar.maxValue =_playerHealth._maxHealth;
        _staminaBar.maxValue = _playerStamina.maxStamina;
    }
    void Update()
    {
        _staminaBar.value = _playerStamina.currentStamina;
        _healthBar.value = _playerHealth._currentHealth;
    }
}
