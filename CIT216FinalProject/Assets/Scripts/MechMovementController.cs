//4/29/26
//Herman Pagan Alvarez
//Handles the mech's movement, animation, and crosshair
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class MechMovementController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Transform tf;
    public Transform playerCamera;
    private Rigidbody rb;
    public GameObject mech_legs;
    public GameObject mech_upper;
    public GameObject cameraPivot;
    private PlayerInput playerInput;
    private PlayerController playerController;

    public float damping;
    public float boostDamping;
    public float rotateSpeed = 10f;
    public float acceleration = 1000f;
    public float jumpHeight = 5f;
    public float xMove;
    public float zMove;
    public Vector2 lookVector;
    private bool onGround;
    public float gravity;
    private bool isShooting;
    public bool boostActive = false;

    public float lookSensitivity = 10f;
    public float minPitch = -40f;
    public float maxPitch = 80f;
    float normalAcceleration;
    float normalRotateSpeed;
    float normalJumpHeight;
    float boostAcceleration;
    float boostRotateSpeed;
    float boostjumpHeight;


    public GameObject crosshair;


    private float yaw;
    private float pitch;



    void Start()
    {
        isShooting = false;
        tf = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        playerController = gameObject.GetComponent<PlayerController>();
        normalAcceleration = acceleration;
        normalRotateSpeed = rotateSpeed;
        normalJumpHeight = jumpHeight;
        boostAcceleration = acceleration * 5;
        boostRotateSpeed = rotateSpeed * 2;
        boostjumpHeight = jumpHeight * 1.25f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        crosshair.SetActive(isShooting);
        if (boostActive)
        {
            acceleration = boostAcceleration;
            rotateSpeed = boostRotateSpeed;
            jumpHeight = boostjumpHeight;
            rb.angularDamping = boostDamping;
        }
        else
        {
            acceleration = normalAcceleration;
            rotateSpeed = normalRotateSpeed;
            jumpHeight = normalJumpHeight;
        }
        Rigidbody2D rbCrosshair = crosshair.GetComponent<Rigidbody2D>();
        RectTransform rtCrosshair = crosshair.GetComponent<RectTransform>();
        Transform upper = mech_upper.GetComponent<Transform>();
        Animator anim_upper = mech_upper.GetComponent<Animator>();
        Animator anim_legs = mech_legs.GetComponent<Animator>();
        Vector2 look = lookVector * lookSensitivity * Time.fixedDeltaTime;
        Vector3 moveDir =Vector3.zero;
        Vector3 velocity = rb.linearVelocity;
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;


        anim_upper.SetBool("IsShooting", isShooting);
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();
        
        //Fixed upper rotation only while shooting
        if (isShooting)
        {
            upper.rotation = tf.rotation;
            rbCrosshair.MovePosition(rbCrosshair.position + look);
        }
        yaw += lookVector.x * lookSensitivity * Time.deltaTime;
        pitch += lookVector.y * lookSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Rotate camera
        Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
        tf.rotation = Quaternion.Slerp(
            tf.rotation,
            targetRotation,
            Time.deltaTime * rotateSpeed);

        moveDir = camForward * zMove + camRight * xMove;
        Vector3 targetVelocity = moveDir * Time.fixedDeltaTime * acceleration;


        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        velocity.y -= gravity * Time.fixedDeltaTime;
        rb.linearVelocity = velocity;
        
        //Turn off boost when not moving
        if (rb.linearVelocity.sqrMagnitude < 5f)
        {
            boostActive = false;
        }

        bool isMoving = moveDir.sqrMagnitude > 0.01f;
        anim_upper.SetBool("IsWalking", isMoving);
        anim_legs.SetBool("IsWalking", isMoving);
        Rotate(moveDir);

    }
    void Rotate(Vector3 move)
    {
        Transform legs = mech_legs.GetComponent<Transform>();
        Transform upper = mech_upper.GetComponent<Transform>();

        if (isShooting || move.sqrMagnitude < .001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);

        legs.rotation = Quaternion.Slerp(
                legs.rotation,
                targetRotation,
                Time.deltaTime * rotateSpeed);
        upper.rotation = Quaternion.Slerp(
            upper.rotation,
            targetRotation,
            Time.deltaTime * rotateSpeed);

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log("Move X value:" + context.ReadValue<Vector2>().x);
        //Debug.Log("Move Y:Value" + context.ReadValue<Vector2>().y);
        zMove = context.ReadValue<Vector2>().y;
        xMove = context.ReadValue<Vector2>().x;
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        //Debug.Log("Look X value:" + context.ReadValue<Vector2>().x);
        //Debug.Log("Look Y:Value" + context.ReadValue<Vector2>().y);
        lookVector = context.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        boostActive = !boostActive;
        Debug.Log("Boost Pressed:" + boostActive);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump Pressed:" + context.ReadValueAsButton());

        if (!context.performed || !onGround) return;

        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x,
            jumpHeight,
            rb.linearVelocity.z
        );
        onGround = false;

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        bool attackPressed = context.ReadValueAsButton();
        isShooting = attackPressed;
        //Debug.Log("Attack Pressed:" + context.ReadValueAsButton());
        if (!context.canceled)
        {
            RectTransform rtCrosshair = crosshair.GetComponent<RectTransform>();
            rtCrosshair.anchoredPosition = Vector2.zero;
            return;
        } 
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            //Debug.Log("Is Grounded: " + onGround);
            onGround = true;
        }
    }


}

