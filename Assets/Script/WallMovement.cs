using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour
{

    public float speed = 10f;
    public float distance = 30f;

    private Vector3 startPos;
    private Vector3 newPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (gameObject.CompareTag("MovingWallLeft"))
        {
            newPos = startPos;
            newPos.x = newPos.x + Mathf.PingPong(Time.time * speed, distance) - 3;

            transform.position = newPos;
        }

        if (gameObject.CompareTag("MovingWallRight"))
        {
            newPos = startPos;
            newPos.x = newPos.x - Mathf.PingPong(Time.time * speed, distance) + 3;

            transform.position = newPos;
        }

        if (gameObject.CompareTag("MovingWallUp"))
        {
            newPos = startPos;
            newPos.y = newPos.y + Mathf.PingPong(Time.time * speed, distance) - 3;

            transform.position = newPos;
        }

    }



}
