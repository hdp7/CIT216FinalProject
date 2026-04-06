using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class MechMovementController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Transform tf;
    public Animator anim_legs;
    public Animator anim_chestarms;

    public float speed;
    private Vector3 move = new Vector3();

    void Start()
    {
        tf = gameObject.GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        tf.Translate(move * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = new Vector3(context.ReadValue<Vector2>().x * speed, 0, context.ReadValue<Vector2>().y);
    }
}
