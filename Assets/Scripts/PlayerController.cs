using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
{


    /************************************************************
     * 
     * GLOBAL VARS
     * 
     ************************************************************/
    #region Variables
    public CharacterController controller;
    public Animator anim;
    public GameObject playerModel;
    public Transform pivot;
    public GameObject vent;
    public bool vent_door; //false = vent, true = door

    public TextMeshProUGUI countText;
    public int count;
    public AudioClip collectsound;

    [Header("Movement")]
    public float maxSpeed;
    public float currSpeed;
    [Range(0, 30f)] public float runSpeed;
    [Range(0, 30f)] public float sprintSpeed;
    [Range(0, 30f)] public float dragSpeed;
    [Range(0, 3f)] public float timeZeroToMax;
    [Range(0, 3f)] public float timeMaxToZero;
    [Range(0, 15f)] public float rotateSpeed;
    public bool isSprinting;
    public bool isDragging;
    private float accelRatePerSec;
    private float decelRatePerSec;

    [Header("Jumping")]
    public float currentJumpForce;
    [Range(0, 30f)] public float jumpForce;
    [Range(0, 30f)] public float draggingJumpForce;
    [Range(0, 1f)] public float jumpWindowTime;
    [Range(0, 50f)] public float fallSpeed;
    [Range(0, 50f)] public float groundedFallSpeed;
    [Range(0, 50f)] public float ledgeFallSpeed;
    [Range(0, 50f)] public float quickFallMultiplier;
    private bool quickFall;
    private bool wasGrounded;
    private bool headBump;

    [Header("Acceleration Tilt")]
    [Range(0, 10f)] public float tiltAngle;
    [Range(0, 10f)] public float tiltMax;
    [Range(-1, 1f)] public float upVectorMultiplier;
    private float tiltAmount;

    private Vector3 moveDirection;
    private Vector3 lastMoveDirection;
    private float airSpeed;
    private float jumpWindowCounter;
//    private bool controllerConnected;

    private Vector3 velocityCurr;
    private Vector3 velocityPrev;
    private Vector3 acceleration;

    public Transform squash_stretch_controller;
    public GameObject cage;
    #endregion


    /************************************************************
     * 
     * START
     * 
     ************************************************************/
    #region Initialize
    void Start()
    {

        controller = GetComponent<CharacterController>();
        maxSpeed = runSpeed;
        currentJumpForce = jumpForce;
        SetupAcceleration();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        count = 0;
        SetCountText();
        //DetectControllers();
    }



    /************************************************************
     * SET UP ACCELERATION
     ************************************************************/
    void SetupAcceleration()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
        currSpeed = 0;
    }



    /************************************************************
     * DETECT CONTROLLERS
     ************************************************************/
