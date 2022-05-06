using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyHealth : MonoBehaviour
{
    [SerializeField] DataValues maxhealth;
    [SerializeField] int enemyIndex;
    [HideInInspector] public int currentHealth;
    void Start()
    {
        currentHealth = maxhealth.values[enemyIndex];
    }

    void Update()
    {
        if (currentHealth <= 0 )
        {
            Destroy(gameObject);
        }
    }
}
