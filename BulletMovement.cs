using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public float speed = 50f;
    [SerializeField] private PlayerShoot _playerShootScript;
    private int bulletDamage;

    private void Awake()
    {
        rb.velocity = transform.right * speed * Time.deltaTime;
        bulletDamage = _playerShootScript._damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<EnnemyHealth>().currentHealth -= bulletDamage;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
