using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentStateText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI state;
    [SerializeField] PlayerMovement playerState;
    private void Update()
    {
        state.text = "Current State: " + playerState._currentState.ToString();
    }
}
