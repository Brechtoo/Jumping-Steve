using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;

public class RollingBall : MonoBehaviour
{
    public float speed = 5f; // Geschwindigkeit der Kugel
    private Rigidbody rb; // Referenz auf die Rigidbody-Komponente
    public GameOverScreen gameOverScreen;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Zugriff auf die Rigidbody-Komponente
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(0, 0, -1); // Bestimme die Richtung (x, y, z)
        rb.AddForce(direction * speed); // Bewegt die Kugel in die angegebene Richtung
    }
}
