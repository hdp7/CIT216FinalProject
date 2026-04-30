//4/29/26
//Herman Pagan Alvarez
//Handles the glass's shattering effect seen on buildings
using System.Collections;
using UnityEngine;
public class GlassShatterController : MonoBehaviour
{
    private MeshRenderer mr;
    public ParticleSystem glassParticles;
    private BoxCollider col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();

    }
    private void OnTriggerEnter(Collider other)
    {
        //Only breaks when
        if(other.gameObject.CompareTag("Shatter"))
        {
            StartCoroutine(Shatter());
            //prevent repeat shatter effect
            col.enabled = false;
        }
        else if (other.gameObject.CompareTag("EnemyProj"))
        {
            StartCoroutine(Shatter());
            Destroy(other);
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