/*    void DetectControllers()
    {
        string[] controllers = Input.GetJoystickNames();
        for (int controller = 0; controller < controllers.Length; controller++)
        {
            if (!controllers[controller].Equals(""))
            {
                controllerConnected = true;
            }
        }
    }
*/
    #endregion

    /************************************************************
     * 
     * UPDATE
     * 
     ************************************************************/
    void Update()
    {

        //Calculate velocity and acceleration
        UpdatePhysics();

        //Calculate Movement
        CalculateMovement("Vertical", "Horizontal", "Jump", "Sprint", "Drag");

        //Move Character
        MoveCharacter("Vertical", "Horizontal");

        //Handle Animations
        AnimateCharacter("Vertical", "Horizontal");
    }


    #region Calculate Lateral Movement
    /************************************************************
     * UPDATE PHYSICS
     ************************************************************/
    void UpdatePhysics ()
    {
        velocityPrev = velocityCurr;                    //Set previous velocity to current outdated one,
        velocityCurr = controller.velocity;             //update the current velocity, and
        acceleration = velocityCurr - velocityPrev;     //calculate acceleration based on those.
    }



    /************************************************************
     * CALCULATE MOVEMENT
     * 
     * yStore:          This float stores the character's current y-axis movement direction.
     * inputName:       Controls input relative to that functionality.
     ************************************************************/
    void CalculateMovement(string inputVertical, string inputHorizontal, string inputJump, string inputSprint, string inputDrag)
    {
        //Store y velocity to function apart from input
        float yStore = moveDirection.y;

        //Do physics on character and apply that to their position
        AcceleratePlayer(yStore, inputVertical, inputHorizontal);
        UpdateMovement(yStore, inputVertical, inputHorizontal);

        //Handle jumping and falling
        if (controller.isGrounded)
        {
            ResetJump();
            Jump(inputJump);
        }
        else
        {
            Fall(inputJump);
            CalculateJumpWindow(inputJump);
        }

        //Handle sprinting and dragging
        CheckSprint(inputSprint, inputVertical, inputHorizontal);
        CheckDrag(inputDrag);
    }



    /************************************************************
     * ACCELERATE OR DECELERATE PLAYER BASED ON INPUT
     ************************************************************/
    void AcceleratePlayer(float yStore, string inputVertical, string inputHorizontal)
    {
        //decelerate currSpeed due to physics
        float decelSpeed = currSpeed;                       //Set decelSpeed to currentSpeed
        decelSpeed += decelRatePerSec * Time.deltaTime;     //Decrease decelSpeed due to deceleration rate
        decelSpeed = Mathf.Max(decelSpeed, 0);              //Set decelspeed to the max between itself and 0, preventing positive deceleration.

        //accelerate currSpeed due to physics and input
        float accelSpeed = currSpeed;                                                //Set accelSpeed to currentSpeed
        float maxPercent = new Vector2(Input.GetAxis(inputVertical),             //Get the percentage of maximum input from the player
                                        Input.GetAxis(inputHorizontal)).magnitude;
        if (maxPercent > 1)
        {
            maxPercent = 1;
        }
        accelSpeed += accelRatePerSec * Time.deltaTime;                              //Increate accelSpeed due to acceleration rate
        accelSpeed = Mathf.Min(accelSpeed, maxSpeed * maxPercent);                   //Set accelSpin to min between itself and the desired percentage of max Speed.

        //set current speed to the greater of the two
        //  Deceleration greater = player input has decreased, so we decelerate to that speed.
        //  Acceleration greater = player input is greater than or equal to current speed.
        currSpeed = Mathf.Max(decelSpeed, accelSpeed);
    }



    /************************************************************
     * UPDATE MOVEMENT BASED ON ACCELERATION
     ************************************************************/
    void UpdateMovement(float yStore, string inputVertical, string inputHorizontal)
    {

        Vector3 forwardHolder = new Vector3(pivot.forward.x, 0.0f, pivot.forward.z).normalized;
        //Set direction of movement equal to some ratio of horizontal and vertical input
        moveDirection = (forwardHolder * Input.GetAxis(inputVertical))
                        + (pivot.right * Input.GetAxis(inputHorizontal));
        //If no input at all, keep movement toward last movement direction.
        if ((moveDirection.x == 0 && moveDirection.z == 0)) {
            moveDirection = lastMoveDirection;
        }
        //Normalize the move direction and multiply it by speed variable, then plug the y Speed back into moveDirection.
        moveDirection = moveDirection.normalized * currSpeed;
        moveDirection.y = yStore;
    }
    #endregion

    #region Jumping/Falling
    /************************************************************
     * RESET JUMP MATH
     ************************************************************/
    void ResetJump()
    {
        if (!wasGrounded)
        {
            squash_stretch_controller.GetComponent<Animator>().SetTrigger("squash");
            wasGrounded = true;
        }
        airSpeed = 1.2f;
        jumpWindowCounter = jumpWindowTime;
        moveDirection.y = -groundedFallSpeed;       //huge number here to keep grounded
        quickFall = false;
        headBump = false;
    }



    /************************************************************
     * JUMP
     ************************************************************/
    void Jump(string inputJump)
    {
        if (Input.GetButtonDown(inputJump))
        {
            moveDirection.y = currentJumpForce;
            jumpWindowCounter = 0;
            squash_stretch_controller.GetComponent<Animator>().SetTrigger("stretch");
        }
    }



    /************************************************************
     * FALL
     ************************************************************/
    void Fall(string  inputJump)
    {
        if (!Input.GetButton(inputJump))
        {
            quickFall = true;
        }
        if (quickFall)
        {
            moveDirection.y += (Physics.gravity.y * fallSpeed * quickFallMultiplier * Time.deltaTime);
        } else
        {
            moveDirection.y += (Physics.gravity.y * fallSpeed * Time.deltaTime);
        }

        /*
        if (velocityCurr.y <= 0f && !headBump && !wasGrounded)
        {
            headBump = true;
        }

        if (headBump)
        {
            moveDirection.y = -10f;
        }
        */

        if (wasGrounded && moveDirection.y < 0)
        {
            moveDirection.y = ledgeFallSpeed;
        }

        if (airSpeed > 0.8)
        {
            airSpeed -= 1 * Time.deltaTime;
        }

        wasGrounded = false;
    }



    /************************************************************
     * CALCULATE JUMP WINDOW
     ************************************************************/
    void CalculateJumpWindow(string inputJump)
    {
        jumpWindowCounter -= Time.deltaTime;
        if (jumpWindowCounter > 0 && Input.GetButtonDown(inputJump))
        {
            quickFall = false;
            moveDirection.y = currentJumpForce;
            jumpWindowCounter = 0;
        }
    }
    #endregion


    /************************************************************
     * CHECK SPRINT
     ************************************************************/
    void CheckSprint(string inputSprint, string inputVertical, string  inputHorizontal)
    {
        if (Input.GetButtonDown(inputSprint))
        {
            if (!isSprinting && !isDragging)
            {
                EnterSprint();
                return;
            } else if (isSprinting)
            {
                ExitSprint();
                return;
            }
        }
        if (Mathf.Abs(Input.GetAxis(inputVertical)) < 0.3 && Mathf.Abs(Input.GetAxis(inputHorizontal)) < 0.3 && isSprinting)
        {
            ExitSprint();
        }
    }

    //Called to enter a sprint
    void EnterSprint()
    {
        isSprinting = true;

        currSpeed += 0.1f;
        maxSpeed = sprintSpeed;
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }

    //Called to end a sprint
    void ExitSprint()
    {
        isSprinting = false;
        maxSpeed = runSpeed;
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }


    /************************************************************
     * CHECK DRAG
     ************************************************************/
    void CheckDrag(string inputDrag)
    {
        if (Input.GetButtonDown(inputDrag) && !isDragging)
        {
            EnterDrag();
            return;
        }

        if (Input.GetButtonUp(inputDrag) &&  isDragging)
        {

            ExitDrag();
            return;
        }
    }

    //Called to start dragging
    void EnterDrag()
    {
        isDragging = true;
        maxSpeed = dragSpeed;
        currentJumpForce = draggingJumpForce;
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }

    //Called to stop dragging
    void ExitDrag()
    {
        isDragging = false;
        maxSpeed = runSpeed;
        currentJumpForce = jumpForce;
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }


    /************************************************************
     * MOVE THE CHARACTER
     ************************************************************/
    void MoveCharacter(string inputHorizontal, string inputVertical)
    {

        //Move
        controller.Move(moveDirection * Time.deltaTime);

        //Update last moved direction for later
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            lastMoveDirection.x = moveDirection.normalized.x;
            lastMoveDirection.z = moveDirection.normalized.z;
        }

        //Set tiltAmount based on magnitude of acceleration times xz velocity magnitude times tiltAngle
        tiltAmount = acceleration.magnitude * new Vector2(controller.velocity.x, controller.velocity.z).magnitude * tiltAngle * (new Vector2(Input.GetAxis(inputHorizontal), Input.GetAxis(inputVertical))).magnitude;

        //If grounded, invert tiltAmount. If airborne, multiply by normalized moveDirection's y component and a porportion of max speed.
        if (controller.isGrounded) { tiltAmount *= -1; }
        else { tiltAmount *= -moveDirection.normalized.y; }
        //Clamp tiltAmount between minimum and maximum possible tilt.

        float currTiltMax = tiltMax * controller.velocity.magnitude / 15.0f;

        tiltAmount = Mathf.Clamp(tiltAmount, -currTiltMax, currTiltMax);

        float xTiltAmount = acceleration.x * upVectorMultiplier;
        float zTiltAmount = acceleration.z * upVectorMultiplier;

        if (xTiltAmount < 0)
        {
            xTiltAmount = -1 * Mathf.Sqrt(-1 * xTiltAmount);
        } else
        {
            xTiltAmount = Mathf.Sqrt(xTiltAmount);
        }

        if (zTiltAmount < 0)
        {
            zTiltAmount = -1 * Mathf.Sqrt(-1 * zTiltAmount);
        }
        else
        {
            zTiltAmount = Mathf.Sqrt(zTiltAmount);
        }

        xTiltAmount = Mathf.Clamp(xTiltAmount, -1, 1f);
        zTiltAmount = Mathf.Clamp(zTiltAmount, -1, 1f);

        Vector3 upVector = new Vector3(xTiltAmount, 1, zTiltAmount);

        Quaternion newRotation;
        //If moving, rotate character relative to movement
        if (Input.GetAxis(inputHorizontal) != 0 || Input.GetAxis(inputVertical) != 0)
        {
            newRotation = Quaternion.LookRotation(new Vector3(controller.velocity.x, tiltAmount, controller.velocity.z), upVector);
        } else
        {
            newRotation = Quaternion.LookRotation(new Vector3(lastMoveDirection.x, tiltAmount, lastMoveDirection.z), upVector);
        }

            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 0.001f + controller.velocity.magnitude * Time.deltaTime / 2f);
    }


    /************************************************************
     * ANIMATE THE CHARACTER
     ************************************************************/
    void AnimateCharacter(string inputHorizontal, string inputVertical)
    {
        anim.SetBool("isSprinting", isSprinting);
        anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetFloat("Vertical Velocity", controller.velocity.y);
        anim.SetFloat("Speed", controller.velocity.magnitude);
        anim.SetBool("isDragging", isDragging);
    }

    /************************************************************
     * Collect Collectables
     ************************************************************/
    void SetCountText()
    {

        // Run the 'SetCountText()' function (see below)
        if (!vent_door) countText.text = "Count: " + count.ToString() + "/" + vent.GetComponent<ventHandler>().indicator_count;
        else
        {
            countText.text = "Count: " + count.ToString() + "/" + vent.GetComponent<doorHandler>().indicator_count;
            if (count == vent.GetComponent<doorHandler>().indicator_count)
            {
                cage.SetActive(false);
            }
        }

        
    }

    void OnTriggerEnter(Collider other)
    {
        // ..and if the GameObject you intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("PickUp"))
        {
            AudioSource.PlayClipAtPoint(collectsound, transform.position);

            other.gameObject.SetActive(false);

            // Add one to the score variable 'count'
            count = count + 1;

            SetCountText();

        }
    }
 
}