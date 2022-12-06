using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public AudioSource audiosource;
    public AudioClip WalkingSound;

    public float sfxDelay;
    public Vector3 prevPos;
    public float deltaPos;
    private bool sfxStopped;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        audiosource.enabled = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        StartCoroutine(sfxStepDelay());
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;

        if ((horizontalInput != 0 || verticalInput != 0))
        {
            if (sfxStopped == true)
                audiosource.enabled = true;
            sfxStopped = false;
        }
        else
            sfxStopped = true;

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude >moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }


    private IEnumerator sfxStepDelay()
    {
        prevPos = transform.position; 
        if (horizontalInput == 0 && verticalInput == 0)
        {
            audiosource.enabled = false;
            bool sfxStopped = true;  
        }
        else if (deltaPos > 1f)
        {
            audiosource.enabled = false;
            audiosource.enabled = true;
        }

        yield return new WaitForSeconds(sfxDelay);
        deltaPos = Vector3.Distance(prevPos, transform.position);
        
        Debug.Log(deltaPos);
        StartCoroutine(sfxStepDelay());
    }
}
