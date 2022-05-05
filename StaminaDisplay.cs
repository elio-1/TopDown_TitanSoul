using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaminaDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI staminaText;
    [SerializeField] PlayerMovement Player;
    private float playerStamina;
    private void Awake()
    {
    }
    void Update()
    {
        playerStamina = Player.currentStamina;

        staminaText.text ="Stamina: " + playerStamina.ToString();
    }
}
