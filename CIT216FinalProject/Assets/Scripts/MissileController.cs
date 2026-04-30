using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MissileController : MonoBehaviour
{
    private Rigidbody rb;
    //public MechMovementController shooter;
    //public UnityAction<float> attackListener;

    public float maxSpeed;
    public Vector3 force;
    public float damage; 
    public float lifetime;
    public GameObject explosion;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        rb.AddForce(force);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
