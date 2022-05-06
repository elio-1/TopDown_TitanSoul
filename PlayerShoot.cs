using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform _barrel;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Vector2 _barrelOffSet;
    [SerializeField] float innacuracyRange = 10;
    public int _damage = 10;
    private Animator animator;
    private Vector2 _direction;
    private Vector2 _facingDirection;
    public float timeBetweenShots = 0.3f;
    private float timeLastShot;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        _direction = new Vector2(0, -1);
    }
    private void Update()
    {
        timeLastShot -= Time.deltaTime;
        _direction.x = animator.GetFloat("MoveSpeedX");
        _direction.y = animator.GetFloat("MoveSpeedY");
        if (_direction.magnitude > 0)
        {
            _facingDirection = _direction;
            BarrelDirection(_direction);
        if (Input.GetButton("Fire1") && timeLastShot < 0)
        {
            Shot();
        }
        }
        else
        {
            BarrelDirection(_facingDirection);
            if (Input.GetButton("Fire1") && timeLastShot < 0)
            {
                Shot();
            }
        }



    }


    void Shot()
    {
        Vector3 innacuracy = new Vector3(0, 0, Random.Range(-innacuracyRange,innacuracyRange));
        Vector3 random =new Vector3(0,0, _barrel.eulerAngles.z + innacuracy.z);
        Instantiate(bullet, _barrel.position, Quaternion.Euler(random));
        timeLastShot = timeBetweenShots;
    }
    private void BarrelDirection(Vector2 direction)
    {
        if (_direction.x > .4)
        {
            _barrel.position = new Vector3(transform.position.x + direction.normalized.x - _barrelOffSet.x, 
                transform.position.y + direction.normalized.y + _barrelOffSet.y, 0);
            if (_direction.y <0 )
            {
                _barrel.rotation = Quaternion.Euler(0, 0, -45);
            }
            else if (_direction.y > 0)
            {
                _barrel.rotation = Quaternion.Euler(0, 0, 45);
            }
            else
            {
                _barrel.rotation = Quaternion.Euler(0, 0, 0);

            }
        }
        if (_direction.x < -.4f)
        {

        _barrel.position = new Vector3(transform.position.x + direction.normalized.x +_barrelOffSet.x, 
            transform.position.y + direction.normalized.y + _barrelOffSet.y, 0);
            if (_direction.y < 0)
            {
                _barrel.rotation = Quaternion.Euler(0, 0, -135);
            }
            else if (_direction.y > 0)
            {
                _barrel.rotation = Quaternion.Euler(0, 0,135);
            }
            else
            {
                _barrel.rotation = Quaternion.Euler(0, 0, 180);

            }
        }
        if (_direction.x == 0)
        {
            _barrel.position = new Vector3(transform.position.x + direction.normalized.x, 
                transform.position.y + direction.normalized.y + _barrelOffSet.y, 0);
            if (_direction.y < 0)
            {
                _barrel.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                _barrel.rotation = Quaternion.Euler(0, 0, 90);

            }
        }
    }

}
