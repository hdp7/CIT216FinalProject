using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
public class MechMovementController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum MovementState {Ground, GroundBoost, Air, AirBoost};
    public MovementState state;

    private Transform tf;
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
        rb.linearVelocity = new Vector3(rb.linearVelocity.x + xMove * acceleration, 0, rb.linearVelocity.z + zMove * acceleration);
        RotateLegs(new Vector2(xMove,zMove));
       
    }

    void RotateLegs(Vector2 move)
    {
        Transform legs = mech_legs.GetComponent<Transform>();

        if (move.sqrMagnitude < .001f)
            return;


        Vector3 direction = new Vector3(move.x - 90, 0f, move.y- 90);
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        legs.rotation = Quaternion.Slerp(
            legs.rotation,
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
        Debug.Log("Boost Pressed:" + context.ReadValueAsButton());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Pressed:" + context.ReadValueAsButton());
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + jumpHeight*acceleration, rb.linearVelocity.z);
    }

}

