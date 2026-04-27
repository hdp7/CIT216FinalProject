 using UnityEngine;
using UnityEngine.Events;

public class ProjectileController : MonoBehaviour
{
    public float velocity;
    private Rigidbody rb;
    public Transform source;
    public MechMovementController shooter;
    public UnityAction<float> attackListener;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //EventManager.StartListening("PlayerAttack" );
    }
    void OnEnable()
    {
        attackListener = new UnityAction<float>(Fire);
        //subscribes to the event
        EventManager.StartListening("PlayerAttack", attackListener);
    }
    void OnDisable()
    {

    }

    void Fire(float damage)
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos= new Vector3(0f, 0f, 1f) * velocity * Time.deltaTime;
        rb.AddForce(newPos);
    }
}
