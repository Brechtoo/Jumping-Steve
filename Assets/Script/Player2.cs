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
    private float doubleJumpMultiplyer = 0.5f;

    private float _directionY;

    private bool canDoubleJump = false;


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
        Vector3 moveDir = new Vector3(horizontalInput, 0, verticalInput);

        JumpMecanics();
        
        _directionY -= _gravity * Time.deltaTime;
        moveDir.y = _directionY;


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.y = _directionY;
        }


        _characterController.Move(moveDir * _moveSpeed * Time.deltaTime);


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

