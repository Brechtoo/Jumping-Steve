using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{

    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private float _gravity = 9.81f;

    [SerializeField]
    private float _jumpSpeed = 3.5f;

    [SerializeField]
    private float dashDistance = 5f; // Die Entfernung, die der Spieler im Dash zurücklegt

    [SerializeField]
    private float dashDuration = 0.2f; // Die Dauer des Dashs in Sekunden

    [SerializeField]
    private float acceleration = 1.5f;

    private bool isDashing = false; // Variable, um den aktuellen Zustand des Dashs zu verfolgen

    [SerializeField]
    private Vector3 moveDir = new Vector3 (0,0,0);


    [SerializeField]
    private float doubleJumpMultiplyer = 0.5f;

    [SerializeField]
    private float _directionY;

    private bool canDoubleJump = false;

    private bool wallrunning = false;

    [SerializeField]
    private float wallrunSpeed = 400f;

    float wallGravity = 0f;
    float walli = 0.3f;


    private CharacterController _characterController;
    public Transform cam;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        moveDir = new Vector3(horizontalInput, 0, verticalInput);

        JumpMecanics();
        

        if(_directionY > -3f)
        {
            _directionY -= _gravity * Time.deltaTime;
        }
  
        moveDir.y = _directionY;

        //kamera winkel berechnung
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.y = _directionY;
        }

        //Dash Bedingung
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {

            // Starten des Dashs
            StartCoroutine(PerformDash(moveDir));
        }

        if (!wallrunning)
        {
        _characterController.Move(moveDir * _moveSpeed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(PerformWallrun(moveDir));
        }
        

        }






    IEnumerator PerformDash(Vector3 dir)
    {

        dir.y = 0f; //Dash geht nicht in die Höhe

        isDashing = true; // Setzen des Zustands auf "Im Dash"

        float startTime = Time.time; // Zeitpunkt, zu dem der Dash begann

        

        while (Time.time - startTime < dashDuration) // Dash-Dauer überprüfen
        {
            // Berechnen des Fortschritts des Dashs (0 bis 1)
            float progress = (Time.time - startTime) / dashDuration;

            // Beschleunigen des Dashs zu Beginn
            float currentSpeed = Mathf.Lerp(0, dashDistance / dashDuration, progress * acceleration);

            // Verlangsamen des Dashs zum Ende hin
            if (progress > 0.2f) 
            {
                // Exponentielle Verzögerung
                float deceleration = Mathf.Pow(1 - (progress - 0.2f) * 2, 2);
                currentSpeed *= deceleration;
            }

            // Bewegen des Charakter-Controllers in Richtung des Dashs
            _characterController.Move(dir * currentSpeed * Time.deltaTime);

            yield return null; // Warten auf den nächsten Frame
        }

        isDashing = false; // Setzen des Zustands auf "Nicht im Dash" nach Abschluss des Dashs
    }

    private ControllerColliderHit _wallCollision; // Variable, um die Kollision mit der Wand zu speichern

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        // Überprüfen, ob der kollidierte Collider den Layer "Wallrun" hat
        if (hit.collider.CompareTag("Wallrun"))
        {
            _wallCollision = hit;
            wallrunning = true;
            isDashing = true;
            // Rufen Sie hier die PerformWallrun-Methode auf
        }
    }


    IEnumerator PerformWallrun(Vector3 vec)
    {
        
            canDoubleJump = true;
            _characterController.Move(vec *  _moveSpeed * Time.deltaTime);

        
        yield return null;

        wallrunning = false;
        isDashing = false;
    }






    public void JumpMecanics()
    {
        if (_characterController.isGrounded)
        {
            canDoubleJump = true;

            if (Input.GetButtonDown("Jump"))
            {
                _directionY = _jumpSpeed;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && canDoubleJump)
            {
                _directionY = _jumpSpeed * doubleJumpMultiplyer;
                canDoubleJump = false;
            }

        }
    }


    
    }

