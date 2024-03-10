using System.Collections;

using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class Player2 : MonoBehaviour
{
    [Header("Movement")]
    public float _moveSpeed = 40f;
    public float _gravity = 3.5f;
    public float _jumpSpeed = 1.5f;

    [Header("Dash")]
    public float dashDistance = 45f;
    public float dashDuration = 0.8f;
    public float acceleration = 200f;
    private bool isDashing = false;


    [Header("Direction")]
    private Vector3 direction;
    private Vector3 moveDir = new(0, 0, 0);
    private float _directionY;


    [Header("DoubleJump")]
    public float doubleJumpMultiplyer = 0.9f;
    private bool canDoubleJump = false;
  


    [Header("Wallrunning")]


    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;


    [Header("References")]
    public Transform orientation;
    private CharacterController characterController;



    [Header("Cam")]
    public float turnSmoothTime = 0.06f;
    float turnSmoothVelocity;
    public Transform cam;

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource dyingSound;
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource musicSound;

    [Header("Screens")]
    public GameOverScreen gameOverScreen;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        Dying();
        HandleInput();
        HandleJump();

        moveDir.y = _directionY;

        if (_directionY > -2f)
        {
            _directionY -= _gravity * Time.deltaTime;
        }
        HandleCamera();
        HandleDash();
        characterController.Move(_moveSpeed * Time.deltaTime * moveDir);

    }

    public void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        direction = new Vector3(horizontalInput, 0, verticalInput);
        moveDir = new Vector3(horizontalInput, 0, verticalInput);

    }





    public void HandleCamera()
    {
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.y = _directionY;
        }
    }

    public void HandleDash()
    {
        //Dash Bedingung
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {

            // Starten des Dashs
            StartCoroutine(PerformDash(moveDir));
        }
    }






    IEnumerator PerformDash(Vector3 dir)
    {
        if (jumpSound.isPlaying)
        {
            jumpSound.Pause();
        }
        dashSound.Play();

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
            characterController.Move(currentSpeed * Time.deltaTime * dir);

            yield return null; // Warten auf den nächsten Frame
        }

        isDashing = false; // Setzen des Zustands auf "Nicht im Dash" nach Abschluss des Dashs

    }

    public void Dying()
    {
        if (transform.position.y < -13f && !dead)
        {
            dead = true;
            GameOverScreen();
        }
    }

    public void GameOverScreen()
    {
        musicSound.Pause();
        dyingSound.Play();
        gameOverScreen.Setup();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }






    public void HandleJump()
    {
       
        if (characterController.isGrounded)
        {
           
            canDoubleJump = true;

            if (Input.GetButtonDown("Jump") && !isDashing)
            {
                
                if (!dashSound.isPlaying)
                {
                    jumpSound.Play();

                }
                _directionY = _jumpSpeed;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && canDoubleJump && !isDashing)
            {
                if (!dashSound.isPlaying)
                {
                    jumpSound.Play();
                }
               
                _directionY = _jumpSpeed * doubleJumpMultiplyer;
                canDoubleJump = false;
            }

        }
    }



}

