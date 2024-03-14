using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBall : MonoBehaviour
{
    public float speed = 10f; 
    private Rigidbody rb;
    public GameOverScreen gameOverScreen;
    public bool isRolling = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    void FixedUpdate()
    {
        if (isRolling)
        {
            rb.isKinematic = false;
            Vector3 direction = new Vector3(0, 0, -1); 
            rb.AddForce(direction * speed);
        }
    }
}
