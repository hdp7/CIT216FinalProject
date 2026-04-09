 using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float velocity;
    private Rigidbody rb;
    public Transform source;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.transform.position = source.position;
        rb.linearVelocity = rb.linearVelocity + (source.localPosition * velocity);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
