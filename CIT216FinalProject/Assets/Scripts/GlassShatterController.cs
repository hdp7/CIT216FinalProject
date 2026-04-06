using System.Collections;
using UnityEngine;

public class GlassShatterController : MonoBehaviour
{
    private MeshRenderer mr;
    private bool isBroken = false;
    public ParticleSystem glassParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        mr = GetComponent<MeshRenderer>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shatter"))
        {
            StartCoroutine(Shatter());
        }
    }

    public IEnumerator Shatter()
    {
        mr.enabled = false;
        glassParticles.Play();
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
