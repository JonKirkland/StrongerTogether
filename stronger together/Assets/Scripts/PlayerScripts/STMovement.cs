

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.Security.Cryptography;

public class STMovement : MonoBehaviour

{

    //Assingables
    public Transform shadowDashPosition;
    public Transform playerCam;
    public Transform orientation;
    public Camera cam;
    public CapsuleCollider bean;
    private float beanHeight;
    //Other
    private Rigidbody rb;
/*
    //SFX
    public AudioSource[] sound;
    public AudioSource footstep;
    public AudioSource jump;
    public AudioSource dash;
*/
    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public float crouchSpeed = 7;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale;
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 1f;

    //Jumping
    private bool readyToJump = true;
    private bool jumpTimer = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    //Input
    float x, y;
    bool jumping, sprinting, crouching, leftDash, rightDash, newJump;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    //Wallrunning
    public float wallRunDownVel = -0.1f;
    public LayerMask wallRunnable;
    public float wallRunForce, maxWallTime, maxWallSpeed, wallGravity;
    bool isWallRunRight, isWallRunLeft;
    bool isWallRunning;
    public float maxWallRunCameraTilt, wallRunCameraTilt;

    //Dashing
    public float dashForce;
    public float dashDuration;
    bool isDashLeft, isDashRight;
    public float maxDashCameraTilt, dashCameraTilt;
    int dashCount = 0;

    private bool shadowDashCooldown = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    void Start()
    {
        
        //sound = GetComponents<AudioSource>();
        //footstep = sound[0];
        //jump = sound[1];
        //dash = sound[2];
        

        playerScale = transform.localScale;
        crouchScale = new Vector3(playerScale.x, playerScale.y * 0.5f, playerScale.z);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        beanHeight = bean.height;
        UnityEngine.Debug.Log(beanHeight);
    }


    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        ResetDash();
        MyInput();
        Look();
        CheckWall();
        WallRunInput();
        Dash();
        
