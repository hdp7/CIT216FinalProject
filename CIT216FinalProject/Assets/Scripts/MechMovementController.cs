using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
public class MechMovementController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum MovementState { Ground, GroundBoost, Air, AirBoost };
    public MovementState state;


    private Transform tf;
    public Transform playerCamera;
    private Rigidbody rb;
    public GameObject mech_legs;
    public GameObject mech_upper;
    public GameObject cameraPivot;
    private PlayerInput playerInput;

    public float friction;
    public float rotateSpeed = 10f;
    public float acceleration = 1f;
    public float maxSpeed = 5f;
    public float jumpHeight = 5f;
    public float xMove;
    public float zMove;
    public Vector2 lookVector;
    private bool onGround;
    public float gravity;

    public float lookSensitivity = 10f;
    public float minPitch = -40f;
    public float maxPitch = 80f;

    float yaw;
    float pitch;

    void LateUpdate()
    {
        Transform upper = mech_upper.GetComponent<Transform>();

        yaw += lookVector.x * lookSensitivity * Time.deltaTime;
        pitch -= lookVector.y * lookSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        // Rotate upper
        upper.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Rotate camera
        cameraPivot.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }


    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
        playerInput = gameObject.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Animator anim_upper = mech_upper.GetComponent<Animator>();
        Animator anim_legs = mech_legs.GetComponent<Animator>();


        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * zMove + camRight * xMove;

        Vector3 velocity = rb.linearVelocity;
        Vector3 targetVelocity = moveDir * Time.fixedDeltaTime * acceleration;

        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        velocity.y -= gravity * Time.fixedDeltaTime;


        rb.linearVelocity = velocity;

        bool isMoving = moveDir.sqrMagnitude > 0.01f;
        anim_upper.SetBool("IsWalking", isMoving);
        anim_legs.SetBool("IsWalking", isMoving);

        //RotateLegs(moveDir);

    }


    //Rotates legs depending on movement 
    //void RotateLegs(Vector3 move)
    //{
    //    Transform legs = mech_legs.GetComponent<Transform>();


    //    if (move.sqrMagnitude < .001f)
    //        return;

    //    Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);

    //        mech_legs.transform.rotation = Quaternion.Slerp(
    //        mech_legs.transform.rotation,
    //        targetRotation,
    //        Time.deltaTime * rotateSpeed);
    //}


    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log("Move X value:" + context.ReadValue<Vector2>().x);
        //Debug.Log("Move Y:Value" + context.ReadValue<Vector2>().y);
        zMove = context.ReadValue<Vector2>().y;
        xMove = context.ReadValue<Vector2>().x;
    }
    //public void OnLook(InputAction.CallbackContext context)
    //{
    //    //Debug.Log("Look X value:" + context.ReadValue<Vector2>().x);
    //    //Debug.Log("Look Y:Value" + context.ReadValue<Vector2>().y);
    //    lookVector = context.ReadValue<Vector2>();
    //}

    public void OnBoost(InputAction.CallbackContext context)
    {
        Debug.Log("Boost Pressed:" + context.ReadValueAsButton());

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Pressed:" + context.ReadValueAsButton());

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
        Animator anim_upper = mech_upper.GetComponent<Animator>();
        Debug.Log("Attack Pressed:" + context.ReadValueAsButton());

        anim_upper.SetBool("IsShooting", attackPressed);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("Is Grounded: " + onGround);
            onGround = true;
        }
    }

}

