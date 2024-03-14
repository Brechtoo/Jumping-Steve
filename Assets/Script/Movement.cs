using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
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
    public Animator animator;
    public Transform animationPos;
    public  ParticleSystem particleDash;
    public  ParticleSystem particleSystemDoubleJump;



    [Header("Cam")]
    public float turnSmoothTime = 0.06f;
    float turnSmoothVelocity;
    public Transform cam;

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource dyingSound;
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource musicSound;
    [SerializeField] private AudioSource finishSound;
    [SerializeField] private AudioSource trampolinSound;

    [Header("Screens")]
    public GameOverScreen gameOverScreen;
    public PauseScreen pauseScreen;
    private bool dead = false;
    public Tut tut;
    public DashTut dashTut;

    public LevelCompleted levelCompleted;

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

        Paused();
    }

    public void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if(horizontalInput != 0  || verticalInput != 0 && !animator.GetBool("isJumping"))
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        direction = new Vector3(horizontalInput, 0, verticalInput);
        moveDir = new Vector3(horizontalInput, 0, verticalInput);

    }


    private void OnCollisionEnter(Collision collision)
    {
        // Überprüft, ob das kollidierende Objekt den richtigen Tag hat
        if (collision.gameObject.CompareTag("GameOverTag"))
        {
            GameOverScreen();
        }
    }

    

    private void OnTriggerEnter(Collider trigger)
    {
        if(trigger.CompareTag("TutTrigger"))
        {
            if(tut != null)
               tut.Setup();
        }

        if (trigger.CompareTag("CloseTutTrigger"))
        {
            if (tut != null)
                tut.Close();
        }

        if (trigger.CompareTag("dashTut"))
        {
            if(dashTut != null)
            dashTut.Setup();
        }

        if (trigger.CompareTag("CloseDashTut"))
        {
            if (dashTut != null)
                dashTut.Close();
        }

        if (trigger.CompareTag("GoalTrigger"))
        {
            finishSound.Play();
            levelCompleted.Setup();
        }
        if (trigger.CompareTag("TrampolinTrigger"))
        {
            _directionY = 1.5f * _jumpSpeed;
            if(!trampolinSound.isPlaying)
            {
                trampolinSound.Play();
            }
        }
    }

    public void FixedUpdate()
    {
        AnimationFix();
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

            if (animator.GetBool("isDoubleJumping"))
            {
                ParticleDash(particleSystemDoubleJump);
            }
            else
            {
                ParticleDash(particleDash);
            }
            StartCoroutine(PerformDash(moveDir));

           
        }
    }
    public void ParticleDash(ParticleSystem p)
    {
        if (p != null)
        {
            p.gameObject.SetActive(true);
            p.Play();
        }
        
    }
  

        public void AnimationFix()
        {
            if (characterController.isGrounded && !animator.GetBool("isDoubleJumping"))
            {
            animationPos.localRotation = Quaternion.identity;
        }
        }





    IEnumerator PerformDash(Vector3 dir)
    {
        if (jumpSound.isPlaying)
        {
            jumpSound.Pause();
        }
        if(dir.x !=0 && dir.z != 0)
        {
            dashSound.Play();
        }

        dir.y = 0f; //Dash geht nicht in die Höhe

        isDashing = true; // Setzen des Zustands auf "Im Dash"

        float startTime = Time.time; // Zeitpunkt, zu dem der Dash begann

        animator.SetBool("isDashing", true);

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

            
            characterController.Move(currentSpeed * Time.deltaTime * dir);

            yield return null; // Warten auf den nächsten Frame
        }

        isDashing = false; // Setzen des Zustands auf "Nicht im Dash" nach Abschluss des Dashs
        animator.SetBool("isDashing",false);
        particleDash.gameObject.SetActive(false);
        particleSystemDoubleJump.gameObject.SetActive(false);


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

            animator.SetBool("isJumping", false);
            
            animator.SetBool("isDoubleJumping", false);
            


            if (Input.GetButtonDown("Jump") && !isDashing)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", true);
                jumpSound.Play();
                _directionY = _jumpSpeed;
            }

        }
        else
        {
            if (Input.GetButtonDown("Jump") && canDoubleJump && !isDashing)
            {
                animator.SetBool("isDoubleJumping", true);
                jumpSound.Play();
                _directionY = _jumpSpeed * doubleJumpMultiplyer;
                canDoubleJump = false;
                

            }

            

        }

      
    }

    public void Paused()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            pauseScreen.PauseGame();
    }
    
}