        //PlayFootsteps();
        MenuChecker();  
    }
    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        newJump = Input.GetButtonUp("Jump");
        leftDash = Input.GetKey(KeyCode.Q);
        rightDash = Input.GetKey(KeyCode.E);

        crouching = Input.GetKey(KeyCode.LeftControl);

        if (jumpTimer && newJump)
        {
            readyToJump = true;
        }


        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }
    private void MenuChecker()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void StartCrouch()
    {
        transform.localScale = crouchScale;
        if (grounded == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (beanHeight * 0.125f), transform.position.z);
        }
            
        /*
        if (rb.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
        */
    }

    private void StopCrouch()
    {
        transform.localScale = playerScale;
        if (grounded == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + beanHeight * 0.125f, transform.position.z);
        }
        
    }

    private void Movement()
    {
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed
        float maxSpeed = this.maxSpeed;
        float crouchSpeed = this.crouchSpeed;

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        /*
        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }
        */
        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (crouching)
        {
            if (xMag > crouchSpeed) x = 0;
            if (x < 0 && xMag < -crouchSpeed) x = 0;
            if (y > 0 && yMag > crouchSpeed) y = 0;
            if (y < 0 && yMag < -crouchSpeed) y = 0;
        }
        else
        {
            if (xMag > maxSpeed) x = 0;
            if (x < 0 && xMag < -maxSpeed) x = 0;
            if (y > 0 && yMag > maxSpeed) y = 0;
            if (y < 0 && yMag < -maxSpeed) y = 0;
        }


       
        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        // Movement while sliding
        if (grounded && crouching) multiplierV = 0.7f;
        if (grounded && crouching) multiplier = 0.7f;

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;
            //play jump SFX
            //jump.Play();
            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        else if (Physics.Raycast(transform.position,-orientation.right,2))
        {
            //jump.Play(); //jump sfx for walls
            readyToJump = false;
            rb.AddForce(orientation.right * jumpForce * 1.4f);

            //add some forward force for da boyz
            //rb.AddForce(orientation.forward * jumpForce * 0.2f);
            rb.AddForce(orientation.up * jumpForce * 2.2f);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        else if(Physics.Raycast(transform.position, orientation.right, 2))
        {
            readyToJump = false;
            rb.AddForce(-orientation.right * jumpForce * 1.4f);
            rb.AddForce(orientation.up * jumpForce * 2.2f);
            Invoke(nameof(ResetJump), jumpCooldown);
            //might want to make how far you jump off the wall dependent on speed o.o
        }
    }

    private void ResetJump()
    {
        jumpTimer = true;
    }

    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

        //Perform the rotations of wall run
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, wallRunCameraTilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
        /*
        //Perform the rotations of dash
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, dashCameraTilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

        */
        //While Wallrunning
        //Tilts camera in .5 second
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallRunRight && !grounded)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 4;
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallRunLeft && !grounded)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 4;

        //Tilts camera back again
        if (wallRunCameraTilt > 0 && !isWallRunRight && !isWallRunLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 4;
        if (wallRunCameraTilt < 0 && !isWallRunRight && !isWallRunLeft)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 4;
        if (wallRunCameraTilt > 0 && grounded)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 4;
        if (wallRunCameraTilt < 0 && grounded)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 4;

        //DASH camera tilts, left dash is jagged for no reason, maybe its the a problem with the above section
        if (Math.Abs(wallRunCameraTilt) < maxDashCameraTilt && isDashRight)
            wallRunCameraTilt -= Time.deltaTime * maxDashCameraTilt * 1;
        if (Math.Abs(wallRunCameraTilt) < maxDashCameraTilt && isDashLeft)
            wallRunCameraTilt += Time.deltaTime * maxDashCameraTilt * 1;

        //Tilts camera back again
        if (wallRunCameraTilt > 0 && !isDashRight && !isDashLeft)
            wallRunCameraTilt -= Time.deltaTime * maxDashCameraTilt * 4;
        if (wallRunCameraTilt < 0 && !isDashRight && !isDashLeft)
            wallRunCameraTilt += Time.deltaTime * maxDashCameraTilt * 4;



    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping || leftDash || rightDash) return;
        //doesnt only apply for dash duration but its fine for now, could be a "mechanic"

        //Slow down sliding
        /*
        if (crouching)
        {
            
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }
        */
        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    /// <summary>
    /// Handle ground detection
    /// </summary>
    public void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    public void StopGrounded()
    {
        grounded = false;
    }
    //wall run functions
    private void WallRunInput()
    {
        //start wallrun
        if (Input.GetKey(KeyCode.D) && isWallRunRight)
        {
            WallRunStart();
        }
        if (Input.GetKey(KeyCode.A) && isWallRunLeft)
        {
            WallRunStart();
        }
    }
    private void WallRunStart()
    {
        rb.useGravity = false;
        isWallRunning = true;

        rb.AddForce(orientation.forward * wallRunForce / 2 * Time.deltaTime);
        //Add a lil gravity
        //rb.AddForce(Vector3.down * Time.deltaTime * wallGravity);
        rb.velocity = rb.velocity + new Vector3(0, wallRunDownVel, 0);
        rb.AddRelativeForce(Vector3.down * wallGravity * Time.deltaTime);
        //make sure char sticks to wall
        if (isWallRunRight)
        {
            rb.AddForce(orientation.right * wallRunForce / 10 * Time.deltaTime);
            //depends on wallrun forward force so might have to change
        }
        else // wall run left
        {
            rb.AddForce(-orientation.right * wallRunForce / 10 * Time.deltaTime);
        }

    }
    private void WallRunStop()
    {
        rb.useGravity = true;
        isWallRunning = false;
    }
    private void CheckWall()
    {
        isWallRunRight = Physics.Raycast(transform.position, orientation.right, 1f, wallRunnable);
        isWallRunLeft = Physics.Raycast(transform.position, -orientation.right, 1f, wallRunnable);

        //stop wallrun
        if (!isWallRunRight && !isWallRunLeft)
        {
            WallRunStop();
        }
    }
    /* new dash: teleports player 1 meter in front of himself
     */
    private void Dash() 

    {
        if (Input.GetKeyDown(KeyCode.E) && dashCount < 3)
        {
            //dash.Play();
            StartCoroutine(RightDash());
            //isDashRight = true;

        }
        if (Input.GetKeyDown(KeyCode.Q) && dashCount < 3)
        {
            //dash.Play();
            StartCoroutine(LeftDash());
            //isDashLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && shadowDashCooldown == false)
        {
            StartCoroutine(ShadowDash());
        }
        
        

    }
    private IEnumerator ShadowDash()
    {
        //yield return new WaitForSeconds(0.2f);

        Vector3 castDirection = transform.position - shadowDashPosition.position;

        RaycastHit hit;

        if (Physics.Raycast(transform.position,-castDirection, out hit,castDirection.magnitude))
        {
            transform.position = (hit.point);
        }
        else
        {
            transform.position = shadowDashPosition.position;
        }
        



        shadowDashCooldown = true;
        yield return new WaitForSeconds(1);
        shadowDashCooldown = false;


    }
    private IEnumerator RightDash()
    {

        rb.AddForce(orientation.right * dashForce, ForceMode.VelocityChange);
        isDashRight = true;
        dashCount = dashCount + 1;
        yield return new WaitForSeconds(dashDuration);
        rb.AddForce(-orientation.right * dashForce, ForceMode.VelocityChange);
        isDashRight = false;




    }
    private IEnumerator LeftDash()
    {

        rb.AddForce(-orientation.right * dashForce, ForceMode.Impulse);
        isDashLeft = true;
        dashCount = dashCount + 1;
        yield return new WaitForSeconds(dashDuration);
        rb.AddForce(orientation.right * dashForce, ForceMode.Impulse);
        isDashLeft = false;
    }
    private void ResetDash()
    {
        if (grounded)
        {
            dashCount = 1;
        }
        
    }
    private void LedgeGrab()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))

        {
            print("cash");
            if (hit.collider.tag == "Ledge")
            {
                print("chiasjdo;asd");
                rb.AddForce(transform.up * 250f);
                rb.AddForce(transform.up * 250f);
            }
        }
    }

   /* private void PlayFootsteps()
    {
        if (grounded == true && rb.velocity.magnitude > 10f && footstep.isPlaying == false)
        {
            footstep.Play();
        }
    }
   */
}
