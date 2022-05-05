using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float zOffset = -10f;
    private Vector3 velocity = Vector3.zero;
    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, zOffset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime * Time.deltaTime);
        // transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime * Time.deltaTime);
    }
}
